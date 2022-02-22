using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace NmmSensors
{
    class Options
    {
        [Option('s', "scan", DefaultValue = 0, HelpText = "Scan index for multi-scan files.")]
        public int ScanIndex { get; set; }

        [Option('j', "json", HelpText = "JSON output.")]
        public bool Json { get; set; }

        [Option('p', "pretty", HelpText = "JSON output pretty printed.")]
        public bool Pretty { get; set; }

        [Option('f', "full", HelpText = "Complete text output.")]
        public bool FullText { get; set; }

        [Option('b', "basic", HelpText = "Plain text output.")]
        public bool PlainText { get; set; }

        [Option("sCSV", HelpText = "Series of sample temperatures.")]
        public bool Scsv { get; set; }

        [Option("aCSV", HelpText = "Series of air temperatures.")]
        public bool Acsv { get; set; }

        [Option("comment", DefaultValue = "---", HelpText = "User supplied comment string.")]
        public string UserComment { get; set; }

        [ValueList(typeof(List<string>), MaximumElements = 2)]
        public IList<string> ListOfFileNames { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string AppVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            HelpText help = new HelpText
            {
                Heading = new HeadingInfo(AppName, "version " + AppVer),
                Copyright = new CopyrightInfo("Michael Matus", 2022),
                AdditionalNewLineAfterOption = false,
                AddDashesToOption = true
            };
            string sPre = "Program to extract environmental data from the scanning files produced by SIOS NMM-1.";
            help.AddPreOptionsLine(sPre);
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Usage: " + AppName + " filename [options]");
            help.AddPostOptionsLine("");

            help.AddOptions(this);

            return help;
        }


    }
}
