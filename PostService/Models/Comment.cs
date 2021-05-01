using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models
{
    public class Comment : Post
    {
        public string PostId { get; set; }
    }
}
