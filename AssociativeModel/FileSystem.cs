using System;
using System.IO;
using System.Linq;

namespace AssociativeModel
{
    public class FileSystem
    {
        public Net<NetFile> Net;
        public NetFile Home;
        public NetFile CurrentFile;
        public string WorkingDirectory;

        public FileSystem(string workingDirectory)
        {
            Net = new Net<NetFile>(new NetFile("<root>"));
            Home = new NetFile("<home>");
            Net.Register(Home);
            
            WorkingDirectory = workingDirectory;

            void Update(string currentPath, NetFile currentFile)
            {
                foreach (var file in Directory.GetFiles(currentPath).Select(Path.GetFileName))
                {
                    var netFile = new NetFile(file, Path.Combine(currentPath, file));
                    Net.Register(netFile);
                    Net.AddAssociation(currentFile, netFile);
                }
                
                foreach (
                    var directory 
                    in Directory
                        .GetDirectories(currentPath)
                        .Select(Path.GetFileName))
                {
                    var netFile = new NetFile(directory, Path.Combine(currentPath, directory), true);
                    Net.Register(netFile);
                    Net.AddAssociation(currentFile, netFile);
                    
                    try
                    {
                        Update(Path.Combine(currentPath, directory), netFile);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        
                    }
                }
            }
            
            Update(WorkingDirectory, Home);
        }

        public NetFile GetByPath(string path)
        {
            var splitPath = path.Split('/').SelectMany(e => e.Split('\\')).ToArray();

            return splitPath.Skip(1).Aggregate(
                splitPath[0] switch
                {
                    "~" => Home,
                    "" => Net.Root,
                    _ => CurrentFile,
                },
                (current, name) => Net
                    .GetAssociations(current)
                    .First(f => f.Name == name));
        }
    }
}