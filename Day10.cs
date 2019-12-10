using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class Roid
    {
        public float x;
        public float y;
        public int v = 0;
        public float a;
        public float d;
    }

    class Day10 : IDay
    {
        List<Roid> Roids;
        Roid Station;

        public void Part1()
        {
            ReadMap(InputData);
            CalcCounts();

            int maxvis = 0;
            foreach(Roid r in Roids)
            {
                if(r.v>maxvis)
                {
                    maxvis = r.v;
                    Station = r;
                }
            }

            Console.WriteLine("Day10 Part1 Result = {0}", maxvis);
        }

        public void Part2()
        {
            CalcAngles();

            float angle = 0.0f;
            Roid r = Target(angle,false);
            for( int a=0;a<199;a++ )
            {
                angle = r.a;
                Roids.Remove(r);
                r = Target(angle,true);
            }

            Console.WriteLine("Day10 Part2 Result = {0}", r.x*100+r.y);
        }

        void ReadMap(String In)
        {
            Roids = new List<Roid>();
            float x = 0.0f;
            float y = 0.0f;

            foreach (char c in In)
            {
                switch(c)
                {
                    case '#':
                        Roid r = new Roid();
                        r.x = x;
                        r.y = y;
                        Roids.Add(r);
                        x += 1.0f;
                        break;
                    case '.':
                        x += 1.0f;
                        break;
                    case '\n':
                        y += 1.0f;
                        x = 0.0f;
                        break;
                    default:
                        break;
                }
            }
        }

        void CalcCounts()
        {
            foreach(Roid r1 in Roids)
            {
                foreach(Roid r2 in Roids)
                {
                    if (r1 != r2 )
                        if(CanSee(r1, r2))
                            r1.v += 1;
                }
            }
        }

        bool CanSee(Roid r1, Roid r2)
        {
            foreach(Roid rblock in Roids)
            {
                if (rblock == r1 || rblock == r2)
                    continue;

                float dist2sq = (r2.x - r1.x) * (r2.x - r1.x) + (r2.y - r1.y) * (r2.y - r1.y);
                float distbsq = (rblock.x - r1.x) * (rblock.x - r1.x) + (rblock.y - r1.y) * (rblock.y - r1.y);
                if (distbsq <= dist2sq)
                {
                    float dotp = ((r2.x - r1.x) * (rblock.x - r1.x)) + ((r2.y - r1.y) * (rblock.y - r1.y));
                    if (Math.Abs(dotp - Math.Sqrt(dist2sq)*Math.Sqrt(distbsq)) < 0.000001)
                        return false;
                }
            }

            return true;
        }

        void CalcAngles()
        {
            foreach(Roid r in Roids)
            {
                if (r == Station)
                    continue;

                r.d = (r.x - Station.x) * (r.x - Station.x) + (r.y - Station.y) * (r.y - Station.y);
                r.a = (float)Math.Atan2(r.x - Station.x, Station.y - r.y);
                if (r.a < 0)
                    r.a += 2.0f*(float)Math.PI;
            }
        }

        Roid Target(float angle, bool AngleMustBeLarger)
        {
            float mina = float.MaxValue;
            float mindist = float.MaxValue;
            Roid tr = null;

            foreach(Roid r in Roids)
            {
                if (r == Station)
                    continue;

                float a = r.a - angle;
                if (a < 0)
                    a += 2.0f * (float)Math.PI;
                if(AngleMustBeLarger && a<= 0.000001f)
                {
                    continue;
                }
                if( a<mina || (a==mina && r.d<mindist) )
                {
                    mina = a;
                    mindist = r.d;
                    tr = r;
                }
            }

            return tr;
        }

        private String TestData1 =
      @".#....#####...#..
        ##...##.#####..##
        ##...#...#.#####.
        ..#.....#...###..
        ..#.#.....#....##";

        private String TestData =
            @".#..##.###...#######
            ##.############..##.
            .#.######.########.#
            .###.#######.####.#.
            #####.##.#.##.###.##
            ..#####..#.#########
            ####################
            #.####....###.#.#.##
            ##.#################
            #####.##.###..####..
            ..######..##.#######
            ####.##.####...##..#
            .#####..#.######.###
            ##...#.##########...
            #.##########.#######
            .####.#.###.###.#.##
            ....##.##.###..#####
            .#.#.###########.###
            #.#.#.#####.####.###
            ###.##.####.##.#..##";

        private String InputData =
           @".#.####..#.#...#...##..#.#.##.
            ..#####.##..#..##....#..#...#.
            ......#.......##.##.#....##..#
            ..#..##..#.###.....#.#..###.#.
            ..#..#..##..#.#.##..###.......
            ...##....#.##.#.#..##.##.#...#
            .##...#.#.##..#.#........#.#..
            .##...##.##..#.#.##.#.#.#.##.#
            #..##....#...###.#..##.#...##.
            .###.###..##......#..#...###.#
            .#..#.####.#..#....#.##..#.#.#
            ..#...#..#.#######....###.....
            ####..#.#.#...##...##....#..##
            ##..#.##.#.#..##.###.#.##.##..
            ..#.........#.#.#.#.......#..#
            ...##.#.....#.#.##........#..#
            ##..###.....#.............#.##
            .#...#....#..####.#.#......##.
            ..#..##..###...#.....#...##..#
            ...####..#.#.##..#....#.#.....
            ####.#####.#.#....#.#....##.#.
            #.#..#......#.........##..#.#.
            #....##.....#........#..##.##.
            .###.##...##..#.##.#.#...#.#.#
            ##.###....##....#.#.....#.###.
            ..#...#......#........####..#.
            #....#.###.##.#...#.#.#.#.....
            .........##....#...#.....#..##
            ###....#.........#..#..#.#.#..
            ##...#...###.#..#.###....#.##.";
    }
}
