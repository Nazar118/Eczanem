// Models/Medicine.cs
using System.ComponentModel.DataAnnotations;

namespace Eczanem.Api.Models
{
    public class Medicine
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Barcode { get; set; } = string.Empty; // İlaç barkodu

        [MaxLength(200)]
        public string? Manufacturer { get; set; } // Üretici firma

        public int Stock { get; set; } = 0;      // İlaç Adedi
        public decimal Price { get; set; } = 0;  // İlaç Fiyatı
    }
}