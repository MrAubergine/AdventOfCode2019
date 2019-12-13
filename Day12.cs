using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class Moon
    {
        public int x;
        public int y;
        public int z;
        public int dx;
        public int dy;
        public int dz;

        public Moon(int ix, int iy, int iz)
        {
            x = ix;
            y = iy;
            z = iz;
            dx = dy = dz = 0;
        }

        public Moon(Moon m)
        {
            x = m.x;
            y = m.y;
            z = m.z;
            dx = dy = dz = 0;
        }

        public void ApplyGravity(Moon m)
        {
            if (x<m.x)
            {
                dx += 1;
                m.dx -= 1;
            }
            else if (x>m.x)
            {
                dx -= 1;
                m.dx += 1;
            }

            if (y < m.y)
            {
                dy += 1;
                m.dy -= 1;
            }
            else if (y > m.y)
            {
                dy -= 1;
                m.dy += 1;
            }

            if (z < m.z)
            {
                dz += 1;
                m.dz -= 1;
            }
            else if (z > m.z)
            {
                dz -= 1;
                m.dz += 1;
            }
        }

        public void ApplyVelocity()
        {
            x += dx;
            y += dy;
            z += dz;
        }

        public void Dump()
        {
            Console.WriteLine("pos=<x={0}, y={1}, z={2}>, vel=<x={3}, y={4}, z={5}>", x, y, z, dx, dy, dz);
        }

        public int Energy()
        {
            return (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) * (Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz));
        }

        public bool SamePos(Moon m)
        {
            return (m.x == x) && (m.y == y) && (m.z == z) && (m.dx==dx) && (m.dy==y) && (m.dz==z);
        }
    }

    class Day12 : IDay
    {
        public void Part1()
        {
            for (int s = 0; s < 1000; s++)
            {
                Step(InputData);
            }

            Console.WriteLine("Day12 Part1 Result = {0}", Energy(InputData));
        }

        public void Part2()
        {
            Moon[] Data = TestData;

            Moon[] InitData = new Moon[Data.Length];
            for (int m = 0; m < Data.Length; m++)
            {
                InitData[m] = new Moon(Data[m]);       
            }

            int step = 1;
            Step(Data);
            while( !SamePos(InitData, Data) )
            {
                Step(Data);
                step++;
            }

            Console.WriteLine("Day12 Part2 Result = {0}", step);
        }

        private void Step(Moon[] Data)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                for (int j = i + 1; j < Data.Length; j++)
                {
                    Data[i].ApplyGravity(Data[j]);
                }
            }

            foreach (Moon m in Data)
            {
                m.ApplyVelocity();
            }
        }

        private void Dump(Moon[] Data)
        {
            foreach (Moon m in Data)
            {
                m.Dump();
            }
            Console.WriteLine();
        }

        private int Energy(Moon[] Data)
        {
            int te = 0;
            foreach (Moon m in Data)
            {
                te += m.Energy();
            }
            return te;
        }

        private bool SamePos(Moon[] m1, Moon[] m2)
        {
            for (int m = 0; m < m1.Length; m++)
            {
                if (!m1[m].SamePos(m2[m]))
                    return false;
            }
            return true;
        }

        private Moon[] TestData = new Moon[]
        {
            new Moon(-1,0,2),
            new Moon(2,-10,-7),
            new Moon(4,-8,8),
            new Moon(3,5,-1)
        };

        private Moon[] InputData = new Moon[]
        {
            new Moon(1,2,-9),
            new Moon(-1,-9,-4),
            new Moon(17,6,8),
            new Moon(12,4,2)
        };
    }
}
