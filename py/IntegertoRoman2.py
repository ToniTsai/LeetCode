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

    numToRoman =(('', 'I', 'II', 'III', 'IV', 'V', 'VI', 'VII', 'VIII', 'IX', 'X'),
                ('', 'X', 'XX', 'XXX', 'XL', 'L', 'LX', 'LXX', 'LXXX', 'XC', 'C'),
                ('', 'C', 'CC', 'CCC', 'CD', 'D', 'DC', 'DCC', 'DCCC', 'CM', 'M'),
                ('', 'M', 'MM', 'MMM') )

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
            roman = self.numToRoman[i][reminder] + roman 
            if(num == 0):
                break;
        
        return roman
        
if __name__ == '__main__':
    unittest.main()
