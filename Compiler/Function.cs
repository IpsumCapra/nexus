using System;

namespace Compiler
{
    public class Function
    {
        private string name;
        private int args;
        private Func<string[], string> function;

        public Function(string name, int args, Func<string[], string> function)
        {
            this.name = name;
            this.args = args;
            this.function = function;
        }

        public string Name => name;

        public int Args => args;

        public Func<string[], string> Func => function;
    }
}