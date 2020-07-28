using System;

namespace AssociativeModel
{
    [Serializable]
    public class NetFile
    {
        public readonly string Name;

        public readonly string FullPath;

        public bool IsDirectory;

        public NetFile(string name, string fullPath=null, bool isDirectory=false)
        {
            Name = name;
            FullPath = fullPath;
            IsDirectory = isDirectory;
        }

        public override bool Equals(object obj) 
            => obj is NetFile f 
               && FullPath == f.FullPath
               && Name == f.Name;

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (FullPath != null ? FullPath.GetHashCode() : 0);
            }
        }

        public override string ToString() => Name;
    }
}