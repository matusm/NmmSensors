// a POCO

using System;

namespace NmmSensors
{
    public class SensorValues
    {
        public string Filename { get; set; }
        public string Status { get; set; }
        public int NumberOfSamples { get; set; }      
        public Quantity SampleTemperature { get; set; }
        public Quantity AirTemperature { get; set; }
        public Quantity Humidity { get; set; }
        public Quantity BarometricPressure { get; set; }
        public Quantity LXAirTemperature { get; set; }
        public Quantity LYAirTemperature { get; set; }
        public Quantity LZAirTemperature { get; set; }
    }

    public class Quantity
    {
        public double Average { get; set; }
        public double? Range { get; set; }
        public double? Gradient { get; set; }
        public string Unit { get; set; }
    }
}
