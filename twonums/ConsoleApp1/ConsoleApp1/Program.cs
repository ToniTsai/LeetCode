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
                if(foundIt > 0)
                {
                    if (myDict.ContainsKey(foundIt) && myDict[foundIt] != i)
                    {
                        retIndex = new int[2];
                        if(i > myDict[foundIt])
                        {
                            retIndex[0] = myDict[foundIt];
                            retIndex[1] = i;
                        }
                        else
                        {
                            retIndex[0] = i;
                            retIndex[1] = myDict[foundIt];
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
