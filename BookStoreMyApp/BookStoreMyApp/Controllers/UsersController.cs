#nullable disable
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreMyApp.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using BookStoreMyApp.Services;
using BookStoreMyApp.ViewModels.Queries;
using BookStoreMyApp.Handlers;
using BookStoreMyApp.Responses;
using BookStoreMyApp.ViewModels;
using AutoMapper;

namespace BookStoreMyApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BookstoreDBContext _context;
        private readonly IUriService _uriService;

        private readonly IMapper _mapper;

        public UsersController(BookstoreDBContext context, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;

            _uriService = uriService;
        }

      
        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery filter)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var validFilter = new PaginationQuery(filter.PageNumber, filter.PageSize, filter.Text);
            var route = Request.Path.Value;
            var pagedData = await _context.Users
                .Where(s =>
                s.FirstName!.Contains(filter.Text)||
                 s.LastName!.Contains(filter.Text)||
                  s.MiddleName!.Contains(filter.Text)
                )
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _context.Users.Where(s =>
                s.FirstName!.Contains(filter.Text) ||
                 s.LastName!.Contains(filter.Text) ||
                  s.MiddleName!.Contains(filter.Text)
                ).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<User>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new Response<User>(user)); ;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id,[FromBody] UserViewModel user)
        {
           

            _context.Entry(_mapper.Map<User>(user)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)

        {
            if (user.RoleId!=null)
            {
                var defoultRole=_context.Roles.Where(i=>i.RoleName== "user").FirstOrDefault();
                user.RoleId=defoultRole.RoleId;
                user.Role = defoultRole;
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User was deleted");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
