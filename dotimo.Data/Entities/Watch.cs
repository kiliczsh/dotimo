using dotimo.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotimo.Data.Entities
{
    public class Watch : BaseEntity
    {
        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string UrlString { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public MonitoringTimePeriod MonitoringTimePeriod { get; set; }
        public int MonitoringTimePeriodId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}