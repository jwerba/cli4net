using System;
using System.Collections.Generic;
using System.Text;

namespace cli4net
{

    /**
     * A group of mutually exclusive options.
     */
    public class OptionGroup
    {

        /** hold the options */
        private readonly Dictionary<String, Option> optionMap = new Dictionary<string, Option>();

        /** the name of the selected option */
        private String selected;

        /** specified whether this group is required */
        private bool required;

        /**
         * Add the specified <code>Option</code> to this group.
         *
         * @param option the option to add to this group
         * @return this option group with the option added
         */
        public OptionGroup AddOption(Option option)
        {
            // key   - option name
            // value - the option
            optionMap.Add(option.GetKey(), option);
            return this;
        }

        /**
         * @return the names of the options in this group as a
         * <code>Collection</code>
         */
        public IList<String> GetNames()
        {
            // the key set is the collection of names
            string[] keys = new string[optionMap.Count];
            optionMap.Keys.CopyTo(keys, 0);
            return new List<String>(keys);
        }

        /**
         * @return the options in this group as a <code>Collection</code>
         */
        public IList<Option> GetOptions()
        {
            // the values are the collection of options
            Option[] items = new Option[optionMap.Count];
            optionMap.Values.CopyTo(items, 0);
            return new List<Option>(items);
        }

        /**
         * Set the selected option of this group to <code>name</code>.
         *
         * @param option the option that is selected
         * @throws AlreadySelectedException if an option from this group has
         * already been selected.
         */
        public void SetSelected(Option option)
        {
            if (option == null)
            {
                // reset the option previously selected
                selected = null;
                return;
            }

            // if no option has already been selected or the
            // same option is being reselected then set the
            // selected member variable
            if (selected == null || selected.Equals(option.GetKey()))
            {
                selected = option.GetKey();
            }
            else
            {
                throw new AlreadySelectedException(this, option);
            }
        }

        /**
         * @return the selected option name
         */
        public String GetSelected()
        {
            return selected;
        }

        /**
         * @param required specifies if this group is required
         */
        public void SetRequired(bool required)
        {
            this.required = required;
        }

        /**
         * Returns whether this option group is required.
         *
         * @return whether this option group is required
         */
        public bool IsRequired()
        {
            return required;
        }

        /**
         * Returns the stringified version of this OptionGroup.
         *
         * @return the stringified representation of this group
         */

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            var options = GetOptions();
            buff.Append("[");
            foreach (Option option in options)
            {

                if (option.GetOpt() != null)
                {
                    buff.Append("-");
                    buff.Append(option.GetOpt());
                }
                else
                {
                    buff.Append("--");
                    buff.Append(option.GetLongOpt());
                }

                if (option.GetDescription() != null)
                {
                    buff.Append(" ");
                    buff.Append(option.GetDescription());
                }

                if (options.IndexOf(option) < options.Count - 1)
                {
                    buff.Append(", ");
                }
            }
            buff.Append("]");
            return buff.ToString();
        }
    }

}