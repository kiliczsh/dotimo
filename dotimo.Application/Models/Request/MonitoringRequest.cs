﻿using dotimo.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotimo.Application.Models.Request
{
    public class MonitoringRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string UrlString { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public MonitoringTimePeriod MonitoringTimePeriod { get; set; }

        public int MonitoringTimePeriodId { get; set; }

    }
}
