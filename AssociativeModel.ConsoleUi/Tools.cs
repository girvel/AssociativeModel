using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociativeModel.ConsoleUi
{
    public static class Tools
    {
        public static string FullPathToDirectory(string path, char[] separators = null)
        {
            separators ??= new[] {'/', '\\'};
            
            var splitPath = separators
                .Aggregate(
                    new[] {path}.AsEnumerable(),
                    (cur, sep) => cur.SelectMany(e => e.Split(sep)))
                .ToArray();

            return splitPath
                .Take(splitPath.Length - 1)
                .Aggregate("", (cur, e) => cur + e + "/");
        }

        public static T Get<T>(this IEnumerable<(KeyPattern[] patterns, T result)> collection, ConsoleKeyInfo info)
            => collection.FirstOrDefault(t => t.patterns.Any(p => p.Match(info))).result;

        public static string CustomToString(this ConsoleModifiers modifiers)
        {
            var result = new List<string>();
            
            if (modifiers.HasFlag(ConsoleModifiers.Control)) result.Add("Ctrl");
            if (modifiers.HasFlag(ConsoleModifiers.Shift)) result.Add("Shift");
            if (modifiers.HasFlag(ConsoleModifiers.Alt)) result.Add("Alt");

            return result.Aggregate("", (cur, e) => cur + " + " + e).Substring(3);
        }
    }
}