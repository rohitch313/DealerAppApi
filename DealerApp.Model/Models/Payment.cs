﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerApp.Model.Models
{
    public enum paymentStatus
    {
        Failed,
        Pending,
        Successful

    }

    public class Payment
    {

        [Key]

        public int Id { get; set; }
        public decimal Amount_Due { get; set; }

        public int CarId { get; set; } // Foreign key referencing CarId

        [ForeignKey("CarId")]
        // Navigation property for the related Car
        public virtual Car Car { get; set; }

        // CarName, Variant, and Image properties from Car
        public string CarName => Car?.CarName;
        public string Variant => Car?.Variant;
        public string Name => BankDetail.Name;
        public string AccountNumber => BankDetail.AccountNumber;


        public string IFSCCode => BankDetail.IFSCCode;

        public string BankName => BankDetail.BankName;

        public string PaymentProofImg { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? ProcessingCharges { get; set; }
        public decimal? Facility_Availed { get; set; }
        public decimal? Invoice_Charges { get; set; }

        public int Userid => Car.UserId;
        public int BankId { get; set; }
        [ForeignKey("BankId")]
        public virtual BankDetail BankDetail { get; set; }

        public ICollection<ProcDetails> procDetails { get; set; }
        public paymentStatus? PaymentStatus { get; set; }

    }
}
