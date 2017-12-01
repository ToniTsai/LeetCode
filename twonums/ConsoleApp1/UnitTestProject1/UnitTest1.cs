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
    }
}
