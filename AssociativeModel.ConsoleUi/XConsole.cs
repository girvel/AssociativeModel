using System;
using System.Linq;
using Old = System.Console;

namespace AssociativeModel.ConsoleUi
{
    public static class XConsole
    {
        public delegate string[] Suggestion(string currentInput);
        
        public static string ReadLine(string request = "", Suggestion getSuggestions = null)
        {
            getSuggestions ??= (inp => new string[0]);

            Old.Write(request);

            ListSelector<string> suggestions = new string[0].ToSelector();
            var result = "";
            char? lastChar;
            
            while (true)
            {
                var key = Old.ReadKey(true);

                void Update()
                {
                    Old.Write(lastChar?.ToString() ?? "");

                    string[] newSuggestionsArray;
                    if (suggestions.Get() == null || !suggestions.Get().StartsWith(result))
                    {
                        newSuggestionsArray =
                            result != ""
                                ? getSuggestions(result).Where(s => s.StartsWith(result)).ToArray()
                                : new string[0];
                    }
                    else
                    {
                        newSuggestionsArray = (string[]) suggestions.List;
                    }
                    
                    var (x, y) = (Old.CursorLeft, Old.CursorTop);
                    var oldColor = Old.ForegroundColor;
                            
                    Old.ForegroundColor = ConsoleColor.DarkGray;

                    var cleaningLength = (suggestions.Get() ?? "").Length 
                                         - (newSuggestionsArray.FirstOrDefault() ?? "").Length;
                    suggestions = newSuggestionsArray.ToSelector();
                    
                    if (suggestions.List.Any())
                        Old.Write(suggestions.Get().Substring(Math.Min(result.Length, suggestions.Get().Length)));
                    Old.Write(new string(' ', Math.Max(0, cleaningLength)));
                            
                    (Old.CursorLeft, Old.CursorTop) = (x, y);
                    Old.ForegroundColor = oldColor;
                }

                lastChar = null;
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        Old.WriteLine();
                        return result;
                    
                    case ConsoleKey.Backspace:
                        if (result.Length == 0) break;
                        
                        result = result.Substring(0, result.Length - 1);
                        
                        Old.CursorLeft--;
                        Old.Write(' ');
                        Old.CursorLeft--;
                        
                        break;
                    
                    case ConsoleKey.Escape:
                        getSuggestions = inp => new string[0];
                        suggestions = new string[0].ToSelector();
                        break;
                    
                    case ConsoleKey.Tab:
                        if (!suggestions.List.Any()) break;
                        
                        Old.Write(suggestions.Get().Substring(result.Length));
                        result = suggestions.Get();
                        break;
                    
                    case ConsoleKey.DownArrow:
                        suggestions.StepForward();
                        break;
                        
                    case ConsoleKey.UpArrow:
                        suggestions.StepBack();
                        break;
                    
                    default:
                        result += lastChar = key.KeyChar;
                        break;
                }
                
                Update();
            }
        }

        public static string ReadLine(string request, string[] suggestions) 
            => ReadLine(request, inp => suggestions ?? new string[0]);
    }
}