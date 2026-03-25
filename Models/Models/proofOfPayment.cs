using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class proofOfPayment
    {
        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public byte[] Bytes { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public decimal Size { get; set; }

        public DateTime dateCreated {get;set;}

    }
}
