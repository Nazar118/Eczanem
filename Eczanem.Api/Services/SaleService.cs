using Eczanem.Api.Interfaces;
using Eczanem.Api.Models;

namespace Eczanem.Api.Services
{
    public class SaleService : ISaleService
    {
        private readonly IRepository<Sale> _saleRepository;
        // Artık sadece İlaç tablosuna bakmamız yeterli, çünkü stok ve fiyat orada!
        private readonly IRepository<Medicine> _medicineRepository;

        public SaleService(
            IRepository<Sale> saleRepository,
            IRepository<Medicine> medicineRepository)
        {
            _saleRepository = saleRepository;
            _medicineRepository = medicineRepository;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            // 1. ADIM: Satılacak İlacı Bul
            var medicine = await _medicineRepository.GetByIdAsync(sale.MedicineId);

            if (medicine == null)
            {
                throw new Exception("Satılmak istenen ilaç sistemde bulunamadı.");
            }

            // 2. ADIM: Stok Kontrolü (İlacın kendi stoğuna bakıyoruz)
            if (medicine.Stock < sale.QuantitySold)
            {
                throw new Exception($"Yetersiz stok! Mevcut stok: {medicine.Stock}, İstenen: {sale.QuantitySold}");
            }

            // 3. ADIM: Stoktan Düşme ve Güncelleme
            medicine.Stock -= sale.QuantitySold;
            await _medicineRepository.UpdateAsync(medicine); // İlacın yeni stoğunu kaydet

            // 4. ADIM: Satış Bilgilerini Tamamla
            sale.SaleDate = DateTime.Now;
            // Fiyatı da ilaçtan alıp hesaplıyoruz
            sale.TotalPrice = medicine.Price * sale.QuantitySold;

            // 5. ADIM: Satışı Kaydet
            await _saleRepository.AddAsync(sale);
            await _saleRepository.SaveChangesAsync();

            return sale;
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _saleRepository.GetAllAsync();
        }

        public async Task<decimal> GetDailyTurnoverAsync()
        {
            var allSales = await _saleRepository.GetAllAsync();
            var today = DateTime.Today;

            return allSales
                .Where(s => s.SaleDate >= today)
                .Sum(s => s.TotalPrice);
        }
        // ... Diğer kodların altındaki boşluğa ekle:

        public async Task DeleteSaleAsync(int id)
        {
            // 1. Silinecek satışı bul
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null) throw new Exception("Satış bulunamadı.");

            // 2. Satılan ilacı bul (Stok iadesi için)
            var medicine = await _medicineRepository.GetByIdAsync(sale.MedicineId);

            // Eğer ilaç hala sistemde kayıtlıysa stoğunu geri yükle
            if (medicine != null)
            {
                medicine.Stock += sale.QuantitySold; //  Stok iadesi
                await _medicineRepository.UpdateAsync(medicine);
            }

            // 3. Satış kaydını sil
            _saleRepository.Delete(sale);
            await _saleRepository.SaveChangesAsync();
        }
    }
}