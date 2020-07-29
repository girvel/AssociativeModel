using System;
using System.Linq;
using System.Reflection;

namespace AssociativeModel.ConsoleUi
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class UiAction : Attribute
    {
        public UiAction(ConsoleKey key, ConsoleModifiers modifiers = 0)
        {
            Pattern = new KeyPattern(key, modifiers);
        }

        public KeyPattern Pattern { get; }



        public static (KeyPattern[] patterns, Action a)[] ReflectMethods(object owner)
        {
            return owner
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Select<MethodInfo, (KeyPattern[] patterns, Action a)>(
                    m => (
                        patterns: m.GetCustomAttributes<UiAction>().Select(a => a.Pattern).ToArray(), 
                        a: () => m.Invoke(owner, new object[0])))
                .Where(tuple => tuple.patterns.Any())
                .ToArray();
        }
    }
}