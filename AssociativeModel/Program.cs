using System;
using System.IO;
using System.Linq;
using AssociativeModel.ConsoleUi;

namespace AssociativeModel
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var explorer = new Explorer(
                new FileSystem(
                    XConsole.ReadLine(
                        "Working directory: ", 
                        inp =>
                        {
                            try
                            {
                                return Directory.GetDirectories(Tools.FullPathToDirectory(inp));
                            }
                            catch (DirectoryNotFoundException)
                            {
                            }
                            catch (ArgumentException)
                            {
                            }

                            return new string[0];
                        })));

            explorer.Start();
        }
    }
}