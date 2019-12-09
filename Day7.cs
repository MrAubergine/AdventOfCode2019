using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class IntCodeComputer7
    {
        private int[] Mem;
        private List<int> Inputs;
        private List<int> Outputs;
        private int InstructionPointer;

        public void ExecuteComplete(int[] Program, List<int> In, List<int> Out)
        {
            Reset(Program);
            SetIO(In, Out);

            while (Step())
            {
            }

        }

        public void Reset(int[] Program)
        {
            InstructionPointer = 0;
            LoadProgram(Program);
        }

        public void SetIO(List<int> In, List<int> Out)
        {
            Inputs = In;
            Outputs = Out;
        }

        public bool Step()
        {
            int inst = Mem[InstructionPointer];
            int opcode = Mem[InstructionPointer] % 100;
            inst /= 100;
            int[] pmode = new int[3];
            for (int p = 0; p < 3; p++)
            {
                pmode[p] = inst % 10;
                inst /= 10;
            }

            int ipstep = 0;

            switch (opcode)
            {
                case 1: // Add
                    Write(Mem[InstructionPointer + 3], pmode[2], Read(Mem[InstructionPointer + 1], pmode[0]) + Read(Mem[InstructionPointer + 2], pmode[1]));
                    ipstep = 4;
                    break;
                case 2: // Mul
                    Write(Mem[InstructionPointer + 3], pmode[2], Read(Mem[InstructionPointer + 1], pmode[0]) * Read(Mem[InstructionPointer + 2], pmode[1]));
                    ipstep = 4;
                    break;
                case 3: // Read
                    if (Inputs.Count > 0)
                    {
                        Write(Mem[InstructionPointer + 1], pmode[0], Inputs[0]);
                        Inputs.RemoveAt(0);
                        ipstep = 2;
                    }
                    else
                    {
                        ipstep = 0;
                    }
                    break;
                case 4: // Write
                    Outputs.Add(Read(Mem[InstructionPointer + 1], pmode[0]));
                    ipstep = 2;
                    break;
                case 5: // Jump if true
                    if (Read(Mem[InstructionPointer + 1], pmode[0]) != 0)
                    {
                        ipstep = Read(Mem[InstructionPointer + 2], pmode[1]) - InstructionPointer;
                    }
                    else
                    {
                        ipstep = 3;
                    }
                    break;
                case 6: // Jump if false
                    if (Read(Mem[InstructionPointer + 1], pmode[0]) == 0)
                    {
                        ipstep = Read(Mem[InstructionPointer + 2], pmode[1]) - InstructionPointer;
                    }
                    else
                    {
                        ipstep = 3;
                    }
                    break;
                case 7: // Less than
                    Write(Mem[InstructionPointer + 3], pmode[2], (Read(Mem[InstructionPointer + 1], pmode[0]) < Read(Mem[InstructionPointer + 2], pmode[1])) ? 1 : 0);
                    ipstep = 4;
                    break;
                case 8: // Equals
                    Write(Mem[InstructionPointer + 3], pmode[2], (Read(Mem[InstructionPointer + 1], pmode[0]) == Read(Mem[InstructionPointer + 2], pmode[1]) ? 1 : 0));
                    ipstep = 4;
                    break;
                case 99: // Stop
                    return false;
                default:
                    Console.WriteLine("Program Error at {0}", InstructionPointer);
                    return false;
            }

            InstructionPointer += ipstep;

            return true;
        }

        private int Read(int Param, int Mode)
        {
            switch (Mode)
            {
                case 0:
                    return Mem[Param];
                case 1:
                    return Param;
                default:
                    Console.WriteLine("Parameter Mode Error on Read {0}", Mode);
                    return 0;
            }
        }

        private void Write(int Param, int Mode, int val)
        {
            switch (Mode)
            {
                case 0:
                    Mem[Param] = val;
                    break;
                default:
                    Console.WriteLine("Parameter Mode Error on Write {0}", Mode);
                    break;
            }
        }

        private void LoadProgram(int[] Program)
        {
            Mem = new int[Program.Length];

            int Addr = 0;
            foreach (int Val in Program)
            {
                Mem[Addr++] = Val;
            }
        }
    }
    class Day7 : IDay
    {
        public void Part1()
        {
            List<int> phase1 = new List<int>() { 4, 3, 2, 1, 0 };
            List<int> phase2 = new List<int>();

            int largest = 0;
            FindLargestPermutePart(phase1, phase2, ref largest);

            Console.WriteLine("Day2 Part1 Result = {0}", largest);
        }

        public void Part2()
        {
            List<int> phase1 = new List<int>() { 5, 6, 7, 8, 9 };
            List<int> phase2 = new List<int>();

            int largest = 0;
            FindLargestPermutePart(phase1, phase2, ref largest);

            Console.WriteLine("Day2 Part1 Result = {0}", largest);
        }

        void FindLargestPermutePart(List<int> phase1, List<int> phase2, ref int largest)
        {
            int[] testphase = new int[5];
            for ( int p=0; p<phase1.Count; p++ )
            {
                List<int> newphase1 = new List<int>(phase1);
                List<int> newphase2 = new List<int>(phase2);
                newphase2.Add(newphase1[p]);
                newphase1.RemoveAt(p);
                int tp = 0;
                foreach (int phase in newphase2)
                    testphase[tp++] = phase;
                foreach (int phase in newphase1)
                    testphase[tp++] = phase;

                int ampout = RunAmp(testphase);
                if (ampout > largest)
                    largest = ampout;

                FindLargestPermutePart(newphase1, newphase2, ref largest);
            }
        }

        private int RunAmp(int[] phase)
        {
            IntCodeComputer7[] amp = new IntCodeComputer7[5];
            List<int>[] In = new List<int>[5];
            List<int>[] Out = new List<int>[5];

            for (int a = 0; a < 5; a++)
            {
                amp[a] = new IntCodeComputer7();
                In[a] = new List<int>();
                Out[a] = new List<int>();

                amp[a].Reset(InputData);
                amp[a].SetIO(In[a], Out[a]);
                In[a].Add(phase[a]);
            }
            In[0].Add(0);

            int outval = -1;
            int running = 5;
            while (running > 0)
            {
                running = 0;
                for (int a = 0; a < 5; a++)
                {
                    if (amp[a].Step())
                    {
                        running++;
                    }
                    if (Out[a].Count > 0)
                    {
                        if(a==4)
                        {
                            outval = Out[4][0];
                        }
                        In[(a + 1) % 5].Add(Out[a][0]);
                        Out[a].RemoveAt(0);
                    }
                }
            }

            return outval;
        }

        /*private int[] TestData1 = new int[]
        {
            3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0
        };*/

        private int[] InputData = new int[]
        {
            3,8,1001,8,10,8,105,1,0,0,21,42,67,84,109,122,203,284,365,446,99999,3,9,1002,9,3,9,1001,9,5,9,
            102,4,9,9,1001,9,3,9,4,9,99,3,9,1001,9,5,9,1002,9,3,9,1001,9,4,9,102,3,9,9,101,3,9,9,4,9,99,3,9,
            101,5,9,9,1002,9,3,9,101,5,9,9,4,9,99,3,9,102,5,9,9,101,5,9,9,102,3,9,9,101,3,9,9,102,2,9,9,4,9,
            99,3,9,101,2,9,9,1002,9,3,9,4,9,99,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,
            9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,
            9,4,9,3,9,1002,9,2,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,
            4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,
            9,1002,9,2,9,4,9,99,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,
            101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,
            1,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,
            4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,
            99,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,
            1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,99
        };
    }
}
