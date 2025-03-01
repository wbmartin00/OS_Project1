# Multi-threaded Kitchen Simulation and IPC Project

This repository contains two projects designed to demonstrate key concepts in multi-threading, deadlock detection, and inter-process communication (IPC) using C#.

---

## Overview

- **Project A: Multi-threaded Kitchen Simulation**  
  Simulates a kitchen environment where multiple chef threads compete for shared resources (ingredients and ovens). It includes:
  - Thread management using C# threads
  - Synchronization using mutexes
  - A deadlock detection and recovery mechanism (timing-based approach)

- **Project B: IPC Using Pipes**  
  Demonstrates inter-process communication using pipes:
  - A **Producer** process writes sample data (strings) to standard output
  - A **Consumer** process reads the data from standard input, processes it (e.g., converting text to uppercase), and outputs the result

---

## Repository Structure
├── ProjectA
│   ├── Program.cs          # Main entry point for the kitchen simulation
│   ├── Kitchen.cs          # Handles shared resources and thread management
│   └── … (additional source files)
├── ProjectB
│   ├── Producer.cs         # Producer process for IPC via pipes
│   ├── Consumer.cs         # Consumer process that reads and processes input
│   └── large file test.txt
├── Report
│   ├── Report.tex          # LaTeX source for the final project report
│   └── Report.pdf          # Compiled report in PDF format
└── README.md               # Documentation and project instructions

---

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 5.0 or later recommended)  
- A LaTeX distribution (e.g., TeX Live, MiKTeX) if you wish to compile the report  
- **Note for ARM Users:** If running on an ARM-based system (e.g., MacBook M2 Pro), you may need additional steps or configurations for VS Code and .NET.

---

## Installation

1. **Clone the Repository:**

    git clone https://github.com/yourusername/yourrepository.git
    cd yourrepository

2. **Build and Run the Projects:**

   - **Project A (Kitchen Simulation)**  

         cd ProjectA
         dotnet run

   - **Project B (IPC Using Pipes)**  

         cd ../ProjectB
         dotnet run

---

## Usage

- **Project A**  
  Run the simulation to see how multiple chef threads interact with shared resources. The program can simulate both a safe scenario (fixed locking order) and a deadlock scenario. If a deadlock is detected (no progress for 10 seconds), the system triggers recovery.

  Set the bool variable simulateDeadlock (line 16 of Pizza Shop/Program.cs) to "True" if you want to simulate the deadlock. Set to "false" to enable safe mode.  

- **Project B**  
  The producer writes data to standard output. The consumer reads from standard input, processes the text (e.g., converting it to uppercase), and outputs the result.

Set the bool variable sendLargeFile (line 10 of the Producer/Program.cs file) to "True" if you want to test a larger file over the IPC pipe connection. 
---

## Challenges and Solutions

- **Environment Setup**  
  Installing Ubuntu on a VM (Parallels) on an ARM-based MacBook M2 Pro was challenging. There were issues with VS Code and .NET on ARM, which I resolved with help from ChatGPT.

- **Deadlock Detection**  
  Initially, I tried using thread state analysis to detect deadlocks, but the states were too dynamic. I switched to a timing-based approach: if no progress occurs within 10 seconds, the system assumes a deadlock and triggers recovery.

---

## Additional Information

- The LaTeX-based report (implementation details, challenges, and learning outcomes) is in the `Report` directory.  
- Contributions, feedback, and suggestions are welcome!

---

## License

KSU Student? idk.
