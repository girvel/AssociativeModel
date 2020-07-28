using System;
using System.Collections.Generic;

namespace AssociativeModel.ConsoleUi
{
    public class ListSelector<T>
    {
        public IList<T> List;
        public int Index = 0;

        public ListSelector()
        {
        }

        public ListSelector(IList<T> list)
        {
            List = list;
        }

        public void StepForward() => Index = Math.Min(Index + 1, List.Count - 1);
        
        public void StepBack() => Index = Math.Max(Index - 1, 0);

        public T Get() => Index < List.Count ? List[Index] : default;
    }
}