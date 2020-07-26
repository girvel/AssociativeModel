using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociativeModel
{
    public class Explorer
    {
        public FileSystem FileSystem { get; }
        
        public string CurrentNode { get; private set; }
        public int SelectionIndex { get; private set; }

        protected readonly Dictionary<ConsoleKey, Action> UiActions;



        public Explorer(FileSystem fileSystem)
        {
            FileSystem = fileSystem;
            CurrentNode = FileSystem.StartingPoint ?? FileSystem.Net.Root;
            SelectionIndex = 0;
            
            UiActions = new Dictionary<ConsoleKey, Action>
            {
                [ConsoleKey.UpArrow] = () => {
                    if (SelectionIndex > 0) SelectionIndex--;
                },
                [ConsoleKey.DownArrow] = () => {
                    if (SelectionIndex < FileSystem.Net.GetAssociations(CurrentNode).Length - 1) SelectionIndex++;
                },
                [ConsoleKey.Enter] = () => {
                    if (!FileSystem.Net.GetAssociations(CurrentNode).Any()) return;
                    CurrentNode = FileSystem.Net.GetAssociations(CurrentNode)[SelectionIndex];
                    SelectionIndex = 0;
                },
                [ConsoleKey.A] = () => {
                    Console.Write("Name: ");
                    var node = Console.ReadLine();
                    
                    if (!FileSystem.Net.Contains(node))
                        FileSystem.Net.Register(node);
                        
                    if (CurrentNode != FileSystem.Net.Root)
                        FileSystem.Net.AddAssociation(CurrentNode, node);
                },
                [ConsoleKey.Delete] = () => {
                    if (!FileSystem.Net.GetAssociations(CurrentNode).Any()) return;

                    var removed = FileSystem.Net.GetAssociations(CurrentNode)[SelectionIndex];
                        
                    Console.Write($"Remove association with {removed}? [Y/n]");
                        
                    if (Console.ReadKey().Key != ConsoleKey.N) 
                        FileSystem.Net.RemoveAssociation(
                            CurrentNode, 
                            FileSystem.Net.GetAssociations(CurrentNode)[SelectionIndex]);

                    if (removed == FileSystem.Net.Root) CurrentNode = FileSystem.Net.Root;
                },
            };
        }



        public void Start()
        {
            while (true)
            {
                Console.Clear();
                DisplayCurrentNode();

                if (UiActions.TryGetValue(Console.ReadKey(true).Key, out var action)) action();
            }
        }

        public void DisplayCurrentNode()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine($" --- {CurrentNode} --- ");
            
            Console.ResetColor();
            
            var i = 0;
            foreach (var node in FileSystem.Net.GetAssociations(CurrentNode))
            {
                Console.Write(i == SelectionIndex ? " > ": "   ");
                Console.WriteLine(node);
                i++;
            }
            
            Console.ResetColor();
        }
    }
}