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
            string s = solution.LongestPalindrome("abab");
            Assert.AreEqual("bab", s);
            s = solution.LongestPalindrome("abccbadabccba");
            Assert.AreEqual("abccbadabccba", s);

        }
    }
}
