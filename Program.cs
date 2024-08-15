using LPR_Project;
using System;
using System.IO;

namespace LPR_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Linear and Integer Programming Solver");
            Console.WriteLine("1. Solve LP/IP Model");
            Console.WriteLine("2. Exit");

            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                Console.Write("Enter input file path: ");
                string inputFilePath = Console.ReadLine();

                Console.Write("Enter output file path: ");
                string outputFilePath = Console.ReadLine();

                string[] inputLines = File.ReadAllLines(inputFilePath);

                var model = Parser.ParseInput(inputLines);
                var result = Solver.Solve(model);

                File.WriteAllLines(outputFilePath, result);
                Console.WriteLine("Solution written to output file.");
            }
            else if (choice == 2)
            {
                Environment.Exit(0);
            }
        }
    }
}
