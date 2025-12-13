// Interfaces/ISaleService.cs
using Eczanem.Api.Models;

namespace Eczanem.Api.Interfaces
{
    public interface ISaleService
    {
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale> CreateSaleAsync(Sale sale);
    }
}