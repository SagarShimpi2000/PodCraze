//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PodCraze.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Podcast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Pdf { get; set; }
        public double Amount { get; set; }
        public int IsPaid { get; set; }
        public string Narrator_username { get; set; }
        public string Audio { get; set; }
        public string Description { get; set; }
    
        public virtual Narrator Narrator { get; set; }
    }
}
