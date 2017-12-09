using System;

namespace LeetCode
{
    public class Solution
    {
        public Solution()
        {
            ;
        }

        public bool IsMatch(string s, string p)
        {
            if (s == null || p == null)
                return false;

            int indexS = s.Length - 1;
            int indexP = p.Length - 1;


            char currentS;
            char currentP;
            int repeatP;
            bool specialChar = false;
            bool lastRecover = false;

            if (indexP >= 0)
            {
                currentP = p[indexP];
            } else
            {
                if (indexS >= 0)
                    return false;
                else
                    return true;
            }

            while (indexS >= 0 && indexP >= 0)
            {
                currentS = s[indexS];
                currentP = p[indexP];

                repeatP = 0;
                specialChar = false;

                if (currentP == '*')
                {
                    specialChar = true;
                    repeatP = Int32.MaxValue;
                    while (indexP > 0 && p[indexP - 1] == '*')
                        --indexP;
                }
                else
                {
                    if (currentP == '.')
                    {
                        repeatP = 1;
                    }
                }

                if(currentP != currentS)
                {
                    if(repeatP == Int32.MaxValue)
                    {
                        if (indexP > 0)
                        {
                            if(p[--indexP] == currentS)
                            {
                                while (indexS > 0 && currentS == s[indexS - 1])
                                    indexS--;
                                indexP--;
                                indexS--;

                                if (indexS < 0)
                                    lastRecover = true;
                            }
                            else
                            {
                                if (p[indexP] == '.')
                                { 
                                    while (indexS > 0 && currentS == s[indexS - 1])
                                        indexS--;
                                    indexS--;

                                    if(indexS >= 0)
                                    {
                                        if(indexP > 0)
                                        {
                                            if (p[indexP-1] == s[indexS])
                                            {
                                                indexP--;
                                            }
                                        }
                                    }
                                }
                                else 
                                    indexP--;
                            }
                        }
                        else
                        {
                            while (indexS > 0 && currentS == s[indexS - 1])
                                indexS--;
                            indexS--;
                        }

                        continue;
                    }
                    else if(repeatP == 1)
                    {
                        indexP--;
                        indexS--;
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    indexP--;
                    indexS--;
                }
            }



            if( (indexS == -1 && indexP == -1))
                return true;
            if (indexS == -1 && indexP <= 1 && currentP == '*')
                return true;

            if (indexP >= 0)
            {
                currentP = p[indexP];
                if (currentP == '*')
                    specialChar = true;
            }

            if(indexS == -1)
            {
                if (specialChar == false)
                    return false;

                while(indexP >= 0)
                {
                    if (currentP == '*')
                    {
                        if (indexP <= 1)
                            return true;
                        else
                        {
                            indexP -= 2;
                            currentP = p[indexP];
                        }

                    }
                    else if (currentP == '.')
                    {
                        if (indexP == 0)
                            return true;
                        else
                        {
                            currentP = p[--indexP];
                            if (currentP == '.' || currentP == '*')
                                return false;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (lastRecover)
                {
                    for (; indexP >= 0; indexP--)
                    {
                        if (s.Length <= 0 || p[indexP] != s[0])
                            return false;
                    }
                }

                if(indexP >= 0)
                    return false;
                return true;
            }

            return false;
        }
    }
}
