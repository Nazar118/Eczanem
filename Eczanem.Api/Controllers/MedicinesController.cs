using Microsoft.AspNetCore.Mvc;
using Eczanem.Api.Models; // Model klasörünü kullandığımızdan emin olalım

namespace Eczanem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        // Geçici veritabanımız (Static List)
        private static List<Medicine> _medicines = new List<Medicine>
        {
            new Medicine { Id = 1, Name = "Parol 500mg", Barcode = "86912345", Manufacturer = "Atabay" },
            new Medicine { Id = 2, Name = "Aspirin", Barcode = "86998765", Manufacturer = "Bayer" }
        };

        // 1. LİSTELEME (GET)
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_medicines);
        }

        // 2. EKLEME (POST)
        [HttpPost]
        public IActionResult Add(Medicine medicine)
        {
            // Yeni ID oluştur (En son ID + 1)
            int newId = _medicines.Any() ? _medicines.Max(m => m.Id) + 1 : 1;
            medicine.Id = newId;

            _medicines.Add(medicine);
            return Ok(medicine);
        }

        // 3. SİLME (DELETE) <-- YENİ
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var medicine = _medicines.FirstOrDefault(m => m.Id == id);
            if (medicine == null) return NotFound("İlaç bulunamadı");

            _medicines.Remove(medicine);
            return Ok();
        }

        // 4. GÜNCELLEME (PUT) <-- YENİ
        [HttpPut("{id}")]
        public IActionResult Update(int id, Medicine updatedMedicine)
        {
            var existingMedicine = _medicines.FirstOrDefault(m => m.Id == id);
            if (existingMedicine == null) return NotFound("İlaç bulunamadı");

            // Bilgileri güncelle
            existingMedicine.Name = updatedMedicine.Name;
            existingMedicine.Barcode = updatedMedicine.Barcode;
            existingMedicine.Manufacturer = updatedMedicine.Manufacturer;

            return Ok(existingMedicine);
        }
    }
}