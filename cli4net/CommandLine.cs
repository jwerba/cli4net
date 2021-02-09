﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace cli4net
{
    /**
       * Represents list of arguments parsed against a {@link Options} descriptor.
       * <p>
       * It allows querying of a bool {@link #hasOption(String opt)},
       * in addition to retrieving the {@link #getOptionValue(String opt)}
       * for options requiring arguments.
       * <p>
       * Additionally, any left-over or unrecognized arguments,
       * are available for further processing.
       */
    public class CommandLine : IEnumerable<Option>
    {
        /** the unrecognized options/arguments */
        private readonly List<string> args = new List<string>();

        /** the processed options */
        private readonly List<Option> options = new List<Option>();

        /**
         * Creates a command line.
         */
        internal CommandLine()
        {
            // nothing to do
        }

        /**
         * Query to see if an option has been set.
         *
         * @param opt the option to check.
         * @return true if set, false if not.
         * @since 1.5
         */
        public bool HasOption(Option opt)
        {
            return options.Contains(opt);
        }

        /**
         * Query to see if an option has been set.
         *
         * @param opt Short name of the option.
         * @return true if set, false if not.
         */
        public bool HasOption(string opt)
        {
            return HasOption(ResolveOption(opt));
        }

        /**
         * Query to see if an option has been set.
         *
         * @param opt character name of the option.
         * @return true if set, false if not.
         */
        public bool HasOption(char opt)
        {
            return HasOption(opt.ToString());
        }


        /**
         * Return a version of this <code>Option</code> converted to a particular type.
         *
         * @param option the name of the option.
         * @return the value parsed into a particular object.
         * @throws ParseException if there are problems turning the option value into the desired type
         * @see PatternOptionBuilder
         * @since 1.5
         */
        public object GetParsedOptionValue(Option option)
        {
            if (option == null)
            {
                return null;
            }
            string res = GetOptionValue(option);
            if (res == null)
            {
                return null;
            }
            return TypeHandler.createValue(res, option.GetOptionType());
        }

        /**
         * Return a version of this <code>Option</code> converted to a particular type.
         *
         * @param opt the name of the option.
         * @return the value parsed into a particular object.
         * @throws ParseException if there are problems turning the option value into the desired type
         * @see PatternOptionBuilder
         * @since 1.2
         */
        public object GetParsedOptionValue(string opt)
        {
            return GetParsedOptionValue(ResolveOption(opt));
        }

        /**
         * Return a version of this <code>Option</code> converted to a particular type.
         *
         * @param opt the name of the option.
         * @return the value parsed into a particular object.
         * @throws ParseException if there are problems turning the option value into the desired type
         * @see PatternOptionBuilder
         * @since 1.5
         */
        public object GetParsedOptionValue(char opt)
        {
            return GetParsedOptionValue(opt.ToString());
        }


        /**
         * Retrieve the first argument, if any, of this option.
         *
         * @param option the name of the option.
         * @return Value of the argument if option is set, and has an argument,
         * otherwise null.
         * @since 1.5
         */
        public string GetOptionValue(Option option)
        {
            if (option == null)
            {
                return null;
            }
            string[] values = GetOptionValues(option);
            return (values == null) ? null : values[0];
        }

        /**
         * Retrieve the first argument, if any, of this option.
         *
         * @param opt the name of the option.
         * @return Value of the argument if option is set, and has an argument,
         * otherwise null.
         */
        public string GetOptionValue(String opt)
        {
            return GetOptionValue(ResolveOption(opt));
        }

        /**
         * Retrieve the first argument, if any, of this option.
         *
         * @param opt the character name of the option.
         * @return Value of the argument if option is set, and has an argument,
         * otherwise null.
         */
        public string GetOptionValue(char opt)
        {
            return GetOptionValue(opt.ToString());
        }

        /**
         * Retrieves the array of values, if any, of an option.
         *
         * @param option string name of the option.
         * @return Values of the argument if option is set, and has an argument,
         * otherwise null.
         * @since 1.5
         */
        public string[] GetOptionValues(Option option)
        {
            List<string> values = new List<string>();

            foreach (Option processedOption in options)
            {
                if (processedOption.Equals(option))
                {
                    values.AddRange(processedOption.GetValuesList());
                }
            }
            return values.Count == 0 ? null : values.ToArray();
        }

        /**
         * Retrieves the array of values, if any, of an option.
         *
         * @param opt string name of the option.
         * @return Values of the argument if option is set, and has an argument,
         * otherwise null.
         */
        public String[] GetOptionValues(string opt)
        {
            return GetOptionValues(ResolveOption(opt));
        }

        /**
         * Retrieves the option object given the long or short option as a String
         *
         * @param opt short or long name of the option.
         * @return Canonicalized option.
         */
        private Option ResolveOption(String opt)
        {
            opt = Util.StripLeadingHyphens(opt);
            foreach (Option option in options)
            {
                if (opt.Equals(option.GetOpt()))
                {
                    return option;
                }
                if (opt.Equals(option.GetLongOpt()))
                {
                    return option;
                }
            }
            return null;
        }

        /**
         * Retrieves the array of values, if any, of an option.
         *
         * @param opt character name of the option.
         * @return Values of the argument if option is set, and has an argument,
         * otherwise null.
         */
        public string[] GetOptionValues(char opt)
        {
            return GetOptionValues(opt.ToString());
        }

        /**
         * Retrieve the first argument, if any, of an option.
         *
         * @param option name of the option.
         * @param defaultValue is the default value to be returned if the option
         * is not specified.
         * @return Value of the argument if option is set, and has an argument,
         * otherwise <code>defaultValue</code>.
         * @since 1.5
         */
        public string GetOptionValue(Option option, string defaultValue)
        {
            string answer = GetOptionValue(option);
            return (answer != null) ? answer : defaultValue;
        }

        /**
         * Retrieve the first argument, if any, of an option.
         *
         * @param opt name of the option.
         * @param defaultValue is the default value to be returned if the option
         * is not specified.
         * @return Value of the argument if option is set, and has an argument,
         * otherwise <code>defaultValue</code>.
         */
        public string GetOptionValue(String opt, string defaultValue)
        {
            return GetOptionValue(ResolveOption(opt), defaultValue);
        }

        /**
         * Retrieve the argument, if any, of an option.
         *
         * @param opt character name of the option
         * @param defaultValue is the default value to be returned if the option
         * is not specified.
         * @return Value of the argument if option is set, and has an argument,
         * otherwise <code>defaultValue</code>.
         */
        public string GetOptionValue(char opt, string defaultValue)
        {
            return GetOptionValue(opt.ToString(), defaultValue);
        }

        /**
         * Retrieve the map of values associated to the option. This is convenient
         * for options specifying Java properties like <code>-Dparam1=value1
         * -Dparam2=value2</code>. The first argument of the option is the key, and
         * the 2nd argument is the value. If the option has only one argument
         * (<code>-Dfoo</code>) it is considered as a bool flag and the value is
         * <code>"true"</code>.
         *
         * @param option name of the option.
         * @return The Properties mapped by the option, never <code>null</code>
         *         even if the option doesn't exists.
         * @since 1.5
         */
        public Dictionary<string, string> GetOptionProperties(Option option)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            foreach (Option processedOption in options)
            {
                if (processedOption.Equals(option))
                {
                    List<string> values = processedOption.GetValuesList();
                    if (values.Count >= 2)
                    {
                        // use the first 2 arguments as the key/value pair
                        props.Add(values[0], values[1]);
                    }
                    else if (values.Count == 1)
                    {
                        // no explicit value, handle it as a boolean
                        props.Add(values[0], "true");
                    }
                }
            }

            return props;
        }

        /**
         * Retrieve the map of values associated to the option. This is convenient
         * for options specifying Java properties like <code>-Dparam1=value1
         * -Dparam2=value2</code>. The first argument of the option is the key, and
         * the 2nd argument is the value. If the option has only one argument
         * (<code>-Dfoo</code>) it is considered as a bool flag and the value is
         * <code>"true"</code>.
         *
         * @param opt name of the option.
         * @return The Properties mapped by the option, never <code>null</code>
         *         even if the option doesn't exists.
         * @since 1.2
         */
        public Dictionary<string, string> GetOptionProperties(string opt)
        {

            Dictionary<string, string> props = new Dictionary<string, string>();

            foreach (Option option in options)
            {
                if (opt.Equals(option.GetOpt()) || opt.Equals(option.GetLongOpt()))
                {
                    List<string> values = option.GetValuesList();
                    if (values.Count >= 2)
                    {
                        // use the first 2 arguments as the key/value pair
                        props.Add(values[0], values[1]);
                    }
                    else if (values.Count == 1)
                    {
                        // no explicit value, handle it as a boolean
                        props.Add(values[0], "true");
                    }
                }
            }
            return props;
        }

        /**
         * Retrieve any left-over non-recognized options and arguments
         *
         * @return remaining items passed in but not parsed as an array.
         */
        public string[] GetArgs()
        {
            return args.ToArray();
        }

        /**
         * Retrieve any left-over non-recognized options and arguments
         *
         * @return remaining items passed in but not parsed as a <code>List</code>.
         */
        public List<string> GetArgList()
        {
            return args;
        }

        /**
         * jkeyes
         * - commented out until it is implemented properly
         * <p>Dump state, suitable for debugging.</p>
         *
         * @return Stringified form of this object.
         */

        /*
        public String toString() {
            StringBuilder buf = new StringBuilder();

            buf.append("[ CommandLine: [ options: ");
            buf.append(options.toString());
            buf.append(" ] [ args: ");
            buf.append(args.toString());
            buf.append(" ] ]");

            return buf.toString();
        }
        */

        /**
         * Add left-over unrecognized option/argument.
         *
         * @param arg the unrecognized option/argument.
         */
        internal void AddArg(string arg)
        {
            args.Add(arg);
        }

        /**
         * Add an option to the command line.  The values of the option are stored.
         *
         * @param opt the processed option.
         */
        internal void AddOption(Option opt)
        {
            options.Add(opt);
        }


        /**
         * Returns an array of the processed {@link Option}s.
         *
         * @return an array of the processed {@link Option}s.
         */
        public Option[] GetOptions()
        {
            // reinitialise array
            Option[] optionsArray = options.ToArray();
            // return the array
            return optionsArray;
        }

        /**
     * Returns an iterator over the Option members of CommandLine.
     *
     * @return an <code>Iterator</code> over the processed {@link Option}
     * members of this {@link CommandLine}.
     */
        public IEnumerator<Option> GetEnumerator()
        {
            return options.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return options.GetEnumerator();
        }

        /**
         * A nested builder class to create <code>CommandLine</code> instance
         * using descriptive methods.
         *
         * @since 1.4
         */
        internal class Builder
        {
            /**
             * CommandLine that is being build by this Builder.
             */
            private static readonly CommandLine commandLine = new CommandLine();

            /**
             * Add an option to the command line. The values of the option are stored.
             *
             * @param opt the processed option.
             *
             * @return this Builder instance for method chaining.
             */
            public Builder AddOption(Option opt)
            {
                commandLine.AddOption(opt);
                return this;
            }

            /**
             * Add left-over unrecognized option/argument.
             *
             * @param arg the unrecognized option/argument.
             *
             * @return this Builder instance for method chaining.
             */
            public Builder AddArg(String arg)
            {
                commandLine.AddArg(arg);
                return this;
            }

            public static CommandLine Build()
            {
                return commandLine;
            }
        }
    }

}
