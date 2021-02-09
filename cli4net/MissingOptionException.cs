using System;
using System.Collections.Generic;
using System.Text;

namespace cli4net
{
    [Serializable]
    internal class MissingOptionException : ParseException
    {
        private List<object> missingOptions;



        public MissingOptionException(List<object> missingOptions) : base(CreateMessage(missingOptions))
        {
            this.missingOptions = missingOptions;
        }

        public MissingOptionException(string message) : base(message)
        {
        }

        /**
     * Returns the list of options or option groups missing in the command line parsed.
     *
     * @return the missing options, consisting of String instances for simple
     *         options, and OptionGroup instances for required option groups.
     * @since 1.2
     */
        public List<object> getMissingOptions()
        {
            return missingOptions;
        }


        /**
     * Build the exception message from the specified list of options.
     *
     * @param missingOptions the list of missing options and groups
     * @since 1.2
     */
        private static string CreateMessage(List<object> missingOptions)
        {
            StringBuilder buf = new StringBuilder("Missing required option");
            buf.Append(missingOptions.Count == 1 ? "" : "s");
            buf.Append(": ");
            foreach(object obj in missingOptions)
            {
                buf.Append(obj.ToString());
                if (missingOptions.IndexOf(obj) < missingOptions.Count - 1)
                {
                    buf.Append(", ");
                }
            }
            return buf.ToString();
        }
    }
}