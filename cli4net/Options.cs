using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.Serialization;
using System.Text;

namespace cli4net
{
    /**
     * Main entry-point into the library.
     * <p>
     * Options represents a collection of {@link Option} objects, which
     * describe the possible options for a command-line.
     * <p>
     * It may flexibly parse long and short options, with or without
     * values.  Additionally, it may parse only a portion of a commandline,
     * allowing for flexible multi-stage parsing.
     *
     * @see org.apache.commons.cli.CommandLine
     */
    public class Options
    {
        /** a map of the options with the character key */
        private readonly Dictionary<string, Option> shortOpts = new Dictionary<string, Option>();

        /** a map of the options with the long key */
        private readonly Dictionary<string, Option> longOpts = new Dictionary<string, Option>();

        /** a map of the required options */
        // N.B. This can contain either a String (addOption) or an OptionGroup (addOptionGroup)
        // TODO this seems wrong
        private readonly List<object> requiredOpts = new List<object>();

        /** a map of the option groups */
        private readonly Dictionary<string, OptionGroup> optionGroups = new Dictionary<string, OptionGroup>();

        /**
         * Add the specified option group.
         *
         * @param group the OptionGroup that is to be added
         * @return the resulting Options instance
         */
        public Options AddOptionGroup(OptionGroup group)
        {
            if (group.IsRequired())
            {
                requiredOpts.Add(group);
            }
            foreach (Option option in group.GetOptions())
            {
                // an Option cannot be required if it is in an
                // OptionGroup, either the group is required or
                // nothing is required
                option.SetRequired(false);
                AddOption(option);
                optionGroups.Add(option.GetKey(), group);
            }
            return this;

        }

        /**
         * Lists the OptionGroups that are members of this Options instance.
         *
         * @return a Collection of OptionGroup instances.
         */
        public ICollection<OptionGroup> GetOptionGroups()
        {
            return new List<OptionGroup>(optionGroups.Values);
        }

        /**
         * Add an option that only contains a short name.
         *
         * <p>
         * The option does not take an argument.
         * </p>
         *
         * @param opt Short single-character name of the option.
         * @param description Self-documenting description
         * @return the resulting Options instance
         * @since 1.3
         */
        public Options AddOption(string opt, string description)
        {
            AddOption(opt, null, false, description);
            return this;
        }

        /**
         * Add an option that only contains a short-name.
         *
         * <p>
         * It may be specified as requiring an argument.
         * </p>
         *
         * @param opt Short single-character name of the option.
         * @param hasArg flag signalling if an argument is required after this option
         * @param description Self-documenting description
         * @return the resulting Options instance
         */
        public Options AddOption(string opt, bool hasArg, string description)
        {
            AddOption(opt, null, hasArg, description);
            return this;
        }

        /**
         * Add an option that contains a short-name and a long-name.
         *
         * <p>
         * It may be specified as requiring an argument.
         * </p>
         *
         * @param opt Short single-character name of the option.
         * @param longOpt Long multi-character name of the option.
         * @param hasArg flag signalling if an argument is required after this option
         * @param description Self-documenting description
         * @return the resulting Options instance
         */
        public Options AddOption(string opt, string longOpt, bool hasArg, string description)
        {
            AddOption(new Option(opt, longOpt, hasArg, description));
            return this;
        }

        /**
         * Add an option that contains a short-name and a long-name.
         *
         * <p>
         * The added option is set as required. It may be specified as requiring an argument. This method is a shortcut for:
         * </p>
         *
         * <pre>
         * <code>
         * Options option = new Option(opt, longOpt, hasArg, description);
         * option.setRequired(true);
         * options.add(option);
         * </code>
         * </pre>
         *
         * @param opt Short single-character name of the option.
         * @param longOpt Long multi-character name of the option.
         * @param hasArg flag signalling if an argument is required after this option
         * @param description Self-documenting description
         * @return the resulting Options instance
         * @since 1.4
         */
        public Options AddRequiredOption(string opt, string longOpt, bool hasArg, string description)
        {
            Option option = new Option(opt, longOpt, hasArg, description);
            option.SetRequired(true);
            AddOption(option);
            return this;
        }

        /**
         * Adds an option instance
         *
         * @param opt the option that is to be added
         * @return the resulting Options instance
         */
        public Options AddOption(Option opt)
        {
            string key = opt.GetKey();

            // add it to the long option list
            if (opt.HasLongOpt())
            {
                longOpts[opt.GetLongOpt()] = opt;
            }

            // if the option is required add it to the required list
            if (opt.IsRequired())
            {
                if (requiredOpts.Contains(key))
                {
                    requiredOpts.Remove(key);
                }
                requiredOpts.Add(key);
            }

            shortOpts[key] = opt;

            return this;
        }

        /**
         * Retrieve a read-only list of options in this set
         *
         * @return read-only Collection of {@link Option} objects in this descriptor
         */
        public IImmutableList<Option> GetOptions()
        {
            return ImmutableList.CreateRange<Option>(HelpOptions());
        }

        /**
         * Returns the Options for use by the HelpFormatter.
         *
         * @return the List of Options
         */
        public List<Option> HelpOptions()
        {
            return new List<Option>(shortOpts.Values);
        }

        /**
         * Returns the required options.
         *
         * @return read-only List of required options
         */
        public IImmutableList<object> GetRequiredOptions()
        {
            return ImmutableList.CreateRange<object>(requiredOpts);
        }

        /**
         * Retrieve the {@link Option} matching the long or short name specified.
         *
         * <p>
         * The leading hyphens in the name are ignored (up to 2).
         * </p>
         *
         * @param opt short or long name of the {@link Option}
         * @return the option represented by opt
         */
        public Option GetOption(String opt)
        {
            opt = Util.StripLeadingHyphens(opt);

            if (shortOpts.ContainsKey(opt))
            {
                return shortOpts[opt];
            }

            return longOpts[opt];
        }

        /**
         * Returns the options with a long name starting with the name specified.
         *
         * @param opt the partial name of the option
         * @return the options matching the partial name specified, or an empty list if none matches
         * @since 1.3
         */
        public IImmutableList<string> GetMatchingOptions(String opt)
        {
            opt = Util.StripLeadingHyphens(opt);

            List<String> matchingOpts = new List<string>();

            // for a perfect match return the single option only
            if (longOpts.ContainsKey(opt))
            {
                return ImmutableList.Create<string>(opt);
            }
            foreach (var longOpt in longOpts)
            {
                if (longOpt.Key.StartsWith(opt))
                {
                    matchingOpts.Add(longOpt.Key);
                }
            }
            return ImmutableList.CreateRange<string>(matchingOpts);
        }

        /**
         * Returns whether the named {@link Option} is a member of this {@link Options}.
         *
         * @param opt short or long name of the {@link Option}
         * @return true if the named {@link Option} is a member of this {@link Options}
         */
        public bool HasOption(String opt)
        {
            opt = Util.StripLeadingHyphens(opt);

            return shortOpts.ContainsKey(opt) || longOpts.ContainsKey(opt);
        }

        /**
         * Returns whether the named {@link Option} is a member of this {@link Options}.
         *
         * @param opt long name of the {@link Option}
         * @return true if the named {@link Option} is a member of this {@link Options}
         * @since 1.3
         */
        public bool HasLongOption(String opt)
        {
            opt = Util.StripLeadingHyphens(opt);

            return longOpts.ContainsKey(opt);
        }

        /**
         * Returns whether the named {@link Option} is a member of this {@link Options}.
         *
         * @param opt short name of the {@link Option}
         * @return true if the named {@link Option} is a member of this {@link Options}
         * @since 1.3
         */
        public bool HasShortOption(String opt)
        {
            opt = Util.StripLeadingHyphens(opt);

            return shortOpts.ContainsKey(opt);
        }

        /**
         * Returns the OptionGroup the <code>opt</code> belongs to.
         *
         * @param opt the option whose OptionGroup is being queried.
         * @return the OptionGroup if <code>opt</code> is part of an OptionGroup, otherwise return null
         */
        public OptionGroup GetOptionGroup(Option opt)
        {
            if (!optionGroups.ContainsKey(opt.GetKey()))
                return null;
            return optionGroups[opt.GetKey()];
        }

        /**
         * Dump state, suitable for debugging.
         *
         * @return Stringified form of this object
         */


        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();

            buf.Append("[ Options: [ short ");
            buf.Append(shortOpts.ToString());
            buf.Append(" ] [ long ");
            buf.Append(longOpts);
            buf.Append(" ]");
            return buf.ToString();
        }

    }

}
