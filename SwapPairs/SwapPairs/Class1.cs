using System;

namespace LeetCode
{
    /**
     * Definition for singly-linked list.
     **/
    public class ListNode {
         public int val;
         public ListNode next;
         public ListNode(int x) { val = x; }
    }
    

    public class Solution
    {
        public ListNode SwapPairs(ListNode head)
        {
            ListNode next = head;

            if (next == null || next.next == null)
                return head;

            ListNode current = next.next;

            head = current;

            while (true)
            {
                ListNode temp = current.next;
                current.next = next;

                if (temp == null || temp.next == null)
                {
                    next.next = temp;
                    return head;
                }

                current = temp.next;
                next.next = current;
                next = temp;
            }

            return head;
        }
    }
}
