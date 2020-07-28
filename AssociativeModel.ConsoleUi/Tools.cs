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
    }
}