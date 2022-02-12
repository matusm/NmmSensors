using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Bev.IO.NmmReader;

namespace NmmSensors
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Options options = new Options();

            if (!CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
                Console.WriteLine("*** ParseArgumentsStrict returned false");

            // get the filename(s)
            string[] fileNames = options.ListOfFileNames.ToArray();
            if (fileNames.Length == 0)
                ErrorExit("!Missing input file", 1);


            NmmFileName nmmFileName = new NmmFileName(fileNames[0]);
            nmmFileName.SetScanIndex(options.ScanIndex);
            NmmEnvironmentData sensors = new NmmEnvironmentData(nmmFileName);

            //testing
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine($"sample temperature: {sensors.AirSampleSource}");
            Console.WriteLine();

            sb.AppendLine($"Status: {sensors.AirSampleSourceText}");
            sb.AppendLine($"number of data points: {sensors.NumberOfAirSamples}");
            sb.AppendLine($"sample temperature:    {sensors.SampleTemperature:F3} °C [{sensors.SampleTemperatureDrift:F3} °C]");
            sb.AppendLine($"air temperature:       {sensors.AirTemperature:F3} °C [{sensors.AirTemperatureDrift:F3} °C] <{sensors.AirTemparatureGradient:F3} °C>");
            sb.AppendLine($"relative humidity:     {sensors.RelativeHumidity:F2} % [{sensors.RelativeHumidityDrift:F2} %]");
            sb.AppendLine($"barometric pressure:   {sensors.BarometricPressure:F0} Pa [{sensors.BarometricPressureDrift:F0} Pa]");
            sb.AppendLine($"air temperatures lasers   X:{sensors.XTemperature:F3} °C  Y:{sensors.YTemperature:F3} °C  Z:{sensors.ZTemperature:F3} °C");

            Console.WriteLine(sb.ToString());
        }

        static void ErrorExit(string message, int code)
        { }
    }
}
