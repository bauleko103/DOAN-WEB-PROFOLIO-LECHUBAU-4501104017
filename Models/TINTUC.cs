//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NATURALLIFE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web;
    public partial class TINTUC
    {
        public string ID { get; set; }
        public string IDTHELOAI { get; set; }
        public string TIEUDE { get; set; }
        public string NOIDUNGPHU { get; set; }

        public string NOIDUNGCHINH { get; set; }

        public Nullable<System.DateTime> NGAYTHANG { get; set; }

        public virtual THELOAITINTUC THELOAITINTUC { get; set; }
        [DisplayName("Upload File")]
        public string HINHANH { get; set; }

        public HttpPostedFileBase HINHANHFile { get; set; }
    }
}
