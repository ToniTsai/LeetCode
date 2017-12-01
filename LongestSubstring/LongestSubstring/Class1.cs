using System;

namespace LeetCode
{
    public class Solution
    {
        public Solution()
        {
            ;
        }
        public int LengthOfLongestSubstring(string s)
        {

            if (s == "")
                return 0;

            int[] CharHashIndex = new int[512];

            int[] charInt = new int[s.Length];

            for (int i = 0; i < charInt.Length; i++)
                charInt[i] = Convert.ToInt32(((char)s[i]));

            CharHashIndex[charInt[0]] = -1;

            int startIndex = 0;
            int endIndex = 1;

            int maxLength = endIndex - startIndex;
            int startIndexMax = startIndex;
            int endindexMax = endIndex;

            for (int i = 1; i < s.Length; i++)
            {
                int charValue = charInt[i];
                if (CharHashIndex[charValue] == 0)
                {
                    CharHashIndex[charValue] = i;
                    endIndex = i + 1;
                }
                else
                {
                    if ((endIndex - startIndex) > maxLength)
                    {
                        maxLength = endIndex - startIndex;
                        startIndexMax = startIndex;
                        endindexMax = endIndex;
                    }

                    int index;

                    if (CharHashIndex[charValue] > 0)
                        index = CharHashIndex[charValue] + 1;
                    else
                        index = 1;

                    for (int j = startIndex; j < index; j++)
                    {
                        CharHashIndex[charInt[j]] = 0;
                    }

                    startIndex = index;

                    CharHashIndex[charValue] = i;
                }
            }

            if ((endIndex - startIndex) > maxLength)
            {
                maxLength = endIndex - startIndex;
                startIndexMax = startIndex;
                endindexMax = endIndex;
            }

            return maxLength;

        }
    }
}

