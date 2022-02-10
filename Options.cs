using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace NmmSensors
{
    class Options
    {
        [Option('s', "scan", DefaultValue = 0, HelpText = "Scan index for multi-scan files.")]
        public int ScanIndex { get; set; }

        [Option('q', "quiet", HelpText = "Quiet mode. No screen output (except for errors).")]
        public bool BeQuiet { get; set; }

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
                Copyright = new CopyrightInfo("Michael Matus", 2015),
                AdditionalNewLineAfterOption = false,
                AddDashesToOption = true
            };
            string sPre = "Program to convert scanning files by SIOS NMM-1 to BCR or ISO 25178-71:2012 raster data format. " +
                "The quadruple of files (dat ind dsc pos) are analyzed to obtain the required parameters. " +
                "A rudimentary data processing is implemented via the -r option.";
            help.AddPreOptionsLine(sPre);
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Usage: " + AppName + " filename1 [filename2] [options]");
            help.AddPostOptionsLine("");
            help.AddPostOptionsLine("Supported values for --reference (-r):");
            help.AddPostOptionsLine("    0: nop");
            help.AddPostOptionsLine("    1: min");
            help.AddPostOptionsLine("    2: max");
            help.AddPostOptionsLine("    3: average");
            help.AddPostOptionsLine("    4: mid");
            help.AddPostOptionsLine("    5: bias");
            help.AddPostOptionsLine("    6: first");
            help.AddPostOptionsLine("    7: last");
            help.AddPostOptionsLine("    8: center");
            help.AddPostOptionsLine("    9: linear");
            help.AddPostOptionsLine("   10: LSQ");
            help.AddPostOptionsLine("   11: linear(positive)");
            help.AddPostOptionsLine("   12: LSQ(positive)");

            help.AddOptions(this);

            return help;
        }


    }
}
