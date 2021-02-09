using System;

namespace cli4net
{

    /**
     * Base for Exceptions thrown during parsing of a command-line.
     */
    public class ParseException : Exception
    {
        /**
         * Construct a new <code>ParseException</code>
         * with the specified detail message.
         *
         * @param message the detail message
         */
        public ParseException(string message) : base(message)
        {

        }
    }
}





