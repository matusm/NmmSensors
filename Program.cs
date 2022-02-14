﻿using System;
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

            NmmFileName nmmFileName = new NmmFileName(fileNames[0]);
            nmmFileName.SetScanIndex(options.ScanIndex);
            NmmEnvironmentData nmmSensors = new NmmEnvironmentData(nmmFileName);

            // wrap all data in a POCO
            SensorValues sensorValues = new SensorValues
            {
                FileName = nmmFileName.BaseFileName,
                NumberOfSamples = nmmSensors.NumberOfAirSamples,
                Status = nmmSensors.AirSampleSourceText,
                SampleTemperature = new Quantity
                {
                    Average = nmmSensors.SampleTemperature,
                    Range = nmmSensors.SampleTemperatureDrift,
                    Unit = "°C"
                },
                AirTemperature = new Quantity
                {
                    Average = nmmSensors.AirTemperature,
                    Range = nmmSensors.AirTemperatureDrift,
                    Gradient = nmmSensors.AirTemparatureGradient,
                    Unit = "°C"
                },
                Humidity = new Quantity
                {
                    Average = nmmSensors.RelativeHumidity,
                    Range = nmmSensors.RelativeHumidityDrift,
                    Unit = "%"
                },
                BarometricPressure = new Quantity
                {
                    Average = nmmSensors.BarometricPressure,
                    Range = nmmSensors.BarometricPressureDrift,
                    Unit = "Pa"
                },
                LXAirTemperature = new Quantity 
                {
                    Average= nmmSensors.XTemperature,
                    Unit="°C"
                },
                LYAirTemperature = new Quantity
                {
                    Average = nmmSensors.YTemperature,
                    Unit = "°C"
                },
                LZAirTemperature = new Quantity
                {
                    Average = nmmSensors.ZTemperature,
                    Unit = "°C"
                },
            };

            var serializerOptions = new JsonSerializerOptions {
                WriteIndented = true,
                //IgnoreNullValues = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = JsonSerializer.Serialize(sensorValues, serializerOptions);

            string verboseString = ToText(sensorValues);

            Console.WriteLine(jsonString);
            Console.WriteLine();
            Console.WriteLine(verboseString);

        }

        private static string ToText(SensorValues poco)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Status text: {poco.Status}");
            sb.AppendLine($"number of data points:      {poco.NumberOfSamples}");
            sb.AppendLine($"sample temperature / {poco.SampleTemperature.Unit}:    {poco.SampleTemperature.Average:F3} ({poco.SampleTemperature.Range:F3})");
            sb.AppendLine($"air temperature / °C:       {poco.AirTemperature.Average:F3} ({poco.AirTemperature.Range:F3}) [{poco.AirTemperature.Gradient:F3}]");
            sb.AppendLine($"relative humidity / %:      {poco.Humidity.Average:F2} ({poco.Humidity.Range:F2})");
            sb.AppendLine($"barometric pressure / Pa:   {poco.BarometricPressure.Average:F0} ({poco.BarometricPressure.Range:F0})");
            return sb.ToString();
        }

        static void ErrorExit(string message, int code)
        {
            Console.WriteLine($"{message} (error code {code})");
            Environment.Exit(code);
        }

        static double Round (double value, int decimals)
        {
            int temp = (int)(value * 1000);
            return (double)temp / 1000;
        }
    }
}
