using System;

namespace LeetCode
{
    public class Solution
    {
        /**
         * Definition for singly-linked list.
         * **/
         public class ListNode {
             public int val;
             public ListNode next;
             public ListNode(int x) { val = x; }
         }
         
        public ListNode ToList(int[] a)
        {
            ListNode l = null;
            ListNode h = null;
            for (int i = 0; i < a.Length; i++)
            {
                ListNode b = new ListNode(a[i]);
                b.next = null;

                if (l == null)
                {
                    l = b;
                    h = l;
                }
                else
                {
                    l.next = b;

                }
            }

            return h;
        }

        public ListNode ToInt(ListNode a)
        {
            
        }

        public Solution()
        {
            ;
        }
        public ListNode ReverseKGroup(ListNode head, int k)
        {
            ListNode current, next;
            ListNode prevTail, currentTail;
            ListNode newHead;

            if (head == null || head.next == null)
                return head;

            current = head;
            prevTail = head;

            currentTail = null;
            next = current.next;

            newHead = null;

            ListNode t1 = head;

            for (int i = 0; i < k; i++)
            {
                if (t1 == null)
                    return head;
                t1 = t1.next;
            }

            while (current != null)
            {
                for (int i = 0; i < k; i++)
                {
                    ListNode temp;

                    if (i == 0)
                    {
                        if (k == 1)
                            prevTail = current;
                        else
                            currentTail = current;
                    }

                    if (next == null)
                    {
                        if (newHead == null)
                        {
                            newHead = current;
                            prevTail.next = null;
                        }
                        else
                        {
                            prevTail.next = current;

                            if (k != 1)
                            {
                                prevTail.next = current;
                                currentTail.next = null;
                            }
                            else
                            {
                                prevTail.next = null;
                            }
                        }

                        return newHead;
                    }

                    if (i == (k - 1))
                    {
                        if (newHead == null)
                        {
                            newHead = current;
                        }

                        prevTail.next = current;
                        prevTail = currentTail;
                    }

                    temp = next.next;
                    next.next = current;
                    current = next;
                    next = temp;

                }


            }

            prevTail.next = null;

            return newHead;
        }
    }
}
