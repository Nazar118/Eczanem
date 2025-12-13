using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Veritabanı işlemleri için şart
using Eczanem.Api.Data;
using Eczanem.Api.Models;

namespace Eczanem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        // Veritabanı Bağlantımız
        private readonly ApplicationDbContext _context;

        public MedicinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME (GET)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Veritabanındaki "Medicines" tablosundan hepsini çek
            var medicines = await _context.Medicines.ToListAsync();
            return Ok(medicines);
        }

        // 2. EKLEME (POST)
        [HttpPost]
        public async Task<IActionResult> Add(Medicine medicine)
        {
            // Veritabanına ekle ve kaydet
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync(); // Kaydetmeden ID oluşmaz
            return Ok(medicine);
        }

        // 3. SİLME (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return NotFound("İlaç bulunamadı");

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // 4. GÜNCELLEME (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Medicine updatedMedicine)
        {
            var existingMedicine = await _context.Medicines.FindAsync(id);
            if (existingMedicine == null) return NotFound("İlaç bulunamadı");

            // Bilgileri güncelle
            existingMedicine.Name = updatedMedicine.Name;
            existingMedicine.Barcode = updatedMedicine.Barcode;
            existingMedicine.Manufacturer = updatedMedicine.Manufacturer;
            existingMedicine.Stock = updatedMedicine.Stock;
            existingMedicine.Price = updatedMedicine.Price;

            await _context.SaveChangesAsync(); // Değişiklikleri veritabanına yaz
            return Ok(existingMedicine);
        }
    }
}