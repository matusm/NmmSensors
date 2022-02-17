using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

            string filename = fileNames[0];
            NmmFileName nmmFileName = new NmmFileName(filename);
            nmmFileName.SetScanIndex(options.ScanIndex);
            NmmEnvironmentData nmmEnvironment = new NmmEnvironmentData(nmmFileName);

            // wrap all data in a POCO
            SensorValues sensorValues = new SensorValues
            {
                Filename = nmmFileName.BaseFileName,
                NumberOfSamples = nmmEnvironment.NumberOfAirSamples,
                Status = nmmEnvironment.AirSampleSourceText,
                SampleTemperature = new Quantity
                {
                    Average = nmmEnvironment.SampleTemperature,
                    Range = nmmEnvironment.SampleTemperatureDrift,
                    Unit = "°C"
                },
                AirTemperature = new Quantity
                {
                    Average = nmmEnvironment.AirTemperature,
                    Range = nmmEnvironment.AirTemperatureDrift,
                    Gradient = nmmEnvironment.AirTemparatureGradient,
                    Unit = "°C"
                },
                Humidity = new Quantity
                {
                    Average = nmmEnvironment.RelativeHumidity,
                    Range = nmmEnvironment.RelativeHumidityDrift,
                    Unit = "%"
                },
                BarometricPressure = new Quantity
                {
                    Average = nmmEnvironment.BarometricPressure,
                    Range = nmmEnvironment.BarometricPressureDrift,
                    Unit = "Pa"
                },
                LXAirTemperature = new Quantity 
                {
                    Average= nmmEnvironment.XTemperature,
                    Unit="°C"
                },
                LYAirTemperature = new Quantity
                {
                    Average = nmmEnvironment.YTemperature,
                    Unit = "°C"
                },
                LZAirTemperature = new Quantity
                {
                    Average = nmmEnvironment.ZTemperature,
                    Unit = "°C"
                },
            };

            Console.WriteLine(GetOutput(sensorValues, OutputStyleOption.Json));
        }

        /*********************************************************************************/

        private static string GetOutput(SensorValues poco, OutputStyleOption outputStyle)
        {
            string outputText = string.Empty;
            if(outputStyle == OutputStyleOption.Json || outputStyle == OutputStyleOption.JsonPretty)
            {
                var serializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = false,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                if (outputStyle == OutputStyleOption.JsonPretty)
                    serializerOptions.WriteIndented = true;
                outputText = JsonSerializer.Serialize(poco, serializerOptions);
                return outputText;
            }
            outputText = TextSerialize(poco, outputStyle);
            return outputText;
        }

        /*********************************************************************************/

        private static string TextSerialize(SensorValues poco, OutputStyleOption outputStyle)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Status: {poco.Status}");
            sb.AppendLine($"number of data points:      {poco.NumberOfSamples}");
            sb.AppendLine($"sample temperature / {poco.SampleTemperature.Unit}:    {poco.SampleTemperature.Average:F3} ({poco.SampleTemperature.Range:F3})");
            sb.AppendLine($"air temperature / {poco.AirTemperature.Unit}:       {poco.AirTemperature.Average:F3} ({poco.AirTemperature.Range:F3}) [{poco.AirTemperature.Gradient:F3}]");
            sb.AppendLine($"relative humidity / {poco.Humidity.Unit}:      {poco.Humidity.Average:F2} ({poco.Humidity.Range:F2})");
            sb.AppendLine($"barometric pressure / {poco.BarometricPressure.Unit}:   {poco.BarometricPressure.Average:F0} ({poco.BarometricPressure.Range:F0})");
            return sb.ToString();
        }

        /*********************************************************************************/

        static void ErrorExit(string message, int code)
        {
            Console.WriteLine($"{message} (error code {code})");
            Environment.Exit(code);
        }

    }

    public enum OutputStyleOption
    {
        None,
        Json,
        JsonPretty,
        Plain,
        Full,
        SampleOnly
    }
}
