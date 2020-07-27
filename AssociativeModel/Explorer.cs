using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AssociativeModel
{
    public class Explorer
    {
        public FileSystem FileSystem { get; }
        
        public int SelectionIndex { get; private set; }

        protected readonly Dictionary<ConsoleKey, Action> UiActions;



        public Explorer(FileSystem fileSystem)
        {
            FileSystem = fileSystem;
            FileSystem.CurrentFile = FileSystem.Home ?? FileSystem.Net.Root;
            SelectionIndex = 0;

            UiActions = new Dictionary<ConsoleKey, Action>
            {
                [ConsoleKey.UpArrow] = () =>
                {
                    if (SelectionIndex > 0) SelectionIndex--;
                },
                [ConsoleKey.DownArrow] = () =>
                {
                    if (SelectionIndex < FileSystem.Net.GetAssociations(FileSystem.CurrentFile).Length - 1)
                        SelectionIndex++;
                },
                [ConsoleKey.Enter] = () =>
                {
                    if (!FileSystem.Net.GetAssociations(FileSystem.CurrentFile).Any()) return;
                    FileSystem.CurrentFile = FileSystem.Net.GetAssociations(FileSystem.CurrentFile)[SelectionIndex];
                    SelectionIndex = 0;
                },
                [ConsoleKey.A] = () =>
                {
                    Console.Write("Path: ");
                    var node = FileSystem.GetByPath(Console.ReadLine());

                    if (FileSystem.CurrentFile != FileSystem.Net.Root)
                        FileSystem.Net.AddAssociation(FileSystem.CurrentFile, node);
                },
                [ConsoleKey.Delete] = () =>
                {
                    if (!FileSystem.Net.GetAssociations(FileSystem.CurrentFile).Any()) return;

                    var removed = FileSystem.Net.GetAssociations(FileSystem.CurrentFile)[SelectionIndex];

                    Console.Write($"Remove association with {removed}? [Y/n]");

                    if (Console.ReadKey().Key != ConsoleKey.N)
                        FileSystem.Net.RemoveAssociation(
                            FileSystem.CurrentFile,
                            FileSystem.Net.GetAssociations(FileSystem.CurrentFile)[SelectionIndex]);

                    if (removed == FileSystem.Net.Root) FileSystem.CurrentFile = FileSystem.Net.Root;
                },
                [ConsoleKey.E] = () =>
                {
                    if (FileSystem.CurrentFile.FullPath != null)
                        Process.Start(FileSystem.CurrentFile.FullPath);
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
            Console.WriteLine($" --- {FileSystem.CurrentFile} --- ");
            
            Console.ResetColor();
            
            var i = 0;
            foreach (var node in FileSystem.Net.GetAssociations(FileSystem.CurrentFile))
            {
                if (node.FullPath == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else if (node.IsDirectory)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
                Console.Write(i == SelectionIndex ? " > ": "   ");
                Console.WriteLine(node);
                i++;
            }
            
            Console.ResetColor();
        }
    }
}