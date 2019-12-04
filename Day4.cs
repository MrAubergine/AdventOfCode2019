using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
     class Day4 : IDay
    {
        public void Part1()
        {
            int cnt = 0;

            for (int n=245318; n<=765747; n++)
            {
                if (CheckValid(n))
                    cnt++;
            }

            Console.WriteLine("Day4 Part1 Result = {0}", cnt);
        }

        public void Part2()
        {
            int cnt = 0;

            for (int n = 245318; n <= 765747; n++)
            {
                if (CheckValid(n,true))
                    cnt++;
            }

            Console.WriteLine("Day4 Part2 Result = {0}", cnt);
        }

        bool CheckValid(int n, bool bComplexRepCheck=false)
        {
            Dictionary<char, int>  reps = new Dictionary<char, int>();

            String sn = n.ToString();
            bool inrep = false;
            for (int c=1; c<sn.Length; c++)
            {
                if (sn[c - 1] > sn[c])
                    return false;
                if (sn[c - 1] == sn[c])
                {
                    if (reps.ContainsKey(sn[c]))
                    {
                        if (inrep)
                        {
                            reps[sn[c]] += 1;
                        }
                        else
                        {
                            reps[sn[c]] = 1;
                        }
                    }
                    else
                    {
                        reps.Add(sn[c], 1);
                    }
                    inrep = true;
                }
                else
                {
                    inrep = false;
                }
            }

            if( bComplexRepCheck )
            {
                foreach( KeyValuePair<char,int> repinfo in reps)
                {
                    if (repinfo.Value == 1)
                        return true;
                }
                return false;
            }
            else
            {
                return reps.Count > 0;
            }
        }
    }
}
