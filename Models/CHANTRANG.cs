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
    public partial class CHANTRANG
    {
        public string ID { get; set; }
        public string NOIDUNG { get; set; }

        [DisplayName("Upload File")]
        public string HINHANHLOGO { get; set; }

        public HttpPostedFileBase HINHANHLOGOFile { get; set; }

    }
}
