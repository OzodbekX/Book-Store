using BookStoreMyApp.Handlers;
using BookStoreMyApp.Models;
using BookStoreMyApp.Responses;
using BookStoreMyApp.Services;
using BookStoreMyApp.ViewModels.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreMyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class QuestionsController : ControllerBase
    {
        private readonly BookstoreDBContext _context;
        private readonly IUriService _uriService;


        public QuestionsController(BookstoreDBContext context, IUriService uriService)
        {
            _context = context;
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
                var pagedData = await _context.Questions
                    .Where(s =>
                    s.TaskAddress!.Contains(filter.Text))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize).AsNoTracking()
                    .ToListAsync();
                Random r = new Random();

                pagedData.ForEach(x =>
                {

                    x.Answer = "null";
                    string[] answers = new string[] { x.Option1, x.Option2, x.Option3 };
                    var i1 = GenerateRandomNumber(3, new int[] { 3 });
                    x.Option1 = answers[i1];
                    var i2 = GenerateRandomNumber(3, new int[] { i1 });
                    x.Option2 = answers[i2];
                    var i3 = GenerateRandomNumber(3, new int[] { i1, i2 });
                    x.Option3 = answers[i3];

                });

                var totalRecords = await _context.Questions.Where(s => s.QuestionText!.Contains(filter.Text)).CountAsync();
                var pagedReponse = PaginationHelper.CreatePagedReponse<Question>(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        [HttpPost]
        public async Task<ActionResult<Question>> PostPublisher(Question question)
        {

            try
            {
                await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync();
                return Ok(new Response<Question>(question));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("CheckAnswer")]
        public async Task<ActionResult<bool>> CheckAnswer([FromQuery] int quizId, string check)
        {

            try
            {
                var question = _context.Questions.Where(q => q.QuestionId == quizId).FirstOrDefault();
                if (question != null)
                {
                    if (question.Answer == check) return Ok(true);
                    return Ok(false);

                }
                return BadRequest("item not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private  int GenerateRandomNumber(int max, int[] exclude)
        {
            var random = new Random();
            var i = exclude[0];
            while ( Array.IndexOf(exclude, i)>-1)
                i = random.Next(max);//Max range
            return i;
        }
    }
}
