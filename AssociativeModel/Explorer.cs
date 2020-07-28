using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AssociativeModel.ConsoleUi;
using OConsole = System.Console;

namespace AssociativeModel
{
    public class Explorer
    {
        public FileSystem FileSystem { get; }
        
        public ListSelector<NetFile> Selector { get; private set; }

        protected readonly Dictionary<ConsoleKey, Action> UiActions;



        public Explorer(FileSystem fileSystem)
        {
            FileSystem = fileSystem;
            FileSystem.CurrentFile = FileSystem.Home ?? FileSystem.Net.Root;
            Selector = new ListSelector<NetFile>();

            UiActions = new Dictionary<ConsoleKey, Action>
            {
                [ConsoleKey.UpArrow] = () => Selector.StepBack(),
                [ConsoleKey.DownArrow] = () => Selector.StepForward(),
                [ConsoleKey.Enter] = () =>
                {
                    if (!FileSystem.Net.GetAssociations(FileSystem.CurrentFile).Any()) return;
                    FileSystem.CurrentFile = Selector.Get();
                    Selector = FileSystem.Net.GetAssociations(FileSystem.CurrentFile).ToSelector();
                },
                [ConsoleKey.A] = () =>
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
                },
                [ConsoleKey.Delete] = () =>
                {
                    if (!Selector.List.Any()) return;

                    var removed = Selector.Get();

                    OConsole.Write($"Remove association with {removed}? [Y/n]");

                    if (OConsole.ReadKey().Key != ConsoleKey.N)
                        FileSystem.Net.RemoveAssociation(
                            FileSystem.CurrentFile,
                            Selector.Get());

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
                OConsole.Clear();

                Selector.List = FileSystem.Net.GetAssociations(FileSystem.CurrentFile);
                DisplayCurrentNode();

                if (UiActions.TryGetValue(OConsole.ReadKey(true).Key, out var action)) action();
            }
        }

        public void DisplayCurrentNode()
        {
            OConsole.ForegroundColor = ConsoleColor.Black;
            OConsole.BackgroundColor = ConsoleColor.Gray;
            OConsole.WriteLine($" --- {FileSystem.CurrentFile} --- ");
            
            OConsole.ResetColor();
            
            var i = 0;
            foreach (var node in FileSystem.Net.GetAssociations(FileSystem.CurrentFile))
            {
                if (node.FullPath == null)
                {
                    OConsole.ForegroundColor = ConsoleColor.DarkGray;
                }
                else if (node.IsDirectory)
                {
                    OConsole.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    OConsole.ForegroundColor = ConsoleColor.White;
                }
                
                OConsole.Write(i == Selector.Index ? " > ": "   ");
                OConsole.WriteLine(node);
                i++;
            }
            
            OConsole.ResetColor();
        }
    }
}