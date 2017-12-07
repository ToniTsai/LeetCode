using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeetCode;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod10()
        {
            Solution solution = new Solution();

            bool actualResult = solution.IsMatch("aa", "a");
            Assert.AreEqual(false, actualResult);

        }

        [TestMethod]
        public void TestMethod2()
        {
            Solution solution = new Solution();

            bool actualResult = solution.IsMatch("aa", "aa");
            Assert.AreEqual(true, actualResult);

        }

        [TestMethod]
        public void TestMethod3()
        {
            Solution solution = new Solution();

            bool actualResult = solution.IsMatch("aaa", "aa");
            Assert.AreEqual(false, actualResult);

        }

        [TestMethod]
        public void TestMethod4()
        {
            Solution solution = new Solution();

            bool actualResult = solution.IsMatch("aaa", "*");
            Assert.AreEqual(true, actualResult);

        }

        [TestMethod]
        public void TestMethod5()
        {
            Solution solution = new Solution();

            bool actualResult = solution.IsMatch("aaa", "a*");
            Assert.AreEqual(true, actualResult);

        }

        [TestMethod]
        public void TestMethod6()
        {
            Solution solution = new Solution();

            bool actualResult = solution.IsMatch("aa", ".*");
            Assert.AreEqual(true, actualResult);

        }

        [TestMethod]
        public void TestMethod7()
        {
            Solution solution = new Solution();

            bool actualResult = solution.IsMatch("ab", ".*");
            Assert.AreEqual(true, actualResult);

        }

        [TestMethod]
        public void TestMethod8()
        {
            Solution solution = new Solution();

            bool actualResult = solution.IsMatch("aab", "c*a*b");
            Assert.AreEqual(true, actualResult);

        }

    }
}
