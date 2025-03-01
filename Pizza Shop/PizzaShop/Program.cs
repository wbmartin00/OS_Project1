using System;
using System.Threading;
using System.Collections.Generic;

class Program
{
    static Kitchen kitchen = new Kitchen();
    static Random random = new Random();
    static List<Thread> chefThreads = new List<Thread>(); // Track all threads
    static DateTime lastProgressTime = DateTime.Now; // Tracks the last time a chef made progress

    static void Main()
    {
        Console.WriteLine("🍕 Kitchen is open! 🍕");

        bool simulateDeadlock = true; // Toggle this to test deadlock or safe mode

        for (int i = 1; i <= 4; i++) // More chefs = higher chance of deadlock
        {
            int chefNumber = i;
            Thread chefThread = new Thread(() => ChefWork(chefNumber, simulateDeadlock));
            chefThreads.Add(chefThread);
            chefThread.Start();
        }

        // Run deadlock detection loop concurrently
        Thread detectionThread = new Thread(DetectDeadlock);
        detectionThread.Start();

        // Wait for all chefs to finish before exiting
        foreach (Thread chefThread in chefThreads)
        {
            chefThread.Join(); // Main thread waits for each chef thread to finish
        }

        Console.WriteLine("🍽️ All chefs have finished. The kitchen is closing.");
    }

    static void ChefWork(int chefNumber, bool simulateDeadlock)
    {
        Console.WriteLine($"👨‍🍳 Chef {chefNumber} is preparing...");
        Thread.Sleep(random.Next(1000, 3000)); // Simulate preparation time

        if (simulateDeadlock)
        {
            kitchen.DeadlockScenario(chefNumber);
        }
        else
        {
            int neededPepperoni = random.Next(1, 3);
            int neededMushrooms = random.Next(1, 4);
            int neededCheese = random.Next(1, 2);
            int neededPeppers = random.Next(1, 3);

            Console.WriteLine($"👨‍🍳 Chef {chefNumber} needs: {neededPepperoni} Pepperoni, {neededMushrooms} Mushrooms, " + 
                              $"{neededCheese} oz Cheese, and {neededPeppers} Peppers");

            // **Prevent deadlock: Always lock in the same order**
            while (!kitchen.TakeResources(neededPepperoni, neededMushrooms, neededCheese, neededPeppers))
            {
                Console.WriteLine($"👨‍🍳 Chef {chefNumber}: Not enough ingredients! Waiting... ⏳");
                Thread.Sleep(1000);
            }

            Console.WriteLine($"👨‍🍳 Chef {chefNumber} is now baking... 🍕");
            Thread.Sleep(random.Next(2000, 5000)); // Simulate baking time

            // ✅ Release the oven after baking
            kitchen.ReleaseOven();
            Console.WriteLine($"✅ Chef {chefNumber} has finished baking and left the kitchen.");
        }

        // Update progress to indicate that this chef made some progress
        lastProgressTime = DateTime.Now;
    }

    // **Deadlock Detection Mechanism**
    static void DetectDeadlock()
    {
        while (true)
        {
            Thread.Sleep(5000); // Check every 5 seconds

            // Check if there hasn't been progress for more than 10 seconds
            if ((DateTime.Now - lastProgressTime).TotalSeconds > 10)
            {
                Console.WriteLine("🚨 Deadlock detected due to lack of progress! Recovering...");
                RecoverFromDeadlock();
            }
        }
    }

    // **Deadlock Recovery: Release all locks**
    static void RecoverFromDeadlock()
    {
        Console.WriteLine("🔓 Releasing all locks to fix deadlock...");
        // Since we cannot forcefully unlock a mutex, we assume we restart threads
        foreach (Thread chefThread in chefThreads)
        {
            if (chefThread.IsAlive)
            {
                chefThread.Interrupt(); // Simulate breaking a deadlocked thread
            }
        }

        kitchen.ResetLocks();
        // Also update progress time so that the detection thread doesn't immediately trigger again
        lastProgressTime = DateTime.Now;
        Console.WriteLine("✅ Deadlock resolved! Chefs will retry.");
    }
}