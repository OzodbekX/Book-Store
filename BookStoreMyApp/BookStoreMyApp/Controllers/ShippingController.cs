using AutoMapper;
using BookStoreMyApp.Models;
using BookStoreMyApp.Responses;
using BookStoreMyApp.Services;
using BookStoreMyApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreMyApp.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly BookstoreDBContext _context;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ShippingController(BookstoreDBContext context, IUriService uriService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public async Task<ActionResult<Shipping>> PostShipping([FromForm] ShippingViewCreateModel shipping)
        {
            try
            {
                var entity = new Shipping()
                {
                    Address = shipping.Address,
                    PostalCode = shipping.PostalCode,
                    ShippingState = ShippingState.Created,
                };
                var list = shipping.SaleData.Split(',').ToList();
                list.ForEach(x =>
                {
                    var previous = entity.Sales.Where(s=> s.BookId== Int32.Parse(x)).FirstOrDefault();
                    if (previous==null)
                    {
                        var book = _context.Books.Where(i => i.BookId == Int32.Parse(x)).FirstOrDefault();
                        if (book != null)
                        {
                            var sale = new Sale()
                            {
                                OrderNum = new Random().Next(0, 1000000).ToString("D6"),
                                OrderDate = DateTime.Now,
                                PayTerms = shipping.PayTerms,
                                BookId = book.BookId,
                                UserId = shipping.UserId,
                                Quantity = 1,
                                SpecialDiscount = shipping.SpecialDiscount,
                                StoreId='0'.ToString(),
                            };
                            entity.Sales.Add(sale);
                        }
                    }
                    else
                    {
                        previous.Quantity++;
                    }

                });

                await _context.Shippings.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Ok(new Response<Shipping>(entity));

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            };
        }



    }
}
