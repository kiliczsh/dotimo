using dotimo.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace dotimo.Application.Models.Request
{
    public class WatchEditRequest
    {
        public Guid Id { get; set; }

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
    }
}