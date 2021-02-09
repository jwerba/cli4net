using System;

namespace cli4net
{
    /**
 * Contains useful helper methods for classes within this package.
 */
    internal class Util
    {
        /**
     * Remove the hyphens from the beginning of<code> str</code> and
     * return the new String.
     *
     * @param str The string from which the hyphens should be removed.
     *
     * @return the new String.
     */
        internal static string StripLeadingHyphens(string str)
        {
            if (str == null)
            {
                return null;
            }
            if (str.StartsWith("--"))
            {
                return str.Substring(2, str.Length);
            }
            else if (str.StartsWith("-"))
            {
                return str.Substring(1);
            }
            return str;
        }

        /**
     * Remove the leading and trailing quotes from <code>str</code>.
     * E.g. if str is '"one two"', then 'one two' is returned.
     *
     * @param str The string from which the leading and trailing quotes
     * should be removed.
     *
     * @return The string without the leading and trailing quotes.
     */
        internal static string StripLeadingAndTrailingQuotes(string str)
        {
            int length = str.Length;
            if (length > 1 && str.StartsWith("\"") && str.EndsWith("\"") && str.Substring(1, length - 1).IndexOf('"') == -1)
            {
                str = str.Substring(1, length - 1);
            }
            return str;
        }
    }
}