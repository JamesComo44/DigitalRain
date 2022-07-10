using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalRain.Raindrop
{
    static class SymbolPools
    {
        public static char[] EnglishAlphanumeric()
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();
        }

        public static char[] EnglishAlphanumericUppercase()
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        }

        public static char[] EnglishAlphanumericUpperSymbols()
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=+/{}[]?><".ToCharArray();
        }
    }
}
