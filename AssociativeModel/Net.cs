using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociativeModel
{
    public class Net<T>
    {
        private readonly Dictionary<T, List<T>> _dependencies = new Dictionary<T, List<T>>();
        
        public T Root { get; }

        public Net(T root)
        {
            Root = root;
            _dependencies[root] = new List<T>();
        }

        public void AddDependency(T first, T second)
        {
            var (parent, child) = _dependencies.ContainsKey(first) ? (first, second) : (second, first);
            
            if (!_dependencies.ContainsKey(parent))
            {
                throw new ArgumentException("Net must contain parent node.");
            }

            if (!_dependencies.ContainsKey(child))
            {
                _dependencies[child] = new List<T>();
            }

            if (!_dependencies[parent].Contains(child))
            {
                _dependencies[parent].Add(child);
            }

            if (!_dependencies[child].Contains(parent))
            {
                _dependencies[child].Add(parent);
            }
        }
        
        public T[] GetDependencies(T node)
        {
            return _dependencies[node].ToArray();
        }

        public void RemoveDependency(T first, T second)
        {
            if (!_dependencies.ContainsKey(first) || !_dependencies.ContainsKey(second))
            {
                throw new ArgumentException("Net must contain both nodes");
            }

            _dependencies[first].Remove(second);
            _dependencies[second].Remove(first);

            if (!_dependencies[first].Any() && !first.Equals(Root)) _dependencies.Remove(first);
            if (!_dependencies[second].Any() && !second.Equals(Root)) _dependencies.Remove(second);
        }
    }
}