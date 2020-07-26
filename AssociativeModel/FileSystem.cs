namespace AssociativeModel
{
    public class FileSystem
    {
        public Net<string> Net;
        public string StartingPoint;

        public FileSystem()
        {
            Net = new Net<string>("<root>");
            StartingPoint = "<home>";
            Net.Register(StartingPoint);
        }
    }
}