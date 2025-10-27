using System;

namespace GlucoMan
{
    public class Device
    {
        public int? IdDevice { get; set; }
        public string PhysicalCode { get; set; }
        public int? IdDeviceModel { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }

        public Device() { }
    }
}
