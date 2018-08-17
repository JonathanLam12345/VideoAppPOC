using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace MvcMovie.ViewModel {
    public class table{
       public string videoPath { get; set; }
        //[System.Data.Linq.Mapping.Column(Storage = "VideoID", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int videoID { get; set; }
        public DateTime du { get; set; }
        public string title { get; set; }
        public string subject { get; set; }
        public int userID { get; set; }
        //public string userName { get; set; }
    }
}