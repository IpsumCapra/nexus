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
            float fp1 = float.Parse(p1);
            float fp2 = float.Parse(p2);
            switch (op)
            {
                case ADDITION:
                    return (fp1 + fp2).ToString();
                case SUBTRACTION:
                    return (fp1 - fp2).ToString();
                case MULTIPLICATION:
                    return (fp1 * fp2).ToString();
                case DIVISION:
                    return (fp1 / fp2).ToString();
                case MODULO:
                    return (fp1 % fp2).ToString();
                default:
                    Console.WriteLine("Error: Unknown operator \'" + op + "\'");
                    return NULL;
            }              
        }

        private static string SolveLogic(string p1, string p2, string op)
        {
            bool bp1 = bool.Parse(p1);
            bool bp2 = bool.Parse(p2);
            switch (op)
            {
                case EQUALS_TO:
                    return (bp1 == bp2).ToString().ToLower();
                case NOT_EQUALS_TO:
                    return (bp1 != bp2).ToString().ToLower();
                case AND:
                    return (bp1 && bp2).ToString().ToLower();
                case OR:
                    return (bp1 || bp2).ToString().ToLower();
                default:
                    Console.WriteLine("Error: Unknown logic operator \'" + op + "\'");
                    return NULL;
            }
        }

        private static string SolveComparisons(string p1, string p2, string op)
        {
            float bp1 = float.Parse(p1);
            float bp2 = float.Parse(p2);
            switch (op)
            {
                case EQUALS_TO:
                    return (bp1.Equals(bp2)).ToString().ToLower();
                case NOT_EQUALS_TO:
                    return (!bp1.Equals(bp2)).ToString().ToLower();
                case GREATER_THAN:
                    return (bp1 > bp2).ToString().ToLower();
                case GREATER_THAN_EQUAL_TO:
                    return (bp1 >= bp2).ToString().ToLower();
                case LESS_THAN:
                    return (bp1 < bp2).ToString().ToLower();
                case LESS_THAN_EQUAL_TO:
                    return (bp1 <= bp2).ToString().ToLower();
                default:
                    Console.WriteLine("Error: Unknown logic operator \'" + op + "\'");
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

            //TODO: Solve functions.

            // Solve multiplications, divisions, and modulo.
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
            // Binary functions
            for (int i = 1; i < symbols.Count; i++)
            {
                if (symbols[i] == MULTIPLICATION || symbols[i] == DIVISION || symbols[i] == MODULO)
                {
                    string result;
                    result = SolveArithmetic(symbols[i - 1], symbols[i + 1], symbols[i]);

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
                    string result;
                    result = SolveArithmetic(symbols[i - 1], symbols[i + 1], symbols[i]);
                    symbols.RemoveRange(i - 1, 3);
                    symbols.Insert(i - 1, result);
                    i -= 1;
                }
            }

            // Solve logic
            // Unary functions
            for (int i = 0; i < symbols.Count; i++)
            {
                if (symbols[i] == NOT)
                {
                    if (symbols[i + 1] == TRUE || symbols[i + 1] == FALSE)
                    {
                        symbols.RemoveAt(i);
                        symbols[i] = (!bool.Parse(symbols[i])).ToString().ToLower();
                    }
                }
            }

            // Binary comparison functions - numeric
            //TODO: String comparison.
            for (int i = 1; i < symbols.Count; i++)
            {
                if (BINARY_LOGIC.Contains(symbols[i]))
                {
                    bool isNumberComparison = char.IsDigit(symbols[i - 1][0]);
                    string result = NULL;

                    if (isNumberComparison)
                        result = SolveComparisons(symbols[i-1], symbols[i+1], symbols[i]);
                    else
                        continue;

                    symbols.RemoveRange(i - 1, 3);
                    symbols.Insert(i - 1, result);
                    i -= 1;
                }
            }
            
            // Binary logic comparison.
            for (int i = 1; i < symbols.Count; i++)
            {
                if (symbols[i] == EQUALS_TO || symbols[i] == NOT_EQUALS_TO)
                {
                    bool isNumberComparison = char.IsDigit(symbols[i - 1][0]);
                    string result = NULL;

                    if (!isNumberComparison)
                        result = SolveLogic(symbols[i-1], symbols[i+1], symbols[i]);
                    else
                        continue;

                    symbols.RemoveRange(i - 1, 3);
                    symbols.Insert(i - 1, result);
                    i -= 1;
                }
            }
            
            // AND and OR
            for (int i = 1; i < symbols.Count; i++)
            {
                if (symbols[i] == AND || symbols[i] == OR)
                {
                    bool isNumberComparison = char.IsDigit(symbols[i - 1][0]);
                    string result = NULL;
                    
                    result = SolveLogic(symbols[i-1], symbols[i+1], symbols[i]);
                    
                    symbols.RemoveRange(i - 1, 3);
                    symbols.Insert(i - 1, result);
                    i -= 1;
                }
            }
            
            //TODO: Assign variables.
            
            
            // Log symbols
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