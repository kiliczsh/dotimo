using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotimo.Data.Entities
{
    public class CheckUp : BaseEntity
    {
        [Required]
        [ForeignKey("Watch")]
        public Guid WatchId { get; set; }

        public Watch Watch { get; set; }

        [Required]
        public short StatusCode { get; set; }

        public bool Success { get; set; }
    }
}