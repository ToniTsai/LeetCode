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

            int indexS = s.Length;
            int indexP = p.Length;


            char currentS = s[--indexS];
            char currentP = p[--indexP];

            while (indexS >= 0 && indexP >= 0)
            {
                int repeatP = 0;

                if (currentP == '*')
                {
                    repeatP = Int32.MaxValue;
                    while (indexP > 0 && p[indexP - 1] == '*')
                        --indexP;
                }
                else
                {
                    if (currentP == '.')
                    {
                        repeatP = 1;
                    }
                }

                if(currentP != currentS)
                {
                    if(repeatP == Int32.MaxValue)
                    {
                        if (indexP > 0)
                            currentP = s[--indexP];
                        continue;
                    }
                    else if(repeatP == 1)
                    {
                        if (indexP >= 0)
                            currentP = s[indexP--];
                        if (indexS >= 0)
                            currentS = s[indexS--];
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (repeatP == Int32.MaxValue)
                    {
                        while (indexP >= 0 && currentP == p[indexP])
                        {
                            indexP--;
                        }

                        while (indexS >= 0 && currentS == s[indexS])
                        {
                            indexS--;
                        }

                        if (indexP >= 0)
                            currentP = p[indexP--];
                        if (indexS >= 0)
                            currentS = s[indexS--];
                    }
                    else
                    {
                        if (indexP >= 0)
                            currentP = p[indexP--];
                        if (indexS >= 0)
                            currentS = s[indexS--];
                    }
                }
            }

            if(indexP == 0)
            {
                if (currentP == '*' || currentP == '.')
                    return true;
                else
                    return false;
            } else if (indexP > 0)
            {
                return false;
            }

            if (indexS >= 0)
                return false;
            return true;
        }
    }
}
