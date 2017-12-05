using System;

namespace LeetCode
{
    public class Solution
    {
        public Solution()
        {
            ;
        }
        public double FindMedianSortedArrays(int[] nums1, int[] nums2)
        {
            int l1, l1data, r1, r1data, m1, m1data;
            int l2, l2data, r2, r2data, m2, m2data;
            int mp1, mp2;

            int dataLength = nums1.Length + nums2.Length;

            if (dataLength % 2)
            {
                mp1 = (dataLength + 1) / 2;
                mp2 = 0;
            }
            else
            {
                mp1 = dataLength  / 2;
                mp2 = mp1 + 1;
            }
        }
    }
}
