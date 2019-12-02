using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            IDay Day = new Day2();

            Day.Part1();
            Day.Part2();

            Console.WriteLine("****");
            Console.WriteLine("Press any key to Continue");
            Console.ReadKey();
        }
    }
}
