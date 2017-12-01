using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeetCode;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInteger64()
        {
            Solution solution = new Solution();

            ListNode l1 = new ListNode(2);
            ListNode headL1 = l1;
            l1.next = new ListNode(4);
            l1 = l1.next;
            l1.next = new ListNode(3);
            l1.next.next = null;
            Assert.AreEqual(342, solution.GetIntegter(headL1));

            ListNode l2 = new ListNode(5);
            ListNode headL2 = l2;
            l2.next = new ListNode(6);
            l2 = l2.next;
            l2.next = new ListNode(4);
            l2.next.next = null;
            Assert.AreEqual(465, solution.GetIntegter(headL2));

            l1 = solution.AddTwoNumbers(headL1, headL2);
            Assert.AreEqual(807, solution.GetIntegter(l1));


        }

        [TestMethod]
        public void TestIntegerOverflow()
        {
            // Design error 
            // The code won't work if there is huge long integers
            // for huge long version we need to take another code path
            // Int64 overflow

            Solution solution = new Solution();

            ListNode l1 = new ListNode(2);
            ListNode headL1 = l1;
            l1.next = new ListNode(4);
            l1 = l1.next;
            l1.next = new ListNode(3);
            l1.next.next = null;
            Assert.AreEqual(342, solution.GetIntegter(headL1));

            ListNode l2 = new ListNode(5);
            ListNode headL2 = l2;
            l2.next = new ListNode(6);
            l2 = l2.next;
            l2.next = new ListNode(4);
            l2.next.next = null;
            Assert.AreEqual(465, solution.GetIntegter(headL2));

            l1 = solution.AddTwoNumbersLong(headL1, headL2);
            Assert.AreEqual(807, solution.GetIntegter(l1));


        }

        public ListNode CreateList(int [] val)
        {
            ListNode lhead, l1 = new ListNode(val[0]);

            lhead = l1;

            for(int i =1; i < val.Length; i++)
            {
                l1.next = new ListNode(val[i]);
                l1 = l1.next;
            }

            l1.next = null;

            return lhead;
        }
        [TestMethod]
        public void TestIntegerOverflow2()
        {
            // Design error 
            // The code won't work if there is huge long integers
            // for huge long version we need to take another code path
            // Int64 overflow

            int[] val1 = { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };
            int[] val2 = { 1 };

            Solution solution = new Solution();

            ListNode l1 = CreateList(val1);
            Assert.AreEqual(-1, solution.GetIntegter(l1));

            ListNode l2 = CreateList(val2);
            Assert.AreEqual(1, solution.GetIntegter(l2));

            l1 = solution.AddTwoNumbersLong(l1, l2);
           // Assert.AreEqual(807, solution.GetIntegter(l1));


        }
    }
}
