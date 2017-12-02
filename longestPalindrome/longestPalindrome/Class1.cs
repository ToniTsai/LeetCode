using System;

namespace LeetCode
{
    public class Solution
    {
        public Solution()
        {
            ;
        }

        public string LongestPalindrome(string s)
        {
            int maxLength = 0;
            int lbound = -1;
            int rbound = -1;

            if (s == null || s.Length <= 1)
                return s;

            for (int i = s.Length / 2; i >= 0; i--)
            {
                if (((i + 1) * 2 + 1) > maxLength)
                {
                    int l, k, len;
                    l = 0;
                    k = 0;
                    len = 0;
                    if ((i + 1) < s.Length && s[i] == s[i + 1])
                    {
                        len = 2;
                        k = i - 1;
                        l = i + 2;
                        while (k >= 0 && l < s.Length && s[k] == s[l])
                        {
                            len += 2;
                            k--;
                            l++;
                        }
                    }
                    if (len > maxLength)
                    {
                        maxLength = len;
                        lbound = k + 1;
                        rbound = l - 1;
                    }
                    len = 0;
                    if ((i + 2) < s.Length && s[i] == s[i + 2])
                    {
                        len = 3;
                        k = i - 1;
                        l = i + 3;
                        while (k >= 0 && l < s.Length && s[k] == s[l])
                        {
                            len += 2;
                            k--;
                            l++;
                        }
                    }
                    if (len > maxLength)
                    {
                        maxLength = len;
                        lbound = k + 1;
                        rbound = l - 1;
                    }

                }
                else
                {
                    break;
                }
            }

            for (int i = s.Length / 2 + 1; i < (s.Length - 1); i++)
            {
                if (((s.Length - i + 1) * 2 + 1) > maxLength)
                {
                    int l, k, len;
                    l = 0;
                    k = 0;
                    len = 0;
                    if ((i + 1) < s.Length && s[i] == s[i + 1])
                    {
                        len = 2;
                        k = i - 1;
                        l = i + 2;
                        while (k >= 0 && l < s.Length && s[k] == s[l])
                        {
                            len += 2;
                            k--;
                            l++;
                        }
                    }
                    if (len > maxLength)
                    {
                        maxLength = len;
                        lbound = k + 1;
                        rbound = l - 1;
                    }
                    len = 0;
                    if ((i + 2) < s.Length && s[i] == s[i + 2])
                    {
                        len = 3;
                        k = i - 1;
                        l = i + 3;
                        while (k >= 0 && l < s.Length && s[k] == s[l])
                        {
                            len += 2;
                            k--;
                            l++;
                        }
                    }
                    if (len > maxLength)
                    {
                        maxLength = len;
                        lbound = k + 1;
                        rbound = l - 1;
                    }

                }
                else
                {
                    break;
                }
            }

            if (maxLength > 0)
                return s.Substring(lbound, maxLength);
            else
                return s.Substring(0, 1);
        }
    }
}
