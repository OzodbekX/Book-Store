#nullable disable
using Microsoft.AspNetCore.Mvc;
using BookStoreMyApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookStoreMyApp.ViewModels.Queries;
using BookStoreMyApp.Responses;
using BookStoreMyApp.Services;
using BookStoreMyApp.Handlers;
using BookStoreMyApp.ViewModels;
using AutoMapper;
using System.Drawing;
using System.Linq;

namespace BookStoreMyApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly BookstoreDBContext _context;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public BooksController(BookstoreDBContext context, IUriService uriService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Books

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
                .Include(d => d.BookAuthors.Where(i => i.AuthorId != null && i.BookId != null)).ThenInclude(i => i.Author)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _context.Books.Where(s => s.Title!.Contains(filter.Text)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Book>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);

        }


        // GET: api/Books/5
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


        //PUT: api/Books/5
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult<Book>> PutBook([FromForm] BookViewModel book)
        {

            try
            {
                var newBook = await _context.Books.FindAsync(book.BookId);
                newBook.Title = book.Title;
                newBook.Type = book.Type;
                newBook.Notes = book.Notes;
                newBook.Price = book.Price;
                newBook.Advance = book.Advance;
                newBook.Royalty = book.Royalty;
                newBook.YtdSales = book.YtdSales;
                newBook.PubId = book.PubId;
                newBook.Royalty = book.Royalty;
                newBook.PublishedDate = book.PublishedDate;

                List<Picture> photolist = new List<Picture>();
                if (book.Files.Count > 0)
                {
                    foreach (var formFile in book.Files)
                    {
                        if (formFile.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                // Upload the file if less than 2 MB
                                if (memoryStream.Length < 2097152)
                                {
                                    //based on the upload file to create Photo instance.
                                    //You can also check the database, whether the image exists in the database.
                                    var newphoto = new Picture()
                                    {
                                        Bytes = memoryStream.ToArray(),
                                        Description = formFile.FileName,
                                        FileExtension = Path.GetExtension(formFile.FileName),
                                        Size = formFile.Length,
                                    };
                                    //add the photo instance to the list.
                                    photolist.Add(newphoto);
                                }
                                else
                                {
                                    ModelState.AddModelError("File", "The file is too large.");
                                }
                            }
                        }
                    }
                }
                newBook.Pictures = photolist;
                newBook.BookAuthors.Clear();
                var alreadyHas = _context.BookAuthors.Where(a => a.BookId == book.BookId);
                var authorIds = book.AuthorId.Split(",").ToList<string>();
                var delete = alreadyHas.Where(a => !authorIds.Any(u => u == a.AuthorId.ToString()));
                _context.BookAuthors.RemoveRange(delete);
                authorIds.ForEach(p =>
                {
                    var UId = Int32.Parse(p);
                    if (!alreadyHas.Any(i => i.AuthorId == UId))
                    {
                        var newBookAuthor = new BookAuthor()
                        {
                            AuthorId = UId,
                            AuthorOrder = book.AuthorOrder,
                            RoyaltyPercentage = book.RoyaltyPercentage,
                        };
                        newBook.BookAuthors.Add(newBookAuthor);
                    };
                });

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(new Response<BookViewModel>(book));
                }
                catch (Exception ex)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromForm] BookViewModel book)
        {
            try
            {

                var isUnique = _context.Books.Any(c => c.Title == book.Title);
                if (isUnique) return BadRequest(new Response<BookViewModel>(book, "Dublicate name error"));
                isUnique = _context.Books.Any(c => c.BookId == book.BookId);
                List<Picture> photolist = new List<Picture>();
                var entity = _mapper.Map<Book>(book);
                if (book.Files.Count > 0)
                {
                    foreach (var formFile in book.Files)
                    {
                        if (formFile.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(memoryStream);
                                // Upload the file if less than 2 MB
                                if (memoryStream.Length < 2097152)
                                {
                                    //based on the upload file to create Photo instance.
                                    //You can also check the database, whether the image exists in the database.
                                    var newphoto = new Picture()
                                    {
                                        Bytes = memoryStream.ToArray(),
                                        Description = formFile.FileName,
                                        FileExtension = Path.GetExtension(formFile.FileName),
                                        Size = formFile.Length,
                                    };
                                    //add the photo instance to the list.
                                    photolist.Add(newphoto);
                                }
                                else
                                {
                                    ModelState.AddModelError("File", "The file is too large.");
                                }
                            }
                        }
                    }
                }
                entity.Pictures = photolist;
                var newAuthors = new List<string>();
                book.AuthorId.Split(',').ToList().ForEach(p =>
                {
                    var newBookAuthor = new BookAuthor()
                    {
                        AuthorId = Int16.Parse(p),
                        AuthorOrder = book.AuthorOrder,
                        RoyaltyPercentage = book.RoyaltyPercentage,
                    };
                    entity.BookAuthors.Add(newBookAuthor);
                });


                await _context.Books.AddAsync(_mapper.Map<Book>(entity));
                await _context.SaveChangesAsync();
                return Ok(new Response<BookViewModel>(book));

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            };
        }


        [HttpPut]
        [Route("rating")]
        public async Task<IActionResult> ChangeRating([FromQuery] int bookId, float rating)
        {
            try
            {
                var book = await _context.Books.FindAsync(bookId);
                if (book == null) return NotFound();
                if (book.Rating != null)
                {
                    book.Rating = (book.Rating + rating) / 2;
                }
                else book.Rating = rating;

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
    public record UrlQueryParameters(int Limit = 50, int Page = 1);
}
