using System;
using System.IO;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool sendLargeFile = true;
            string filePath = "large file test.txt"; // Ensure this file exists in the same directory

            // Sample data to output
            string[] data = { "Hello", "This is a test", "IPC via pipes", "C# is awesome!" };

            // Write each line to standard output
            foreach (var line in data)
            {
                Console.WriteLine(line);
            }

            if (sendLargeFile)
            {
                if (File.Exists(filePath))
                {
                    foreach (var line in File.ReadLines(filePath))
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.WriteLine($"Error: File '{filePath}' not found.");
                }
            }
        }
    }
}