using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ConsoleApp1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Solution solution = new Solution();

            int[] myTest = {1, 2, 3, 4, 5};
            int[] myResult;

            myResult = solution.TwoSum(myTest, 8);

            Assert.AreEqual(myResult[0], 2);
            Assert.AreEqual(myResult[1], 4);

            myResult = solution.TwoSum(myTest, 9);

            Assert.AreEqual(myResult[0], 3);
            Assert.AreEqual(myResult[1], 4);

            myResult = solution.TwoSum(myTest, 10);

            Assert.AreEqual(myResult, null);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Solution solution = new Solution();

            int[] myTest = { 0, 4, 3, 0 };
            int[] myResult;

            myResult = solution.TwoSum(myTest, 0);

            Assert.AreEqual(myResult[0], 0);
            Assert.AreEqual(myResult[1], 3);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Solution solution = new Solution();

            int[] myTest = { 0, 4, 3, 0 };
            int[] myResult;

            myResult = solution.TwoSum(myTest, 0);

            Assert.AreEqual(myResult[0], 0);
            Assert.AreEqual(myResult[1], 3);
        }
    }
}
