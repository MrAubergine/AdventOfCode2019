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
    }

    class Day10 : IDay
    {
        List<Roid> Roids;

        public void Part1()
        {
            ReadMap(InputData);
            CalcCounts();

            int maxvis = 0;
            Roid maxroid = null;
            foreach(Roid r in Roids)
            {
                if(r.v>maxvis)
                {
                    maxvis = r.v;
                    maxroid = r;
                }
            }

            Console.WriteLine("Day10 Part1 Result = {0}", maxvis);
        }

        public void Part2()
        {
            Console.WriteLine("Day10 Part2 Result = {0}", 0);
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

        private String TestData1 =
      @".#..#
        .....
        #####
        ....#
        ...##";

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
