// Services/SaleService.cs dosyasını bu şekilde güncelle:

using Eczanem.Api.Interfaces;
using Eczanem.Api.Models;

namespace Eczanem.Api.Services
{
    public class SaleService : ISaleService
    {
        private readonly IRepository<Sale> _saleRepository;
        // Stok işlemlerini yapabilmek için Repository'i buraya çağırmalıyız
        private readonly IRepository<Stock> _stockRepository;

        public SaleService(IRepository<Sale> saleRepository, IRepository<Stock> stockRepository)
        {
            _saleRepository = saleRepository;
            _stockRepository = stockRepository;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            // 1. ADIM: Önce stok kontrolü yapalım
            // Tüm stokları çekip filtrelemek yerine, Repository'ye özel bir metot yazmak daha doğru olurdu 
            // ama şimdilik mantığı kurmak için böyle ilerleyelim:
            var allStocks = await _stockRepository.GetAllAsync();

            var targetStock = allStocks.FirstOrDefault(s =>
                s.PharmacyId == sale.PharmacyId &&
                s.MedicineId == sale.MedicineId);

            // Stok hiç yoksa veya adet yetersizse hata fırlatabiliriz veya null dönebiliriz
            if (targetStock == null)
            {
                throw new Exception("Bu eczanede bu ilaçtan stok kaydı bulunamadı.");
            }

            if (targetStock.Quantity < sale.QuantitySold)
            {
                throw new Exception($"Yetersiz stok! Mevcut stok: {targetStock.Quantity}");
            }

            // 2. ADIM: Stoktan düşme işlemi
            targetStock.Quantity -= sale.QuantitySold;
            await _stockRepository.UpdateAsync(targetStock); // Repository'de UpdateAsync metodun var mı? (Aşağıda kontrol edelim)

            // 3. ADIM: Satışı kaydetme
            sale.SaleDate = DateTime.Now;
            await _saleRepository.AddAsync(sale);
            await _saleRepository.SaveChangesAsync();

            return sale;
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _saleRepository.GetAllAsync();
        }
    }
}