//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationFPTGroup14.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Messenger
    {
        public string Receiver { get; set; }
        public System.DateTime SentTime { get; set; }
        public string Message { get; set; }
        public Nullable<bool> IsRead { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
