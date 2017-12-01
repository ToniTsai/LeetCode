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
            int[] CharHashIndex = new int[65536];


            CharHashIndex[Convert.ToInt32(((char)s[0]))] = -1;

            int startIndex = 0;
            int endIndex = 1;

            int maxLength = endIndex - startIndex;
            int startIndexMax = startIndex;
            int endindexMax = endIndex;

            for (int i = 1; i < s.Length; i++)
            {
                int charValue = Convert.ToInt32(((char)s[i]));
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
                        CharHashIndex[Convert.ToInt32(((char)s[j]))] = 0;
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

