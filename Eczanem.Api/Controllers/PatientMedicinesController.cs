using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eczanem.Api.Data;
using Eczanem.Api.Models;

namespace Eczanem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientMedicinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientMedicinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. HASTANIN KULLANDIĞI İLAÇLARI LİSTELE
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var list = await _context.PatientMedicines
                                     .Include(x => x.Medicine) // İlaç adını gör
                                     .Where(x => x.PatientId == patientId && x.IsActive)
                                     .OrderBy(x => x.EstimatedEndDate) // Bitiş tarihine göre sırala
                                     .ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PatientMedicine pm)
        {
            // 1. İlacı Veritabanından Bul (Kutu İçi Adedi öğrenmek için)
            var medicine = await _context.Medicines.FindAsync(pm.MedicineId);

            if (medicine == null) return NotFound("İlaç bulunamadı.");

            // Formül: (Kutu İçi Adet / Günlük Kullanım)
            // Örn: 30 tablet / Günde 2 = 15 Gün gider.

            int dailyUsage = pm.DailyUsage > 0 ? pm.DailyUsage : 1;
            int daysLasts = medicine.PackageSize / dailyUsage;

            pm.StartDate = DateTime.Now;
            pm.EstimatedEndDate = pm.StartDate.AddDays(daysLasts);

            pm.IsNotificationSent = false;
            pm.IsActive = true;

            _context.PatientMedicines.Add(pm);
            await _context.SaveChangesAsync();
            return Ok(pm);
        }

        // 3. İLACI BIRAKTI / SİL
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.PatientMedicines.FindAsync(id);
            if (item == null) return NotFound();

            // Silmeyelim, pasife çekelim (Geçmişte kullandığı görünsün)
            item.IsActive = false;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // 4. BİLDİRİMLERİ GETİR (Bitişine 3 gün kalanlar)
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var today = DateTime.Now;
            var threeDaysLater = today.AddDays(3);

            var endingMedicines = await _context.PatientMedicines
                .Include(pm => pm.Patient)
                .Include(pm => pm.Medicine)
                .Where(pm => pm.IsActive
                             && !pm.IsNotificationSent
                             && pm.EstimatedEndDate <= threeDaysLater
                             && pm.EstimatedEndDate >= today) // Geçmiştekileri getirme, sadece yaklaşanları
                .ToListAsync();

            return Ok(endingMedicines);
        }
    }
}