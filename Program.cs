using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bev.IO.NmmReader;
using Bev.UI;

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
            if (options.BeQuiet == true)
                ConsoleUI.BeSilent();
            else
                ConsoleUI.BeVerbatim();
            ConsoleUI.Welcome();

            // get the filename(s)
            string[] fileNames = options.ListOfFileNames.ToArray();
            if (fileNames.Length == 0)
                ConsoleUI.ErrorExit("!Missing input file", 1);

            ConsoleUI.ReadingFile(fileNames[0]);
            NmmFileName nmmFileName = new NmmFileName(fileNames[0]);
            nmmFileName.SetScanIndex(options.ScanIndex);
            NmmEnvironmentData sensors = new NmmEnvironmentData(nmmFileName);
            ConsoleUI.Done();

            //testing
            Console.WriteLine($"number of data points: {sensors.NumberOfAirSamples}");
            Console.WriteLine($"sample temperature: {sensors.SampleTemperature:F3} °C");
            Console.WriteLine($"sample temperature drift: {sensors.SampleTemperatureDrift:F3} °C");
            Console.WriteLine($"sample temperature: {sensors.AirSampleSource}");
            Console.WriteLine($"sample temperature: {sensors.AirSampleSourceText}");

        }
    }
}
