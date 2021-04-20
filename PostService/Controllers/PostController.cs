using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Database;
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
        PostDbContext _context;

        public PostController(PostDbContext context)
        {
            _context = context;
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
                return NotFound(id);
            }
        }

        //Validate FromBody with logic classes to 
        [HttpPost]
        public ActionResult<Post> Create([FromBody] Post post)
        {
            _context.Posts.Add(post);

            try
            {
                Post newPost = _context.Posts.Find(post.Id);
                return Created($"Posts/{newPost.Id}", newPost);
            }
            catch(Exception)
            {
                return StatusCode(500, post);
            }
        }
    }
}
