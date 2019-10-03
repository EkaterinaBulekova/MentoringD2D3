using System;
using System.Collections.Generic;
using System.Threading;
using Task1;
using Task2;
using Task3;
using Task4;
using Task5;
using Task6;
using Task7;
using TasksInterface;

namespace MultethreadingTasks
{
    class TasksStarter
    { 
        static void Main(string[] args)
        {
            GetTasks().ForEach(_ => {StartNewTask(0, _); });

            Console.WriteLine("All mulithreading tasks finished");
            Console.ReadKey();
        }

        static List<IStartable> GetTasks()
        {
            var tasks = new List<IStartable>{
                new Iterators(100),
                new ArrayProcessor(10),
                new MatrixMultiplier(),
                new RecursiveTreadsCreator(10),
                new RecursiveTreadPoolCreator(10),
                new PrintableCollectionCreator(10),
                new ParentContinuation()
            };

            return tasks;
        } 

        private static void StartNewTask(int number, IStartable testClass)
        {
            Console.WriteLine($"Start Multithreading Task{number} \n");
            testClass.Start();
            Console.WriteLine("Press key to continue");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}