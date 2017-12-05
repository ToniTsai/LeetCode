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
            string s = solution.Convert("PAYPALISHIRING", 3);
            Assert.AreEqual("PAHNAPLSIIGYIR", s);
        }
    }
}
