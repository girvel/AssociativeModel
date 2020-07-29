using System;

namespace AssociativeModel.ConsoleUi
{
    public readonly struct KeyPattern
    {
        public readonly ConsoleKey Key;
        public readonly ConsoleModifiers Modifiers;

        public KeyPattern(ConsoleKey key, ConsoleModifiers modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public bool Match(ConsoleKeyInfo input) => Key == input.Key && Modifiers == input.Modifiers;
        
        public static implicit operator KeyPattern(ConsoleKey key)
        {
            return new KeyPattern(key, 0);
        }
    }
}