using System;

namespace GlucoMan
{
    public class DeviceModel
    {
        public int? IdDeviceModel { get; set; }
        public string Name { get; set; }
        public int? IdTypeOfDevice { get; set; }
        public string Description { get; set; }

        public DeviceModel() { }
    }
}
