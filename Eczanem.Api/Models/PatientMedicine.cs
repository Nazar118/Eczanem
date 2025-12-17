using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eczanem.Api.Models
{
    public class PatientMedicine
    {
        public int Id { get; set; }

        // Hangi Hasta?
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }

        // Hangi İlaç?
        public int MedicineId { get; set; }
        [ForeignKey("MedicineId")]
        public Medicine? Medicine { get; set; }

        // Kullanım Sıklığı (Günde kaç adet?)
        public int DailyUsage { get; set; } = 1;

        // İlaç Başlangıç Tarihi (En son ne zaman aldı?)
        public DateTime StartDate { get; set; } = DateTime.Now;

        // Bitiş Tarihi (Sistem bunu otomatik hesaplayacak)
        // (Stok / Günlük Kullanım + Başlangıç Tarihi)
        public DateTime EstimatedEndDate { get; set; }

        // Bildirim Gönderildi mi? (Tekrar tekrar atmamak için)
        public bool IsNotificationSent { get; set; } = false;

        // Aktif mi? (Hasta ilacı bırakmış olabilir)
        public bool IsActive { get; set; } = true;
    }
}