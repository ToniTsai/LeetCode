﻿using System;
using System.Text.RegularExpressions;

namespace LeetCode
{
    public class Solution
    {
        public int MyAtoi(string str)
        {
            bool negative = false;
            int startIndex = 0;
            int factor = 10;
            int total = 0;

            if (str == null)
                return 0;

            str.Trim();

            if (str == "")
                return 0;

            while (str[startIndex] == ' ')
            {
                startIndex++;
                if (startIndex >= str.Length)
                    return 0;
            }

            if (startIndex < str.Length && str[startIndex] == '-')
            {
                negative = true;
                startIndex++;
            }
            else
            {

                if (startIndex < str.Length && str[startIndex] == '+')
                {
                    startIndex++;
                }
            }

            int maxIndex = 0;
            int maxDigit = startIndex + 10;

            for (int i = startIndex; i < str.Length; i++)
            {

                int charVal = str[i] - '0';

                if (charVal < 0 || charVal > 9)
                {
                    if (negative)
                        return -total;
                    else
                        return total;
                }

                total = total * factor + charVal;

                if (total >= Int32.MaxValue / 10)
                {
                    maxIndex = i + 1;
                    break;
                }

            }

            if (negative)
                total = -total;

            if (maxIndex != 0)
            {
                //try
                //{
                    for (int i = maxIndex; i < str.Length; i++)
                    {
                        int charVal = str[i] - '0';

                        if (negative)
                        {
                            if (charVal < 0 || charVal > 9)
                                return total;

                            if (i >= maxDigit)
                            {
                                return Int32.MinValue;
                            }

                            if (total < Int32.MinValue / 10)
                                return Int32.MinValue;

                            if (charVal < 9)
                                total = total * factor - charVal;
                            else
                                total = Int32.MinValue;
                        }
                        else
                        {
                            if (charVal < 0 || charVal > 9)
                                return total;

                            if (i >= maxDigit)
                            {
                                return Int32.MaxValue;
                            }

                            if (total > Int32.MaxValue / 10)
                                return Int32.MaxValue;

                            if (charVal < 8)
                                total = total * factor + charVal;
                            else
                                total = Int32.MaxValue;
                        }

                        break;
                    }
                //}
                //catch
                //{
                //    if (negative)
                //        return Int32.MinValue;
                //    else
                //        return Int32.MaxValue;
                //}

            }

            return total;


        }
    }
}
