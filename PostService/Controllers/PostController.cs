using Microsoft.AspNetCore.Mvc;
using PostService.Database;
using PostService.MessageQueue;
using PostService.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        PostDbContext _context;
        RabbitMQHandler mQHandler;


        public PostController(PostDbContext context)
        {
            _context = context;
            mQHandler = new RabbitMQHandler("post");
        }

        [HttpGet("{id:length(24)}", Name = "GetPost")]
        public ActionResult<Post> Get(string id)
        {
            try
            {
                Post p = _context.Posts.Find(id);
                return Ok(p);
            }
            catch (Exception ex)
            {
                return BadRequest(id);
            }
        }

        [HttpGet]
        public ActionResult<Post[]> GetMultiple([FromQuery] string start, [FromQuery] string end)
        {
            try
            {
                Post[] posts = _context.Posts.Skip(int.Parse(start))
                                             .Take(int.Parse(end))
                                             .DefaultIfEmpty()
                                             .ToArray();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //TODO: Validate FromBody with logic classes too
        //TODO: Make the SendMessage asynchronuous
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();

            try
            {
                Post newPost = _context.Posts.Find(post.Id);
                newPost.CreatedAt = DateTime.Now;
                mQHandler.SendMessage(newPost);
                return Created($"Posts/{newPost.Id}", newPost);
            }
            catch(Exception)
            {
                return StatusCode(500, post);
            }
        }
        
        [HttpDelete("{id:length(24)}", Name = "DeletePost")]
        public ActionResult<Post> Delete(string id)
        {
            try
            {
                Post p = _context.Posts.Find(id);
                p.DeletedAt = DateTime.Now;
                return Ok(p);
            }
            catch (Exception ex)
            {
                return BadRequest(id);
            }
        }
    }
}
