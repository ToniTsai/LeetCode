using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeetCode;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Solution solution = new Solution();
            int length = solution.LengthOfLongestSubstring("abcabcb");
            Assert.AreEqual(3, length);

            length = solution.LengthOfLongestSubstring("bbbbbbb");
            Assert.AreEqual(1, length);
        }
    }
}
