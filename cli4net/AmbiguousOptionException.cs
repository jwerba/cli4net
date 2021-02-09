using System;
using System.Collections.Generic;
using System.Text;

namespace cli4net
{
    /**
     * Exception thrown when an option can't be identified from a partial name.
     *
     * @since 1.3
     */
    public class AmbiguousOptionException : UnrecognizedOptionException
    {

        /** The list of options matching the partial name specified */
        private readonly ICollection<String> matchingOptions;

        /**
         * Constructs a new AmbiguousOptionException.
         *
         * @param option          the partial option name
         * @param matchingOptions the options matching the name
         */
        public AmbiguousOptionException(string option, IList<string> matchingOptions) : base(CreateMessage(option, matchingOptions), option)
        {
            this.matchingOptions = matchingOptions;
        }

        /**
         * Returns the options matching the partial name.
         * @return a collection of options matching the name
         */
        public ICollection<string> getMatchingOptions()
        {
            return matchingOptions;
        }

        /**
         * Build the exception message from the specified list of options.
         *
         * @param option
         * @param matchingOptions
         * @return
         */
        private static String CreateMessage(string option, IList<string> matchingOptions)
        {
            StringBuilder buf = new StringBuilder("Ambiguous option: '");
            buf.Append(option);
            buf.Append("'  (could be: ");
            foreach (var it in matchingOptions)
            {
                buf.Append("'");
                buf.Append(it);
                buf.Append("'");
                if (matchingOptions.IndexOf(it) < matchingOptions.Count -1)
                {
                    buf.Append(", ");
                }
            }
            buf.Append(")");
            return buf.ToString();
        }

    }
}
