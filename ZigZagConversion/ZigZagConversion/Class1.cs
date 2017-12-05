using System;

namespace LeetCode
{
    public class Solution
    {
        public Solution()
        {
            ;
        }
        public string Convert(string s, int numRows)
        {
            char[] zigzagChar;

            if (numRows < 2)
                return s;

            zigzagChar = new char[s.Length];

            int mainCount = ( 2 * numRows - 2);
            int secondCount = 0;

            int charIndex = 0;
            for (int i = 0; i < numRows; i++)
            {
                if (i < s.Length)
                    zigzagChar[charIndex++] = s[i];

                int index = i;

                while (index < s.Length)
                {
                    if(mainCount != 0)
                    {
                        index += mainCount;
                        if (index < s.Length)
                            zigzagChar[charIndex++] = s[index];
                        else
                            break;
                    }

                    if (secondCount != 0)
                    {
                        index += secondCount;
                        if (index < s.Length)
                            zigzagChar[charIndex++] = s[index];
                        else
                            break;
                    }
                }

                mainCount -= 2;
                secondCount += 2;
            }

            return new string(zigzagChar);
        }
    }
}
