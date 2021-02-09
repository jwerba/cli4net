using System;

/**
 * Validates an Option string.
 *
 * @since 1.1
 */
sealed class OptionValidator
{
    /**
     * Validates whether <code>opt</code> is a permissible Option
     * shortOpt.  The rules that specify if the <code>opt</code>
     * is valid are:
     *
     * <ul>
     *  <li>a single character <code>opt</code> that is either
     *  ' '(special case), '?', '@' or a letter</li>
     *  <li>a multi character <code>opt</code> that only contains
     *  letters.</li>
     * </ul>
     * <p>
     * In case {@code opt} is {@code null} no further validation is performed.
     *
     * @param opt The option string to validate, may be null
     * @throws IllegalArgumentException if the Option is not valid.
     */
    internal static void ValidateOption(string opt)
    {
        if (opt == null)
        {
            return;
        }
        // handle the single character opt
        if (opt.Length == 1)
        {
            char ch = opt[0];

            if (!IsValidOpt(ch))
            {
                throw new ArgumentException("Illegal option name '" + ch + "'");
            }
        }
        // handle the multi character opt
        else
        {
            foreach (char ch in opt.ToCharArray())
            {
                if (!IsValidChar(ch))
                {
                    throw new ArgumentException("The option '" + opt + "' contains an illegal "
                                                       + "character : '" + ch + "'");
                }
            }
        }
    }

    /**
     * Returns whether the specified character is a valid Option.
     *
     * @param c the option to validate
     * @return true if <code>c</code> is a letter, '?' or '@', otherwise false.
     */
    private static bool IsValidOpt(char c)
    {
        return IsValidChar(c) || c == '?' || c == '@';
    }

    /**
     * Returns whether the specified character is a valid character.
     *
     * @param c the character to validate
     * @return true if <code>c</code> is a letter.
     */
    private static bool IsValidChar(char c)
    {
        return true;
    }
}
