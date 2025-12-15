using Eczanem.Api.Interfaces;
using Eczanem.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eczanem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] Sale sale)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // İş mantığı (stok düşme vb.) servisin içinde yapılacak
                var createdSale = await _saleService.CreateSaleAsync(sale);
                return Ok(createdSale);
            }
            catch (Exception ex)
            {
                // Eğer stok yetersizse servis hata fırlatacak, burada yakalayacağız
                return BadRequest(ex.Message);
            }
        }

        //  Dashboard için Günlük Ciro
        [HttpGet("today-total")]
        public async Task<IActionResult> GetTodayTotal()
        {
            var total = await _saleService.GetDailyTurnoverAsync();
            return Ok(total);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            try
            {
                await _saleService.DeleteSaleAsync(id);
                return Ok("Satış iptal edildi ve stok geri yüklendi.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}