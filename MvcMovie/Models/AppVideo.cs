//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcMovie.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class AppVideo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppVideo()
        {
            this.AppVideoUsers = new HashSet<AppVideoUser>();
        }
    
        public int VideoID { get; set; }
        public string VideoTitle { get; set; }
        public string VideoSubject { get; set; }
        public string VideoPath { get; set; }
        public string VideoDescription { get; set; }
        public Nullable<System.DateTime> uploadDate { get; set; }
        public HttpPostedFileBase VideoFile { get; set; }
        public bool isChecked { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppVideoUser> AppVideoUsers { get; set; }


    }
}
