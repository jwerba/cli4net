using System;
namespace cli4net
{
    /**
      * A class that implements the <code>CommandLineParser</code> interface
      * can parse a String array according to the {@link Options} specified
      * and return a {@link CommandLine}.
      */
    public interface CommandLineParser
    {
        /**
         * Parse the arguments according to the specified options.
         *
         * @param options the specified Options
         * @param arguments the command line arguments
         * @return the list of atomic option and value tokens
         *
         * @throws ParseException if there are any problems encountered
         * while parsing the command line tokens.
         */
        CommandLine Parse(Options options, string[] arguments);


        /**
         * Parse the arguments according to the specified options.
         *
         * @param options the specified Options
         * @param arguments the command line arguments
         * @param stopAtNonOption if <code>true</code> an unrecognized argument stops
         *     the parsing and the remaining arguments are added to the
         *     {@link CommandLine}s args list. If <code>false</code> an unrecognized
         *     argument triggers a ParseException.
         *
         * @return the list of atomic option and value tokens
         * @throws ParseException if there are any problems encountered
         * while parsing the command line tokens.
         */
        CommandLine Parse(Options options, string[] arguments, bool stopAtNonOption);


    }

}
