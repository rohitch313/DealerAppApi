﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DealerApp.Model.Models
{
    public class PV_OpenMarket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? PurchaseAmount { get; set; }
        public string? TokenAmount { get; set; }
        public string? WithholdAmount { get; set; }

        [MaxLength(12)]
        public string? SellerContactNumber { get; set; }

        [EmailAddress]
        [RegularExpression(@"^[\w-]+@gmail\.(com|in)$", ErrorMessage = "Email must end with @gmail.com or @gmail.in")]
        public string? SellerEmailAddress { get; set; }
        public string? VehicleNumber { get; set; }
        public string? PaymentProof { get; set; }
        public string? SellerAdhaar { get; set; }
        public string? SellerPAN { get; set; }
        public string? PictureOfOriginalRC { get; set; }
        public string? OdometerPicture { get; set; }
        public string? VehiclePictureFromFront { get; set; }
        public string? VehiclePictureFromBack { get; set; }

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }
        public virtual UserInfo UserInfos { get; set; }

    }
}