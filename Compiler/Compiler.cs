using System;
using System.Collections.Generic;
using System.Linq;
using static Compiler.Constants;

namespace Compiler
{
    public static class Compiler
    {
        public static void Compile(string toCompile)
        {
            //TODO: Purge comments.
            
            // Split into lines of code, based on line termination character.
            string[] lines = toCompile.Split(new [] {LINE_TERMINATOR}, StringSplitOptions.None);

            foreach (string line in lines)
            {
                Console.WriteLine(Solve(line));
            }
        }

        private static bool IsGeneric(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }

        private static string SolveArithmetic(string p1, string p2, string op)
        {
            switch (op)
            {
                case ADDITION:
                    return (int.Parse(p1) + int.Parse(p2)).ToString();
                case SUBTRACTION:
                    return (int.Parse(p1) - int.Parse(p2)).ToString();
                case MULTIPLICATION:
                    return (int.Parse(p1) * int.Parse(p2)).ToString();
                case DIVISION:
                    return (int.Parse(p1) / int.Parse(p2)).ToString();
                case MODULO:
                    return (int.Parse(p1) % int.Parse(p2)).ToString();
                default:
                    Console.WriteLine("Error: Unknown operator \'" + op + "\'");
                    return NULL;
            }              
        }

        public static string Solve(string toSolve)
        {
            Console.WriteLine("Solving: " + toSolve);
            // Apply PEMDAS.
            // Solve for braces.
            if (toSolve.Contains(ORDER_CONTROL_OPEN))
            {
                int depth = 0;
                int start = 0;
                for (int i = 0; i < toSolve.Length; i++)
                {
                    if (toSolve[i].ToString() == ORDER_CONTROL_OPEN)
                    {
                        if (depth == 0)
                            start = i;
                        depth++;
                    }

                    if (toSolve[i].ToString() == ORDER_CONTROL_CLOSE)
                    {
                        depth--;
                        if (depth == 0)
                        {
                            string solvedPart = Solve(toSolve.Substring(start + 1, i - start - 1));
                            toSolve = toSolve.Remove(start, i - start + 1).Insert(start, solvedPart);
                        }
                    }
                }

                if (depth != 0)
                {
                    Console.WriteLine("Error: Uneven amount of parentheses.");
                    return NULL;
                    //TODO: Error handling.
                }
            }
            
            // Determine symbols.
            List<String> symbols = new List<string>();
            string currentSymbol = "";
            for (int i = 0; i < toSolve.Length; i++)
            {
                char c = toSolve[i];
                
                bool isWhitespace = char.IsWhiteSpace(c);
                bool isGeneric = char.IsLetterOrDigit(c) || c == '_';
                bool isEnclosure = ENCLOSURES.Contains(c.ToString());

                if ((isWhitespace || !isGeneric) && !string.IsNullOrEmpty(currentSymbol))
                {
                    if (!isWhitespace && !isEnclosure && !IsGeneric(currentSymbol.Last()))
                    {
                        currentSymbol += toSolve[i];
                        symbols.Add(currentSymbol);
                        currentSymbol = "";
                        continue;
                    }
                    symbols.Add(currentSymbol);
                    currentSymbol = "";

                    if (isEnclosure)
                    {
                        symbols.Add(c.ToString());
                    }
                    
                    if (c.ToString() == NOT && toSolve[i + 1].ToString() != EQUALS)
                    {
                        symbols.Add(c.ToString());
                        continue;
                    }
                    
                    if (c.ToString() == EQUALS && toSolve[i + 1].ToString() != EQUALS)
                    {
                        symbols.Add(c.ToString());
                    }
                }
                else if (!isWhitespace)
                {
                    if (!isGeneric && (i == toSolve.Length - 1 || IsGeneric(toSolve[i + 1]) || char.IsWhiteSpace(toSolve[i + 1]) || toSolve[i + 1].ToString() == NOT))
                    {
                        symbols.Add(c.ToString());
                    }
                    else
                    {
                        currentSymbol += c;
                    }
                }

            }

            symbols.Add(currentSymbol);
            foreach (string symbol in symbols)
            {
                Console.WriteLine(symbol);
            }
            Console.WriteLine("---");

            // Solve functions.

            // Solve multiplications.

            // Solve divisions.

            // Solve additions.

            // Solve subtractions.


            // Solve logic

            // Assign variables.
            return NULL;
        }
    }
}