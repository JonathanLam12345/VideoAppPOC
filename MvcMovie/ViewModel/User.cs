using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcMovie.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcMovie.ViewModel {
    public class User {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "this field is required")]
        public string userName { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "this field is required")]
        public string password { get; set; }
        public string LoginErrorMessage { get; set; }
    }
}