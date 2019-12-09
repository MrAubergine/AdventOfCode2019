﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class IntCodeComputer9
    {
        private Int64[] Mem;
        private List<Int64> Inputs;
        private List<Int64> Outputs;
        private Int64 InstructionPointer;
        private Int64 RelativeBase;

        public void ExecuteComplete(Int64[] Program, List<Int64> In, List<Int64> Out)
        {
            Reset(Program);
            SetIO(In, Out);

            while (Step())
            {
            }

        }

        public void Reset(Int64[] Program)
        {
            InstructionPointer = 0;
            RelativeBase = 0;
            LoadProgram(Program);
        }

        public void SetIO(List<Int64> In, List<Int64> Out)
        {
            Inputs = In;
            Outputs = Out;
        }

        public bool Step()
        {
            Int64 inst = Mem[InstructionPointer];
            Int64 opcode = Mem[InstructionPointer] % 100;
            inst /= 100;
            Int64[] pmode = new Int64[3];
            for (Int64 p = 0; p < 3; p++)
            {
                pmode[p] = inst % 10;
                inst /= 10;
            }

            Int64 ipstep = 0;

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
                case 9: // SetRelativeBase
                    RelativeBase += Read(Mem[InstructionPointer + 1], pmode[0]);
                    ipstep = 2;
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

        private Int64 Read(Int64 Param, Int64 Mode)
        {
            switch (Mode)
            {
                case 0:
                    return Mem[Param];
                case 1:
                    return Param;
                case 2:
                    return Mem[Param+RelativeBase];
                default:
                    Console.WriteLine("Parameter Mode Error on Read {0}", Mode);
                    return 0;
            }
        }

        private void Write(Int64 Param, Int64 Mode, Int64 val)
        {
            switch (Mode)
            {
                case 0:
                    Mem[Param] = val;
                    break;
                case 2:
                    Mem[Param+RelativeBase] = val;
                    break;
                default:
                    Console.WriteLine("Parameter Mode Error on Write {0}", Mode);
                    break;
            }
        }

        private void LoadProgram(Int64[] Program)
        {
            Mem = new Int64[Math.Max(Program.Length*2,4096)];

            Int64 Addr = 0;
            foreach (Int64 Val in Program)
            {
                Mem[Addr++] = Val;
            }
        }
    }
    class Day9 : IDay
    {
        public void Part1()
        {
            IntCodeComputer9 icc = new IntCodeComputer9();

            List<Int64> In = new List<Int64>() { 1 };
            List<Int64> Out = new List<Int64>();

            icc.ExecuteComplete(InputData, In, Out);

            Console.WriteLine("Day9 Part1 Result = ");
            foreach(Int64 oval in Out)
            {
                Console.Write("{0},",oval);
            }
            Console.WriteLine();
        }

        public void Part2()
        {
            IntCodeComputer9 icc = new IntCodeComputer9();

            List<Int64> In = new List<Int64>() { 2 };
            List<Int64> Out = new List<Int64>();

            icc.ExecuteComplete(InputData, In, Out);

            Console.WriteLine("Day9 Part2 Result = ");
            foreach (Int64 oval in Out)
            {
                Console.Write("{0},", oval);
            }
            Console.WriteLine();
        }

        /*       private Int64[] TestData1 = new Int64[]
               {
                   109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99
               };

               private Int64[] TestData2 = new Int64[]
               {
                   1102,34915192,34915192,7,4,7,99,0
               };

               private Int64[] TestData3 = new Int64[]
               {
                   104,1125899906842624,99
               };*/

        private Int64[] InputData = new Int64[]
        {
            1102,34463338,34463338,63,1007,63,34463338,63,1005,63,53,1101,3,0,1000,109,988,209,12,9,1000,209,6,209,3,203,0,1008,1000,
            1,63,1005,63,65,1008,1000,2,63,1005,63,904,1008,1000,0,63,1005,63,58,4,25,104,0,99,4,0,104,0,99,4,17,104,0,99,0,0,1102,1,
            31,1018,1102,352,1,1023,1101,0,1,1021,1101,0,33,1003,1102,1,36,1007,1102,21,1,1005,1101,359,0,1022,1101,0,787,1024,1102,1,24,
            1011,1101,30,0,1014,1101,22,0,1016,1101,0,0,1020,1102,1,29,1000,1101,778,0,1025,1102,23,1,1017,1102,1,28,1002,1101,38,0,1019,
            1102,1,27,1013,1102,1,32,1012,1101,0,37,1006,1101,444,0,1027,1102,1,20,1009,1101,0,447,1026,1101,0,39,1008,1101,35,0,1010,
            1102,559,1,1028,1102,26,1,1004,1102,1,25,1015,1102,1,34,1001,1101,0,554,1029,109,-3,2101,0,9,63,1008,63,34,63,1005,63,205,1001,
            64,1,64,1105,1,207,4,187,1002,64,2,64,109,23,21107,40,39,-7,1005,1013,227,1001,64,1,64,1106,0,229,4,213,1002,64,2,64,109,-17,
            1202,-2,1,63,1008,63,36,63,1005,63,249,1106,0,255,4,235,1001,64,1,64,1002,64,2,64,109,-6,1202,10,1,63,1008,63,36,63,1005,63,
            277,4,261,1106,0,281,1001,64,1,64,1002,64,2,64,109,-2,1208,9,26,63,1005,63,303,4,287,1001,64,1,64,1106,0,303,1002,64,2,64,109,
            32,1206,-7,321,4,309,1001,64,1,64,1106,0,321,1002,64,2,64,109,-29,1207,7,20,63,1005,63,337,1105,1,343,4,327,1001,64,1,64,1002,
            64,2,64,109,27,2105,1,-2,1001,64,1,64,1106,0,361,4,349,1002,64,2,64,109,-25,2108,39,7,63,1005,63,377,1106,0,383,4,367,1001,64,1,
            64,1002,64,2,64,109,1,1201,6,0,63,1008,63,36,63,1005,63,409,4,389,1001,64,1,64,1105,1,409,1002,64,2,64,109,1,2102,1,1,63,1008,63,
            33,63,1005,63,435,4,415,1001,64,1,64,1105,1,435,1002,64,2,64,109,28,2106,0,-3,1106,0,453,4,441,1001,64,1,64,1002,64,2,64,109,-13,
            21101,41,0,1,1008,1018,44,63,1005,63,477,1001,64,1,64,1106,0,479,4,459,1002,64,2,64,109,4,21108,42,42,-2,1005,1019,501,4,485,1001,
            64,1,64,1106,0,501,1002,64,2,64,109,-21,2101,0,2,63,1008,63,28,63,1005,63,523,4,507,1105,1,527,1001,64,1,64,1002,64,2,64,109,26,1205,
            -5,545,4,533,1001,64,1,64,1105,1,545,1002,64,2,64,109,3,2106,0,-1,4,551,1106,0,563,1001,64,1,64,1002,64,2,64,109,-33,1201,4,0,63,
            1008,63,28,63,1005,63,583,1105,1,589,4,569,1001,64,1,64,1002,64,2,64,109,11,2107,27,-3,63,1005,63,609,1001,64,1,64,1106,0,611,4,595,
            1002,64,2,64,109,8,21102,43,1,3,1008,1018,43,63,1005,63,637,4,617,1001,64,1,64,1105,1,637,1002,64,2,64,109,-5,21108,44,41,0,1005,
            1010,653,1105,1,659,4,643,1001,64,1,64,1002,64,2,64,109,-13,2108,21,8,63,1005,63,681,4,665,1001,64,1,64,1106,0,681,1002,64,2,64,109,
            6,1207,0,34,63,1005,63,703,4,687,1001,64,1,64,1105,1,703,1002,64,2,64,109,7,1208,-7,35,63,1005,63,723,1001,64,1,64,1106,0,725,4,709,
            1002,64,2,64,109,-13,2102,1,7,63,1008,63,23,63,1005,63,745,1105,1,751,4,731,1001,64,1,64,1002,64,2,64,109,13,1205,10,767,1001,64,1,
            64,1105,1,769,4,757,1002,64,2,64,109,14,2105,1,0,4,775,1001,64,1,64,1106,0,787,1002,64,2,64,109,-20,21107,45,46,7,1005,1011,809,4,
            793,1001,64,1,64,1105,1,809,1002,64,2,64,109,-3,2107,25,3,63,1005,63,827,4,815,1106,0,831,1001,64,1,64,1002,64,2,64,109,13,1206,7,
            847,1001,64,1,64,1106,0,849,4,837,1002,64,2,64,109,-11,21101,46,0,7,1008,1010,46,63,1005,63,871,4,855,1106,0,875,1001,64,1,64,1002,
            64,2,64,109,15,21102,47,1,-4,1008,1014,48,63,1005,63,895,1106,0,901,4,881,1001,64,1,64,4,64,99,21102,27,1,1,21101,0,915,0,1106,0,
            922,21201,1,63208,1,204,1,99,109,3,1207,-2,3,63,1005,63,964,21201,-2,-1,1,21102,1,942,0,1106,0,922,21202,1,1,-1,21201,-2,-3,1,21101,
            957,0,0,1105,1,922,22201,1,-1,-2,1106,0,968,21201,-2,0,-2,109,-3,2106,0,0
        };
    }
}
