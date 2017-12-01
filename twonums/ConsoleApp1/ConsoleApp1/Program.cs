using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Solution
    {
        public Solution()
        {
            ;
        }
        public int[] TwoSum(int[] nums, int target)
        {
            int[] retIndex;

            Dictionary<int, int> myDict = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
            {
                myDict[nums[i]] = i;
            }

            for (int i = 0; i < nums.Length; i++)
            {
                int foundIt = target - nums[i];

                if (myDict.ContainsKey(foundIt))
                {
                    int value = myDict[foundIt];
                    if (value != i)
                    {
                        retIndex = new int[2];
                        if (i > value)
                        {
                            retIndex[0] = value;
                            retIndex[1] = i;
                        }
                        else
                        {
                            retIndex[0] = i;
                            retIndex[1] = value;
                        }
                        return retIndex;
                    }
                }
            }

            return null;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
