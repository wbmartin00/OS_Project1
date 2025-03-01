using System;
using System.Threading;

class Kitchen
{
    private Mutex ingredientMutex = new Mutex(); // 🔒 Protects ingredient access
    private Mutex ovenMutex = new Mutex(); // 🔒 Protects oven access

    public int Pepperoni { get; private set; } = 100;
    public int Mushrooms { get; private set; } = 100;
    public int Cheese { get; private set; } = 100;
    public int Peppers { get; private set; } = 100;
    public int Ovens { get; private set; } = 2; // Limited number of ovens

    // **Safe Mode: Chefs always acquire locks in the same order to prevent deadlock**
    public bool TakeResources(int neededPepperoni, int neededMushrooms, int neededCheese, int neededPeppers)
    {
        // Lock both mutexes in a **fixed order** to prevent deadlock
        ingredientMutex.WaitOne();
        ovenMutex.WaitOne();

        try
        {
            if (Pepperoni >= neededPepperoni && Mushrooms >= neededMushrooms &&
                Cheese >= neededCheese && Peppers >= neededPeppers && Ovens > 0)
            {
                // Deduct ingredients
                Pepperoni -= neededPepperoni;
                Mushrooms -= neededMushrooms;
                Cheese -= neededCheese;
                Peppers -= neededPeppers;
                Ovens--;

                Console.WriteLine($"[Kitchen] Ingredients and oven taken! 🍕 Remaining: " +
                                  $"{Pepperoni} Pepperoni, {Mushrooms} Mushrooms, {Cheese} oz Cheese, {Peppers} Peppers. Ovens left: {Ovens}");

                return true;
            }
            else
            {
                Console.WriteLine("[Kitchen] Not enough ingredients or ovens available! 🚨");
                return false;
            }
        }
        finally
        {
            // Release resources
            ovenMutex.ReleaseMutex();
            ingredientMutex.ReleaseMutex();
        }
    }

    // **Deadlock Mode: Chefs acquire resources in opposite order**
    public void DeadlockScenario(int chefNumber)
    {
        if (chefNumber % 2 == 0)
        {
            // 🔒 Chef A: Lock ingredients first, then oven
            ingredientMutex.WaitOne();
            Console.WriteLine($"👨‍🍳 Chef {chefNumber} locked ingredients, waiting for an oven...");
            Thread.Sleep(500); // Simulate delay

            ovenMutex.WaitOne(); // 🔒 Deadlock risk!
            Console.WriteLine($"👨‍🍳 Chef {chefNumber} got both locks! Cooking...");

            Thread.Sleep(2000); // Simulate baking time

            ovenMutex.ReleaseMutex();
            ingredientMutex.ReleaseMutex();
        }
        else
        {
            // 🔒 Chef B: Lock oven first, then ingredients
            ovenMutex.WaitOne();
            Console.WriteLine($"👨‍🍳 Chef {chefNumber} locked an oven, waiting for ingredients...");
            Thread.Sleep(500);

            ingredientMutex.WaitOne(); // 🔒 Deadlock risk!
            Console.WriteLine($"👨‍🍳 Chef {chefNumber} got both locks! Cooking...");

            Thread.Sleep(2000); // Simulate baking time

            ingredientMutex.ReleaseMutex();
            ovenMutex.ReleaseMutex();
        }
    }

    public void ReleaseOven()
    {
        ovenMutex.WaitOne();
        try
        {
            Ovens++; // ✅ Return an oven to availability
            Console.WriteLine($"[Kitchen] Oven freed up! Available ovens: {Ovens}");
        }
        finally
        {
            ovenMutex.ReleaseMutex();
        }
    }

    public void ResetLocks()
    {
        Console.WriteLine("🔄 Resetting kitchen locks...");

        // **Try releasing both mutexes if locked**
        while (ingredientMutex.WaitOne(0))
        {
            ingredientMutex.ReleaseMutex();
        }
        while (ovenMutex.WaitOne(0))
        {
            ovenMutex.ReleaseMutex();
        }

        Console.WriteLine("✅ Kitchen locks have been reset.");
    }
}