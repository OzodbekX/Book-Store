using AutoMapper;
using BookStoreMyApp.Handlers;
using BookStoreMyApp.Models;
using BookStoreMyApp.Responses;
using BookStoreMyApp.Services;
using BookStoreMyApp.tempModels;
using BookStoreMyApp.ViewModels.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreMyApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class AuthorController : ControllerBase
    {
        private readonly BookstoreDBContext _context;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;


        public AuthorController(BookstoreDBContext context, IUriService uriService, IMapper mapper)
        {
            _context = context;
            _uriService = uriService;
            _mapper = mapper;
        }
        // GET: api/Author
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var validFilter = new PaginationQuery(filter.PageNumber, filter.PageSize, filter.Text);
            var route = Request.Path.Value;
            var pagedData = await _context.Authors
                .Where(s =>
                s.LastName!.Contains(filter.Text) ||
                 s.FirstName!.Contains(filter.Text)
                )
                .Include(s => s.BookAuthors)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToListAsync();

            var totalRecords = await _context.Authors.Where(s =>
                s.LastName!.Contains(filter.Text) ||
                 s.FirstName!.Contains(filter.Text)
                ).CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<Author>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

        // GET: api/Author/5
        [HttpGet("GetById")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(new Response<Author>(author));
        }

        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor([FromBody] AuthorViewModel author)
        {

            try
            {
                var isUnique = _context.Authors.Any(c => c.FirstName == author.FirstName && c.LastName == author.LastName);
                if (isUnique) return BadRequest(new Response<AuthorViewModel>(author, "Dublicate name error"));
                var _author = _mapper.Map<Author>(author);
                _author.BookAuthors = new List<BookAuthor>();
                await _context.Authors.AddAsync(_author);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetAuthor", new { id = _author.AuthorId }, _author);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

           
        }
        //[Authorize(Roles = "adminstrator")]

        [HttpPut]
        public async Task<ActionResult<Author>> PutBook([FromBody] Author author)
        {
            try
            {
                var _author = await _context.Authors.FirstOrDefaultAsync(e => e.AuthorId == author.AuthorId);
                _author.City = author.City;
                _author.Zip = author.Zip;
                _author.LastName = author.LastName;
                _author.FirstName = author.FirstName;
                _author.Phone = author.Phone;
                _author.Address = author.Address;
                _author.EmailAddress = author.EmailAddress;
                _context.SaveChanges();
                return  Ok(new Response<Author>(_author));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}