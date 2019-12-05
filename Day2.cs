using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class Day2 : IDay
    {
        public void Part1()
        {
            ResetMem();

            Mem[1] = 12;
            Mem[2] = 2;

            Execute(0);

            Console.WriteLine("Day2 Part1 Result = {0}", Mem[0]);
        }

        public void Part2()
        {
            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    ResetMem();

                    Mem[1] = noun;
                    Mem[2] = verb;

                    Execute(0);

                    if( Mem[0] == 19690720)
                    {
                        Console.WriteLine("Day2 Part2 Result = {0} {1}", noun, verb);
                        return;
                    }
                }
            }
        }

        private bool Execute(int ip)
        {
            while (Mem[ip] != 99)
            {
                int op1 = Mem[ip + 1];
                int op2 = Mem[ip + 2];
                int op3 = Mem[ip + 3];

                switch (Mem[ip])
                {
                    case 1:
                        Mem[op3] = Mem[op1] + Mem[op2];
                        break;
                    case 2:
                        Mem[op3] = Mem[op1] * Mem[op2];
                        break;
                    default:
                        Console.WriteLine("Program Error at {0}", ip);
                        return false;
                }
                ip += 4;
            }

            return true;
        }

        public void ResetMem()
        {
            Mem = new int[InputData.Length];

            int Addr = 0;
            foreach (int Val in InputData)
            {
                Mem[Addr++] = Val;
            }
        }

        private int[] Mem;

        private int[] InputData = new int[]
        {
            1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,1,6,19,2,19,6,23,1,23,5,27,1,9,27,31,1,31,10,35,2,35,9,39,
            1,5,39,43,2,43,9,47,1,5,47,51,2,51,13,55,1,55,10,59,1,59,10,63,2,9,63,67,1,67,5,71,2,13,71,75,
            1,75,10,79,1,79,6,83,2,13,83,87,1,87,6,91,1,6,91,95,1,10,95,99,2,99,6,103,1,103,5,107,2,6,107,
            111,1,10,111,115,1,115,5,119,2,6,119,123,1,123,5,127,2,127,6,131,1,131,5,135,1,2,135,139,1,
            139,13,0,99,2,0,14,0
        };
    }
}
