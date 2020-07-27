using System;
using System.Linq;

namespace AssociativeModel
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Working directory: ");
            var explorer = new Explorer(new FileSystem(Console.ReadLine()));

            explorer.Start();
        }
    }
}