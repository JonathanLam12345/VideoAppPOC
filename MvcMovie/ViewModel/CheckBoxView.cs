using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMovie.ViewModel
{
    public class CheckBoxView
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int VideoID { get; set; }
        public string userName { get; set; }
        public bool isChecked { get; set; }
        public List<CheckBoxView> users { get; set; }
        public CheckBoxView()
        {
            users = new List<CheckBoxView>();
        }
        //public string videoTitle { get; set; }
    }
}