using Microsoft.AspNetCore.Mvc;
using PostService.Database;
using PostService.MessageQueue;
using PostService.Models;
using System;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        PostDbContext _context;
        RabbitMQHandler mQHandler;

        public CommentController()
        {
            _context = new PostDbContext();
            //mQHandler = new RabbitMQHandler("comment");
        }

        [HttpGet("{id:length(24)}", Name = "GetComment")]
        public ActionResult<Comment> Get(string id)
        {
            try
            {
                Comment c = _context.Comments.Find(id);
                return Ok(c);
            }
            catch (Exception ex)
            {
                return BadRequest(id);
            }
        }

        //TODO: Validate FromBody with logic classes too
        //TODO: Make the SendMessage asynchronuous
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Comment comment)
        {
            _context.Comments.Add(comment);

            try
            {
                Post newComment = _context.Comments.Find(comment.Id);
                newComment.CreatedAt = DateTime.Now;
                mQHandler.SendMessage(newComment);
                return Created($"Comments/{newComment.Id}", newComment);
            }
            catch (Exception)
            {
                return StatusCode(500, comment);
            }
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteComment")]
        public ActionResult<Post> Delete(string id)
        {
            try
            {
                Comment c = _context.Comments.Find(id);
                c.DeletedAt = DateTime.Now;
                return Ok(c);
            }
            catch (Exception ex)
            {
                return BadRequest(id);
            }
        }
    }
}
