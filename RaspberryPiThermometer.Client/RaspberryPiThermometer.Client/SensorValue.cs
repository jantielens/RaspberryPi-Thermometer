using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPiThermometer.Client
{
    public class SensorValue
    {
        public string Id { get; set; }
        public string SensorName { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
