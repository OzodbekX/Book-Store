using AutoMapper;
using BookStoreMyApp.Handlers;
using BookStoreMyApp.Models;
using BookStoreMyApp.Responses;
using BookStoreMyApp.Services;
using BookStoreMyApp.ViewModels;
using BookStoreMyApp.ViewModels.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreMyApp.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    //[EnableCors("_myAllowSpecificOrigins")]

    [ApiController]
    public class ClientBooksController : ControllerBase
    {
        private readonly BookstoreDBContext _context;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ClientBooksController(BookstoreDBContext context, IUriService uriService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var book = await _context.Books.AsNoTracking()
                .Include(i => i.Pictures)
                .Include(i => i.BookAuthors).ThenInclude(i => i.Author)
                .FirstOrDefaultAsync(o => o.BookId == id);

                if (book == null)
                {
                    return NotFound();
                }
                return Ok(new Response<Book>(book));    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("CreateUser")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserViewModel user)

        {
            try
            {
                var defoultRole = _context.Roles.Where(i => i.RoleName == "user").FirstOrDefault();
                var userToDatabase = _mapper.Map<User>(user);
                userToDatabase.RoleId = defoultRole.RoleId;
                userToDatabase.Role = defoultRole;
                _context.Users.Add(userToDatabase);
                await _context.SaveChangesAsync();
                var loginParam = new LoginRequest();
                loginParam.EmailAddress = user.EmailAddress;
                loginParam.Password = user.Password;
                return RedirectToAction("Login","Authenticate", loginParam);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [AllowAnonymous]

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var validFilter = new PaginationQuery(filter.PageNumber, filter.PageSize, filter.Text);
            var route = Request.Path.Value;
            var pagedData = await _context.Books
                .Where(s =>
                s.Title!.Contains(filter.Text))
                .Include(s => s.Pictures)
                .Include(d => d.BookAuthors.Where(i => i.AuthorId != null && i.BookId != null)).ThenInclude(i => i.Author)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _context.Books.Where(s => s.Title!.Contains(filter.Text)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Book>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);

        }

    }
}
