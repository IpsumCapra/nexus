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
                Console.WriteLine("Result: " + Solve(line));
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
                    return (float.Parse(p1) + float.Parse(p2)).ToString();
                case SUBTRACTION:
                    return (float.Parse(p1) - float.Parse(p2)).ToString();
                case MULTIPLICATION:
                    return (float.Parse(p1) * float.Parse(p2)).ToString();
                case DIVISION:
                    return (float.Parse(p1) / float.Parse(p2)).ToString();
                case MODULO:
                    return (float.Parse(p1) % float.Parse(p2)).ToString();
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
                            i = start;
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
                        continue;
                    }

                    if (c.ToString() == NOT && toSolve[i + 1].ToString() != EQUALS)
                    {
                        symbols.Add(c.ToString());
                        continue;
                    }

                    if (c.ToString() == EQUALS && toSolve[i + 1].ToString() != EQUALS)
                    {
                        symbols.Add(c.ToString());
                        continue;
                    }

                    if (OPERATORS.Contains(c.ToString()) && toSolve[i + 1].ToString() != EQUALS)
                    {
                        symbols.Add(c.ToString());
                        continue;
                    }
                }

                if (!isWhitespace)
                {
                    if (!isGeneric && (i == toSolve.Length - 1 || IsGeneric(toSolve[i + 1]) ||
                                       char.IsWhiteSpace(toSolve[i + 1]) || toSolve[i + 1].ToString() == NOT))
                    {
                        symbols.Add(c.ToString());
                    }
                    else
                    {
                        currentSymbol += c;
                    }
                }

            }

            if (!string.IsNullOrEmpty(currentSymbol))
                symbols.Add(currentSymbol);

            // Solve functions.
            
            // Unary functions
            for (int i = 0; i < symbols.Count; i++)
            {
                if (symbols[i] == SUBTRACTION)
                {
                    if (i == 0 || !char.IsLetterOrDigit(symbols[i - 1][0]))
                    {
                        symbols.RemoveAt(i);
                        symbols[i] = SUBTRACTION + symbols[i];
                    } 
                }
            }

            // Solve multiplications, divisions, and modulo.
            for (int i = 1; i < symbols.Count; i++)
            {
                if (symbols[i] == MULTIPLICATION || symbols[i] == DIVISION || symbols[i] == MODULO)
                {
                    string result = NULL;
                    switch (symbols[i])
                    {
                        case MULTIPLICATION:
                            result = SolveArithmetic(symbols[i - 1], symbols[i + 1], MULTIPLICATION);
                            break;
                        case DIVISION:
                            result = SolveArithmetic(symbols[i - 1], symbols[i + 1], DIVISION);
                            break;
                        case MODULO:
                            result = SolveArithmetic(symbols[i - 1], symbols[i + 1], MODULO);
                            break;
                    }

                    symbols.RemoveRange(i - 1, 3);
                    symbols.Insert(i - 1, result);
                    i -= 1;
                }
            }

            // Solve additions and subtractions.
            for (int i = 1; i < symbols.Count; i++)
            {
                if (symbols[i] == ADDITION || symbols[i] == SUBTRACTION)
                {
                    string result = NULL;
                    switch (symbols[i])
                    {
                        case ADDITION:
                            result = SolveArithmetic(symbols[i - 1], symbols[i + 1], ADDITION);
                            break;
                        case SUBTRACTION:
                            result = SolveArithmetic(symbols[i - 1], symbols[i + 1], SUBTRACTION);
                            break;
                    }

                    symbols.RemoveRange(i - 1, 3);
                    symbols.Insert(i - 1, result);
                    i -= 1;
                }
            }

            // Solve logic

            // Assign variables.

            foreach (string symbol in symbols)
            {
                Console.WriteLine(symbol);
            }
            Console.WriteLine("---");
            
            if (symbols.Count > 1)
            {
                if (symbols.Count == 2 && symbols[0] == SUBTRACTION && float.TryParse(symbols[1], out var result))
                {
                    return (-result).ToString();
                }
                Console.WriteLine("Error: Computation error.");
                return NULL;
            }
            return symbols[0];
        }
    }
}