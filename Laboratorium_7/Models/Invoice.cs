using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laboratorium_7.Models
{
    public partial class Invoice
    {
        public int InvoiceId { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Invoice Date")]
        [DataType(DataType.DateTime)]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Billing Address")]
        public string? BillingAddress { get; set; }

        [Display(Name = "City")]
        public string? BillingCity { get; set; }

        [Display(Name = "State")]
        public string? BillingState { get; set; }

        [Display(Name = "Country")]
        public string? BillingCountry { get; set; }

        [Display(Name = "Postal Code")]
        public string? BillingPostalCode { get; set; }

        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();
    }
}
