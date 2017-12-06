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
            int test = solution.MyAtoi("    010");
            Assert.AreEqual(10, test);
        }
        [TestMethod]
        public void TestMethod2()
        {
            Solution solution = new Solution();
            int test = solution.MyAtoi("2147483647");
            Assert.AreEqual(2147483647, test);
        }
        [TestMethod]
        public void TestMethod3()
        {
            Solution solution = new Solution();
            int test = solution.MyAtoi("-2147483647");
            Assert.AreEqual(-2147483647, test);
        }

        [TestMethod]
        public void TestMethod4()
        {
            Solution solution = new Solution();
            int test = solution.MyAtoi("2147483648");
            Assert.AreEqual(2147483647, test);
        }

        [TestMethod]
        public void TestMethod5()
        {
            Solution solution = new Solution();
            int test = solution.MyAtoi("      -11919730356x");
            Assert.AreEqual(-2147483648, test);
        }
        [TestMethod]
        public void TestMethod6()
        {
            Solution solution = new Solution();
            int test = solution.MyAtoi("9223372036854775809");
            Assert.AreEqual(2147483647, test);
        }
    }
}
