//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public Nullable<int> RequestId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<decimal> ItemPrice { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<decimal> ServicePrice { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public decimal Total { get; set; }

        public virtual Item Item { get; set; }
        public virtual Service Service { get; set; }
        public virtual UserRegistration UserRegistration { get; set; }
    }
}
