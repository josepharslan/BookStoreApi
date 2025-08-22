using BookStore.BusinessLayer.Abstract;
using BookStore.DtoLayer.Dtos.QuoteDtos;
using BookStore.EntityLayer.Concrete;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuotesController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }
        [HttpGet]
        public IActionResult QuoteList()
        {
            var value = _quoteService.TGetAll();
            return Ok(value);
        }
        [HttpPost]
        public IActionResult CreateQuote(CreateQuoteDto quote)
        {
            var value = quote.Adapt<Quote>();
            _quoteService.TAdd(value);
            return Ok("Ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteQuote(int id)
        {
            _quoteService.TDelete(id);
            return Ok("Silme işlemi başarılı");
        }
        [HttpPut]
        public IActionResult UpdateQuote(UpdateQuoteDto updateQuoteDto)
        {
            var value = updateQuoteDto.Adapt<Quote>();
            _quoteService.TUpdate(value);
            return Ok("Güncelleme işlemi başarılı");
        }
        [HttpGet("GetQuote")]
        public IActionResult GetQuote(int id)
        {
            return Ok(_quoteService.TGetById(id));
        }
        [HttpGet("GetLast")]
        public IActionResult GetLastQuote()
        {
            return Ok(_quoteService.TGetLast());
        }
    }
}
