using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMate1.Models;

namespace WMate1.ViewModels
{
    public class CommentViewModel
    
    {
        public List<Comment> CommentsesList { get; set; }
        public Entry Entry { get; set; }
        public Comment Comment { get; set; }
    }
}