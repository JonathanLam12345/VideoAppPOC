using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMovie.Models
{
    public class GetListPostRequest
    {
        public string UserId { get; set; }
    }

    public class GetListPostResult
    {
        public int VideoId { get; set; }
        public bool IsChecked { get; set; }
    }

    public class GetListVideosPostRequest
    {
        public string VideoId { get; set; }
    }

    public class GetListVideosPostResult
    {
        public int UserId { get; set; }
        public bool IsChecked { get; set; }
    }
}