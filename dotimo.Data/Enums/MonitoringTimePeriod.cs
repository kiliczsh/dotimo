using System.ComponentModel;

namespace dotimo.Data.Enums
{
    public enum MonitoringTimePeriod : int
    {
        [Description("Cancel monitoring")]
        Never = 0,
        Min1 = 1,
        Min3 = 3,
        Min5 = 5,
        Min15 = 15,
        Min30 = 30,
        Hour1 = 60,
        Hour7 = 60 * 7,
        Hour12 = 60 * 12,
        Daily = 60 * 24,
        Weekly = 60 * 24 * 7
    }
}