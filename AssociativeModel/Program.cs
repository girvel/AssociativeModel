using System;
using System.Linq;

namespace AssociativeModel
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var net = new Net<string>("root");
            var currentNode = net.Root;
            var selectedIndex = 0;
            var active = true;
            
            while (active)
            {
                Console.Clear();
                Console.WriteLine($"--- {currentNode} ---\n");
                
                var i = 0;
                var dependencies = net.GetAssociations(currentNode);
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
                        var node = Console.ReadLine();
                        
                        net.Register(node);
                        
                        if (currentNode != net.Root)
                            net.AddAssociation(currentNode, node);
                        break;
                    
                    case ConsoleKey.Delete:
                        if (!dependencies.Any()) break;

                        var removed = dependencies[selectedIndex];
                        
                        Console.Write($"Remove association with {removed}? [Y/n]");
                        
                        if (Console.ReadKey().Key != ConsoleKey.N) 
                            net.RemoveAssociation(currentNode, dependencies[selectedIndex]);

                        if (removed == net.Root) currentNode = net.Root;
                        break;
                }
            }
        }
    }
}