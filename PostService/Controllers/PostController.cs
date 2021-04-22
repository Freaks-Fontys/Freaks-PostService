using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Database;
using PostService.MessageQueue;
using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        PostDbContext _context = new PostDbContext();
        RabbitMQHandler mQHandler = new RabbitMQHandler("post");


        public PostController()
        {

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

        //TODO: Validate FromBody with logic classes to
        //TODO: Make the SendMessage asynchronuous
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            PostDbContext _context = new PostDbContext();
            _context.Posts.Add(post);

            try
            {
                Post newPost = _context.Posts.Find(post.Id);
                mQHandler.SendMessage(newPost);
                return Created($"Posts/{newPost.Id}", newPost);
            }
            catch(Exception)
            {
                return StatusCode(500, post);
            }
        }
    }
}
