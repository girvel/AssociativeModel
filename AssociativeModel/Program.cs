using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociativeModel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var net = new Net<string>("root");
            var currentNode = net.Root;
            var selectedIndex = 0;
            var active = true;
            var history = new Stack<string>();
            
            while (active)
            {
                Console.Clear();
                Console.WriteLine($"--- {currentNode} ---\n");
                
                var i = 0;
                var dependencies = net.GetDependencies(currentNode);
                foreach (var node in dependencies)
                {
                    Console.ForegroundColor = i == selectedIndex ? ConsoleColor.White : ConsoleColor.Gray;
                    Console.WriteLine(node);
                    i++;
                }
                
                Console.ResetColor();

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0) selectedIndex--;
                        break;
                    
                    case ConsoleKey.DownArrow:
                        if (selectedIndex < dependencies.Length - 1) selectedIndex++;
                        break;
                    
                    case ConsoleKey.Enter:
                        if (!dependencies.Any()) break;
                        currentNode = dependencies[selectedIndex];
                        selectedIndex = 0;
                        break;
                    
                    case ConsoleKey.A:
                        Console.Write("Name: ");
                        net.AddDependency(currentNode, Console.ReadLine());
                        break;
                    
                    case ConsoleKey.Delete:
                        if (!dependencies.Any()) break;
                        
                        Console.Write($"Remove node {dependencies[selectedIndex]}? [Y/n]");
                        
                        if (Console.ReadKey().Key != ConsoleKey.N) 
                            net.RemoveDependency(currentNode, dependencies[selectedIndex]);
                        break;
                }
            }
        }
    }
}