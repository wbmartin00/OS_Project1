using System;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;

            // Read from standard input until the end of the stream
            while ((line = Console.ReadLine()) != null)
            {
                // Example processing: convert the line to uppercase
                Console.WriteLine(line.ToUpper());
            }
        }
    }
}