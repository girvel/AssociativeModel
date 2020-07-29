using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AssociativeModel.ConsoleUi;

namespace AssociativeModel
{
    public class Explorer
    {
        public FileSystem FileSystem { get; }
        
        public ListSelector<NetFile> Selector { get; private set; }
        
        protected readonly (KeyPattern[] patterns, Action action)[] UiActions;



        public Explorer(FileSystem fileSystem)
        {
            FileSystem = fileSystem;
            FileSystem.CurrentFile = FileSystem.Home ?? FileSystem.Net.Root;
            Selector = new ListSelector<NetFile>();

            UiActions = UiAction.ReflectMethods(this);
        }



        public void Start()
        {
            while (true)
            {
                Console.Clear();

                Selector.List = FileSystem.Net.GetAssociations(FileSystem.CurrentFile);
                DisplayCurrentNode();

                UiActions.Get(Console.ReadKey())?.Invoke();
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
                
                Console.Write(i == Selector.Index ? " > ": "   ");
                Console.WriteLine(node);
                i++;
            }
            
            Console.ResetColor();
        }


        
        [UiAction(ConsoleKey.UpArrow)] 
        [UiAction(ConsoleKey.W, ConsoleModifiers.Alt)]
        private void CursorUp() => Selector.StepBack();

        [UiAction(ConsoleKey.DownArrow)]
        [UiAction(ConsoleKey.S, ConsoleModifiers.Alt)]
        private void CursorDown() => Selector.StepForward();

        [UiAction(ConsoleKey.Enter)]
        [UiAction(ConsoleKey.D, ConsoleModifiers.Alt)]
        private void OpenAssociations() 
        {
            if (!Selector.List.Any()) return;
            
            FileSystem.CurrentFile = Selector.Get();
            Selector = FileSystem.Net.GetAssociations(FileSystem.CurrentFile).ToSelector();
        }

        [UiAction(ConsoleKey.A, ConsoleModifiers.Control)]
        private void AddAssociations()
        {
            var node = FileSystem.GetByPath(
                XConsole.ReadLine(
                    "Path: ",
                    inp =>
                    {
                        var fullPath = Tools.FullPathToDirectory(inp);
                        return FileSystem.Net.GetAssociations(
                                FileSystem.GetByPath(fullPath))
                            .Select(a => fullPath + a.Name)
                            .OrderBy(s => s)
                            .ToArray();
                    }));
            
            if (FileSystem.CurrentFile != FileSystem.Net.Root)
                FileSystem.Net.AddAssociation(FileSystem.CurrentFile, node);
        }

        [UiAction(ConsoleKey.Delete)]
        private void DeleteAssociation()
        {
            if (!Selector.List.Any()) return;
            
            var removed = Selector.Get();

            Console.Write($"Remove association with {removed}? [Y/n]");

            if (Console.ReadKey().Key != ConsoleKey.N)
                FileSystem.Net.RemoveAssociation(
                    FileSystem.CurrentFile,
                    Selector.Get());

            if (removed.Equals(FileSystem.Net.Root)) FileSystem.CurrentFile = FileSystem.Net.Root;
        }

        [UiAction(ConsoleKey.E, ConsoleModifiers.Control)]
        private void Execute()
        {
            if (FileSystem.CurrentFile.FullPath != null)
                Process.Start(FileSystem.CurrentFile.FullPath);
        }
    }
}