using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class IntCodeComputer11
    {
        private Int64[] Mem;
        private List<Int64> Inputs = new List<Int64>();
        private List<Int64> Outputs = new List<Int64>();
        private Int64 InstructionPointer;
        private Int64 RelativeBase;

        public void ExecuteComplete(Int64[] Program, List<Int64> In, List<Int64> Out)
        {
            Reset(Program);

            Inputs = In;
            Outputs = Out;

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

        public void AddInput(Int64 In)
        {
            Inputs.Add(In);
        }

        public bool GetOutput(out Int64 Out)
        {
            if( Outputs.Count>0 )
            {
                Out = Outputs[0];
                Outputs.RemoveAt(0);
                return true;
            }
            else
            {
                Out = 0;
                return false;
            }
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

    class Panel
    {
        public int x;
        public int y;
        public int c;
    }

    class HullPaintingRobot
    {
        private IntCodeComputer11 icc = new IntCodeComputer11();
        private int x, y;
        private Dictionary<Tuple<int, int>, Panel> PanelMap = new Dictionary<Tuple<int, int>, Panel>();
        private int d;
        private int[] xd = new int[] { 0, -1, 0, 1 };
        private int[] yd = new int[] { -1, 0, 1, 0 };


        public void Reset(Int64[] Program)
        {
            icc.Reset(Program);
            x = 0;
            y = 0;
            d = 0;
        }

        public void Run(bool startwhite)
        {
            if (!startwhite)
            {
                icc.AddInput(0);
            }
            else
            {
                Panel p = new Panel();
                p.x = p.y = 0;
                p.c = 1;
                Tuple<int, int> PanelLoc = new Tuple<int, int>(x, y);
                PanelMap.Add(PanelLoc, p);
                icc.AddInput(1);
            }

            int vc = 0;
            Int64[] Vals = new Int64[2];

            while(icc.Step())
            {
                if (icc.GetOutput(out Vals[vc]))
                {
                    vc++;
                    if(vc==2)
                    {
                        vc = 0;

                        Tuple<int, int> PanelLoc = new Tuple<int, int>(x, y);
                        Panel p;
                        if( PanelMap.ContainsKey(PanelLoc))
                        {
                            p = PanelMap[PanelLoc];
                        }
                        else
                        {
                            p = new Panel();
                            p.x = x;
                            p.y = y;
                            PanelMap.Add(PanelLoc, p);
                        }
                        p.c = (int)Vals[0];

                        if (Vals[1] == 0)
                            d = (d + 1) % 4;
                        else
                            d = (d + 3) % 4;

                        x = x + xd[d];
                        y = y + yd[d];

                        PanelLoc = new Tuple<int, int>(x, y);
                        if (PanelMap.ContainsKey(PanelLoc))
                        {
                            p = PanelMap[PanelLoc];
                            icc.AddInput(p.c);
                        }
                        else 
                        {
                            icc.AddInput(0);
                        }
                    }
                }
            }
        }

        public int PanelCount()
        {
            return PanelMap.Count();
        }

        public void DumpImage()
        {
            int minx = 0;
            int maxx = 0;
            int miny = 0;
            int maxy = 0;

            foreach(KeyValuePair<Tuple<int, int>,Panel> p in PanelMap)
            {
                minx = Math.Min(minx, p.Value.x);
                maxx = Math.Max(maxx, p.Value.x);
                miny = Math.Min(miny, p.Value.y);
                maxy = Math.Max(maxy, p.Value.y);
            }

            for(int y=miny; y<=maxy; y++)
            {
                for(int x=minx; x<=maxx; x++)
                {
                    Tuple<int, int> PanelLoc = new Tuple<int, int>(x, y);
                    Panel p;
                    if (PanelMap.ContainsKey(PanelLoc))
                    {
                        p = PanelMap[PanelLoc];
                        if (p.c == 1)
                        {
                            Console.Write('#');
                        }
                        else
                        {
                            Console.Write(' ');
                        }
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
        }
    }
    class Day11 : IDay
    {
        public void Part1()
        {
            HullPaintingRobot r = new HullPaintingRobot();
            r.Reset(InputData);
            r.Run(false);

            Console.WriteLine("Day11 Part1 Result = {0}", r.PanelCount()) ;
        }

        public void Part2()
        {
            HullPaintingRobot r = new HullPaintingRobot();
            r.Reset(InputData);
            r.Run(true);

            Console.WriteLine("Day11 Part2 Result =");
            r.DumpImage();
        }

        private Int64[] InputData = new Int64[]
        {
            3,8,1005,8,326,1106,0,11,0,0,0,104,1,104,0,3,8,102,-1,8,10,101,1,10,10,4,10,1008,8,1,10,4,10,1001,8,0,
            29,2,1003,17,10,1006,0,22,2,106,5,10,1006,0,87,3,8,102,-1,8,10,101,1,10,10,4,10,1008,8,1,10,4,10,1001,
            8,0,65,2,7,20,10,2,9,17,10,2,6,16,10,3,8,102,-1,8,10,1001,10,1,10,4,10,1008,8,0,10,4,10,101,0,8,99,1006,
            0,69,1006,0,40,3,8,102,-1,8,10,1001,10,1,10,4,10,1008,8,1,10,4,10,101,0,8,127,1006,0,51,2,102,17,10,3,
            8,1002,8,-1,10,1001,10,1,10,4,10,108,1,8,10,4,10,1002,8,1,155,1006,0,42,3,8,1002,8,-1,10,101,1,10,10,4,
            10,108,0,8,10,4,10,101,0,8,180,1,106,4,10,2,1103,0,10,1006,0,14,3,8,102,-1,8,10,1001,10,1,10,4,10,108,0,
            8,10,4,10,1001,8,0,213,1,1009,0,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,108,0,8,10,4,10,1002,8,1,239,1006,
            0,5,2,108,5,10,2,1104,7,10,3,8,102,-1,8,10,101,1,10,10,4,10,108,0,8,10,4,10,102,1,8,272,2,1104,12,10,1,
            1109,10,10,3,8,102,-1,8,10,1001,10,1,10,4,10,108,1,8,10,4,10,102,1,8,302,1006,0,35,101,1,9,9,1007,9,
            1095,10,1005,10,15,99,109,648,104,0,104,1,21102,937268449940,1,1,21102,1,343,0,1105,1,447,21101,387365315480,
            0,1,21102,1,354,0,1105,1,447,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,3,10,104,0,104,1,3,10,104,0,
            104,0,3,10,104,0,104,1,21101,0,29220891795,1,21102,1,401,0,1106,0,447,21101,0,248075283623,1,21102,412,1,0,
            1105,1,447,3,10,104,0,104,0,3,10,104,0,104,0,21101,0,984353760012,1,21102,1,435,0,1105,1,447,21102,1,718078227200,
            1,21102,1,446,0,1105,1,447,99,109,2,21202,-1,1,1,21102,40,1,2,21101,0,478,3,21101,468,0,0,1106,0,511,109,-2,
            2106,0,0,0,1,0,0,1,109,2,3,10,204,-1,1001,473,474,489,4,0,1001,473,1,473,108,4,473,10,1006,10,505,1102,1,0,473,
            109,-2,2105,1,0,0,109,4,1202,-1,1,510,1207,-3,0,10,1006,10,528,21102,1,0,-3,22102,1,-3,1,22101,0,-2,2,21101,0,1,
            3,21102,1,547,0,1105,1,552,109,-4,2105,1,0,109,5,1207,-3,1,10,1006,10,575,2207,-4,-2,10,1006,10,575,21202,-4,1,
            -4,1105,1,643,21202,-4,1,1,21201,-3,-1,2,21202,-2,2,3,21102,1,594,0,1106,0,552,22102,1,1,-4,21101,1,0,-1,2207,-4,
            -2,10,1006,10,613,21101,0,0,-1,22202,-2,-1,-2,2107,0,-3,10,1006,10,635,22101,0,-1,1,21101,0,635,0,106,0,510,21202,
            -2,-1,-2,22201,-4,-2,-4,109,-5,2105,1,0
        };
    }
}
