using System.ComponentModel;

namespace dotimo.Data.Enums
{
    public enum MonitoringTimePeriod
    {
        [Description("Cancel monitoring")]
        Never = 0,
        Min1,
        Min3,
        Min5,
        Min15,
        Min30,
        Hour1,
        Hour7,
        Hour12,
        Daily,
        Weekly = 10
    }
}