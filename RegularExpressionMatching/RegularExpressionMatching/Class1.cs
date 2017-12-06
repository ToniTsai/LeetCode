using System;

namespace LeetCode
{
    public class Solution
    {
        public Solution()
        {
            ;
        }

        public bool IsMatch(string s, string p)
        {
            if (s == null || p == null)
                return false;

            int indexS = s.Length - 1;
            int indexP = p.Length - 1;


            char currentS = s[indexS--];
            char currentP = p[indexP--];

            while (indexS > 0 && indexP > 0)
            {
                int repeatS = 0;
                int repeatP = 0;

                if(currentS == '*')
                {
                    repeatS = Int32.MaxValue;
                    currentS = s[indexS--];
                    while (currentS == '*')
                        currentS = s[indexS--];
                }
                else
                {
                    if (currentS == '.')
                    {
                        repeatS = 1;
                        currentS = s[indexS--];
                        while (currentS == '.')
                            currentS = s[indexS--];
                    }
                }

                if (currentP == '*')
                {
                    repeatP = Int32.MaxValue;
                    currentP = s[indexP--];
                    while (currentP == '*')
                        currentP = s[indexP--];
                }
                else
                {
                    if (currentP == '.')
                    {
                        repeatP = 1;
                        currentP = s[indexP--];
                        while (currentP == '.')
                            currentP = s[indexP--];
                    }
                }

                if(currentP != currentS)
                {
                    if(repeatP == Int32.MaxValue)
                    {
                        currentP = s[indexP--];
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }

            return true;
        }
    }
}
