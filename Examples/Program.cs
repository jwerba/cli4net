using System;
using cli4net;
using static cli4net.Option;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            // create Options object
            Options options = new Options();
            // add t option
            options.AddOption("t", false, "display current time");
            options.AddOption("h", false, "Display help options");
            CommandLineParser parser = new DefaultParser();
            CommandLine cmd = parser.Parse(options, args);

            options.AddOption("h", "help", false, "Print this usage information");
            options.AddOption("v", "verbose", false, "Print out VERBOSE information");

            OptionGroup optionGroup = new OptionGroup();
            optionGroup.AddOption(new OptionBuilder("f").HasArg(true).ArgName("filename").Build());
            optionGroup.AddOption(new OptionBuilder("m").HasArg(true).ArgName("email").Build());
            options.AddOptionGroup(optionGroup);

            if (cmd.HasOption("h"))
            {
                HelpFormatter formatter = new HelpFormatter();
                formatter.printHelp("x", options, true);
                return;
            }
            if (cmd.HasOption("t"))
            {
                Console.WriteLine(System.DateTime.Now);
            }
        }
    }
}
