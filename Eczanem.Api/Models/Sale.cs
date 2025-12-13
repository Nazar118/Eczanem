// Models/Sale.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eczanem.Api.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now; // Satış Tarihi

        [Required]
        public int QuantitySold { get; set; } // Kaç kutu satıldı?

        // İlişkiler

        public int PatientId { get; set; } // Kime satıldı?
        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }

        public int MedicineId { get; set; } // Hangi ilaç satıldı?
        [ForeignKey("MedicineId")]
        public Medicine? Medicine { get; set; }

        public int PharmacyId { get; set; } // Hangi eczane sattı?
        [ForeignKey("PharmacyId")]
        public Pharmacy? Pharmacy { get; set; }
    }
}