using System;
namespace cli4net
{
    /**
     * Thrown when more than one option in an option group
     * has been provided.
     */
    public class AlreadySelectedException : ParseException
    {
        /** The option group selected. */
        private OptionGroup group;

        /** The option that triggered the exception. */
        private Option option;

        /**
         * Construct a new <code>AlreadySelectedException</code>
         * with the specified detail message.
         *
         * @param message the detail message
         */
        public AlreadySelectedException(String message) : base(message)
        {

        }

        /**
         * Construct a new <code>AlreadySelectedException</code>
         * for the specified option group.
         *
         * @param group  the option group already selected
         * @param option the option that triggered the exception
         * @since 1.2
         */
        public AlreadySelectedException(OptionGroup group, Option option) : base("The option '" + option.GetKey() + "' was specified but an option from this group "
                    + "has already been selected: '" + group.GetSelected() + "'")
        {
            this.group = group;
            this.option = option;
        }

        /**
         * Returns the option group where another option has been selected.
         *
         * @return the related option group
         * @since 1.2
         */
        public OptionGroup GetOptionGroup()
        {
            return group;
        }

        /**
         * Returns the option that was added to the group and triggered the exception.
         *
         * @return the related option
         * @since 1.2
         */
        public Option GetOption()
        {
            return option;
        }
    }

}


