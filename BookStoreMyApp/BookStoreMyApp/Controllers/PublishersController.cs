#nullable disable
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreMyApp.Models;
using Microsoft.AspNetCore.Authorization;
using BookStoreMyApp.ViewModels.Queries;
using BookStoreMyApp.Handlers;
using BookStoreMyApp.Services;
using BookStoreMyApp.Responses;
using BookStoreMyApp.ViewModels;
using AutoMapper;

namespace BookStoreMyApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly BookstoreDBContext _context;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public PublishersController(BookstoreDBContext context, IMapper mapper,  IUriService uriService)
        {
            _context = context;
            _mapper = mapper;

            _uriService = uriService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery filter)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var validFilter = new PaginationQuery(filter.PageNumber, filter.PageSize, filter.Text);

                var route = Request.Path.Value;
                var pagedData = await _context.Publishers
                    .Where(s =>
                    s.PublisherName!.Contains(filter.Text))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
                var totalRecords = await _context.Publishers.Where(s => s.PublisherName!.Contains(filter.Text)).CountAsync();

                var pagedReponse = PaginationHelper.CreatePagedReponse<Publisher>(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Publishers/5
        [HttpGet("GetPublisherDetails/{id}")]
        public async Task<ActionResult<Publisher>> GetPublisherDetails(int id)
        {
            var publisher = _context.Publishers
                .Include(pub => pub.Books).ThenInclude(book => book.Sales)
                .Include(pub => pub.Users)
                .Where(pub => pub.PubId == id).FirstOrDefault();

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // PUT: api/Publishers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutPublisher(int id, PublisherViewModel publisher)
        {
         

            _context.Entry(_mapper.Map<Publisher>(publisher)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new Response<PublisherViewModel>(publisher));
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Publisher>> PutPublisher(PublisherViewModel publisher)
        {

            try
            {
                await _context.Publishers.AddAsync(_mapper.Map<Publisher>(publisher));
                await _context.SaveChangesAsync();
                return Ok(new Response<Publisher>(_mapper.Map<Publisher>(publisher)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.PubId == id);
        }
    }

}
