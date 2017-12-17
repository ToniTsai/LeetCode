import unittest

class TestItoR(unittest.TestCase):
    def setUp(self):
        pass
    
    def test_num_1(self):
        a = Solution()
        b = a.intToRoman(121)
        self.assertEqual('CXXI', b)

    def test_num_2(self):
        a = Solution()
        b = a.intToRoman(100)
        self.assertEqual('C', b)

    def test_num_3(self):
        a = Solution()
        b = a.intToRoman(1398)
        self.assertEqual('MCCCXCVIII', b)

class Solution:

    numToRoman =[{1:'I', 2:'II', 3:'III', 4: 'IV', 5: 'V', 6:'VI', 7:'VII', 8:'VIII', 9:'IX', 10: 'X'},
                {1:'X', 2:'XX', 3:'XXX', 4: 'XL', 5: 'L', 6:'LX', 7:'LXX', 8:'LXXX', 9:'XC', 10: 'C'},
                {1:'C', 2:'CC', 3:'CCC', 4: 'CD', 5: 'D', 6:'DC', 7:'DCC', 8:'DCCC', 9:'CM', 10: 'M'},
                {1:'M', 2:'MM', 3:'MMM'} ]

    def intToRoman(self, num):
        """
        :type num: int
        :rtype: str
        """

        if(num <= 0):
            return None

        if(num > 3999):
            return None

        factor = 10
        roman = '';
        num = int(num)
        for i in range(4):
            reminder = int(int(num) % factor)
            num = num / factor
            if reminder != 0:
                roman = self.numToRoman[i][reminder] + roman 
            if(num == 0):
                break;
        
        return roman
        
if __name__ == '__main__':
    unittest.main()
