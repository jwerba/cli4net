namespace cli4net
{
   
    /**
     * Exception thrown during parsing signalling an unrecognized
     * option was seen.
     */
    public class UnrecognizedOptionException : ParseException
    {
        /** The  unrecognized option */
        private string option;

        /**
         * Construct a new <code>UnrecognizedArgumentException</code>
         * with the specified detail message.
         *
         * @param message the detail message
         */
        public UnrecognizedOptionException(string message):base(message){ }

        /**
         * Construct a new <code>UnrecognizedArgumentException</code>
         * with the specified option and detail message.
         *
         * @param message the detail message
         * @param option  the unrecognized option
         * @since 1.2
         */
        public UnrecognizedOptionException(string message, string option): this(message)
        {
            this.option = option;
        }

        /**
         * Returns the unrecognized option.
         *
         * @return the related option
         * @since 1.2
         */
        public string getOption()
        {
            return option;
        }
    }

}