using AutoMapper;
using BookStoreMyApp.Handlers;
using BookStoreMyApp.Responses;
using BookStoreMyApp.Services;
using BookStoreMyApp.ViewModels.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreMyApp.Models;
using BookStoreMyApp.ViewModels;
using System.Drawing;
namespace BookStoreMyApp.Models
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {

        private readonly BookstoreDBContext _context;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;


        public UserTaskController(BookstoreDBContext context, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var validFilter = new PaginationQuery(filter.PageNumber, filter.PageSize, filter.Text);
            var route = Request.Path.Value;
            var pagedData = await _context.UserTasks
                .Where(s =>
                s.TaskAddress!.Contains(filter.Text))
                //.Include(s => s.TaskFile)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _context.UserTasks.Where(s => s.UserTaskTittle!.Contains(filter.Text)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<UserTask>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);

        }
        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> Get([FromQuery]int userId,int fileId)
        {
            try
            {
                var isUnique = _context.UserTasks.Where(c => c.UserTaskId == fileId).Include(i=>i.TaskFile).FirstOrDefault();
                if (isUnique == null) return BadRequest("not found");
                return Ok(isUnique.TaskFile);
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
           

        }
        [HttpPost]
        public async Task<ActionResult<UserTask>> PostPublisher([FromForm] UserTaskView userTask)
        {

            try
            {
                var isUnique = _context.UserTasks.Any(c => c.TaskAddress == userTask.TaskAddress);
                if (isUnique) return BadRequest(new Response<UserTask>(_mapper.Map<UserTask>(userTask), "Dublicate name error"));
                var newUserTask = _mapper.Map<UserTask>(userTask);
                if (userTask.TaskFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await userTask.TaskFile.CopyToAsync(memoryStream);
                        // Upload the file if less than 2 MB
                        if (memoryStream.Length < 20971520)
                        {
                            //based on the upload file to create Photo instance.
                            //You can also check the database, whether the image exists in the database.

                            var newphoto = new TaskFile()
                            {

                                Bytes = memoryStream.ToArray(),
                                Description = userTask.TaskFile.FileName,
                                FileExtension = Path.GetExtension(userTask.TaskFile.FileName),
                                Size = userTask.TaskFile.Length,
                            };
                            //add the photo instance to the list.
                           newUserTask.TaskFile.Add(newphoto);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "The file is too large.");
                        }
                    }
                }
                var newAuthors = new List<string>();

                await _context.UserTasks.AddAsync(newUserTask);
                await _context.SaveChangesAsync();
                return Ok(new Response<UserTask>(_mapper.Map<UserTask>(userTask)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
