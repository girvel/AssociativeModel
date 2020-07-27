using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AssociativeModel
{
    public class Net<T> : IEnumerable<T>
    {
        protected readonly Dictionary<T, List<T>> Associations = new Dictionary<T, List<T>>();
        
        public T Root { get; }
        
        public Net(T root)
        {
            Root = root;
            Associations[Root] = new List<T>();
        }



        public bool Contains(T node) => Associations.ContainsKey(node);
        
        public T Register(T node)
        {
            Debug.Assert(!Associations.ContainsKey(node), "this node has already been registered");
            
            Associations[node] = new List<T> {Root};
            Associations[Root].Add(node);

            return node;
        }

        public void Unregister(T node)
        {
            foreach (var a in Associations[node])
            {
                Associations[a].Remove(node);
            }

            Associations.Remove(node);
        }

        public void AddAssociation(T first, T second)
        {
            Debug.Assert(!first.Equals(second), "first must not be equal second");
            Debug.Assert(Associations.ContainsKey(first), "first must be registered");
            Debug.Assert(Associations.ContainsKey(second), "second must be registered");
            
            Associations[first].Add(second);
            Associations[second].Add(first);
        }

        public bool RemoveAssociation(T first, T second)
        {
            Debug.Assert(Associations.ContainsKey(first), "first must be registered");
            Debug.Assert(Associations.ContainsKey(second), "second must be registered");
            Debug.Assert(!first.Equals(second), "first must not be equal second");

            if (first.Equals(Root))
            {
                Unregister(second);
                return true;
            }

            if (second.Equals(Root))
            {
                Unregister(first);
                return true;
            }
            
            if (!Associations[first].Contains(second)) return false;

            Associations[first].Remove(second);
            Associations[second].Remove(first);

            return true;
        }

        public T[] GetAssociations(T node)
        {
            Debug.Assert(Associations.ContainsKey(node), "node must be registered");

            return Associations[node].ToArray();
        }
        
        
        
        #region IEnumerable<T> implementation

        public IEnumerator<T> GetEnumerator()
        {
            return Associations.Keys.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}