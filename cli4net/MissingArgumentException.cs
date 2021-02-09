using System;
using System.Runtime.Serialization;

namespace cli4net
{
    [Serializable]
    internal class MissingArgumentException : ParseException
    {
        private Option option;



        public MissingArgumentException(Option option) : base("Missing argument for option: " + option.GetKey())
        {
            this.option = option;
        }

        public MissingArgumentException(string message) : base(message)
        {
        }

        /**
         * * Return the option requiring an argument that wasn't provided
     * on the command line.
     *
     * @return the related option
     * @since 1.2
     */
        public Option getOption()
        {
            return option;
        }
    }
}