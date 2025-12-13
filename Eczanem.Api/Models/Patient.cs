// Models/Patient.cs
using System.ComponentModel.DataAnnotations;

namespace Eczanem.Api.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(11)] // TC Kimlik No gibi düşünülebilir
        public string TcNo { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        // Hastanın aldığı ilaç geçmişini buradan takip edebiliriz (İsteğe bağlı navigasyon)
        // public ICollection<Sale> Sales { get; set; }
    }
} 