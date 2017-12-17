import unittest

class TestItoR(unittest.TestCase):
    def setUp(self):
        pass
    
    def test_num_1(self):
        a = Solution()
        b = a.longestCommonPrefix(["abc", "abc", "abcdef", "abcdefgrtyh"])
        self.assertEqual('abc', b)

    def test_num_2(self):
        a = Solution()
        b = a.longestCommonPrefix(["abc", "abc", "abcdef", "qabcdefgrtyh"])
        self.assertEqual('', b)

class Solution:
    def longestCommonPrefix(self, strs):
        """
        :type strs: List[str]
        :rtype: str
        """
        minLen = min(map(lambda x: len(x), strs))
        
        l = 0
        r = minLen
        
        if r < 5:
            for i in range(1, len(strs), 1):
                if strs[i].startswith(strs[0],0, r) == False:
                    break
            else:
                return strs[0][:i+1]
        
        return ''
        
        

if __name__ == '__main__':
    unittest.main()
