using System;

namespace LeetCode
{
    /**
 * Definition for singly-linked list.
 * **/
 public class ListNode {
      public int val;
      public ListNode next;
      public ListNode(int x) { val = x; }
  }
 
    public class Solution
    {
        public Solution()
        {
            ;
        }

        Int64 HalfMaxInt64 = Int64.MaxValue / 10;
        // If Int64 overflow then return -1
        public Int64 GetIntegter(ListNode list)
        {
            Int64 retInt = 0;
            Int64 factor = 1;
            while (list != null)
            {
                retInt = retInt + list.val * factor;
                list = list.next;

                factor *= 10;

                if (factor >= HalfMaxInt64)
                    return -1;

            }

            return retInt;
        }

        public ListNode AddTwoNumbersLong(ListNode l1, ListNode l2)
        {
            int leftValue = 0;
            ListNode lhead = new ListNode(l1.val + l2.val);
            ListNode ltail = lhead;
            ltail.next = null;
            if(ltail.val >= 10)
            {
                ltail.val -= 10;
                leftValue = 1;
            }

            l1 = l1.next;
            l2 = l2.next;

            while (l1 != null && l2 != null)
            {
                ltail.next = new ListNode(l1.val + l2.val + leftValue);

                ltail = ltail.next;

                if (ltail.val >= 10)
                {
                    ltail.val -= 10;
                    leftValue = 1;
                }
                else
                    leftValue = 0;
                l1 = l1.next;
                l2 = l2.next;
            }

            while (l1 != null)
            {
                ltail.next = new ListNode(l1.val + leftValue);

                ltail = ltail.next;

                if (ltail.val >= 10)
                {
                    ltail.val -= 10;
                    leftValue = 1;
                }
                else
                    leftValue = 0;
                l1 = l1.next;

            }

            while (l2 != null)
            {
                ltail.next = new ListNode(l2.val + leftValue);

                ltail = ltail.next;

                if (ltail.val >= 10)
                {
                    ltail.val -= 10;
                    leftValue = 1;
                }
                else
                    leftValue = 0;
                l2 = l2.next;

            }

            if (leftValue != 0)
            {
                ltail.next = new ListNode(leftValue);

                ltail = ltail.next;
            }

            ltail.next = null;

            return lhead;
        }

        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            if (l1 == null || l2 == null)
            {
                if (l2 == null)
                {
                    if (l1 == null)
                        return null;
                    else
                        return l1;
                }
                else
                {
                    return l2;
                }
            }


            Int64 total= GetIntegter(l1);
            Int64 total2 = GetIntegter(l2);

            if (total == -1 || total2 == -1)
               return AddTwoNumbersLong(l1, l2);

            total = total + total2;
            Int64 val = total % 10;

            ListNode headList = new ListNode((int) val);
            ListNode tailList = headList;
            headList.next = null;

            total = total / 10;

            while(total != 0)
            {
                val = total % 10;
                tailList.next = new ListNode((int) val);
                total = total / 10;
                tailList = tailList.next;
                tailList.next = null;
            }

            return headList;
        }
    }
}
