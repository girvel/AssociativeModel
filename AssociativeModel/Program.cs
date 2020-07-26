using System;
using System.Linq;

namespace AssociativeModel
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var explorer = new Explorer(new FileSystem());

            explorer.Start();
        }
    }
}