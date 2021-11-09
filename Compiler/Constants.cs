namespace Compiler
{
    public static class Constants
    {
        // Order Control
        public const string LINE_TERMINATOR = ";";
        public const string ORDER_CONTROL_OPEN = "(";
        public const string ORDER_CONTROL_CLOSE = ")";
        public const string DEFINITION_OPEN = "{";
        public const string DEFINITION_CLOSE = "}";
        
        // Comments
        public const string COMMENT = "//";
        public const string INLINE_COMMENT_OPEN = "/*";
        public const string INLINE_COMMENT_CLOSE = "*/";
        
        // Two Input Arithmetic Operators
        public const string ADDITION = "+";
        public const string SUBTRACTION = "-";
        public const string MULTIPLICATION = "*";
        public const string DIVISION = "/";
        public const string MODULO = "%";
        
        // One Input Arithmetic Operators
        public const string AUTO_INCREMENT = "++";
        public const string AUTO_DECREMENT = "--";
        
        // Assignment Operators
        public const string EQUALS = "=";
        public const string EQUALS_ADDITION = "+=";
        public const string EQUALS_SUBTRACTION = "-=";
        public const string EQUALS_MULTIPLICATION = "*=";
        public const string EQUALS_DIVISION = "/=";
        public const string EQUALS_MODULO = "%=";
        
        // Two Input Logic Operators
        public const string EQUALS_TO = "==";
        public const string NOT_EQUALS_TO = "!=";
        public const string GREATER_THAN = ">";
        public const string GREATER_THAN_EQUAL_TO = ">=";
        public const string LESS_THAN = "<";
        public const string LESS_THAN_EQUAL_TO = "<=";
        public const string AND = "&&";
        public const string OR = "||";
        
        // One Input Logic Operators
        public const string NOT = "!";
        
        // Variables
        public const string LIST_OPEN = "[";
        public const string LIST_CLOSE = "]";
        
        // Constants
        public const string NULL = "null";
        public const string TRUE = "true";
        public const string FALSE = "false";
        
        // Misc
        public static string[] ENCLOSURES = {DEFINITION_OPEN, DEFINITION_CLOSE, LIST_OPEN, LIST_CLOSE};
        public static string[] OPERATORS = {ADDITION, SUBTRACTION, MULTIPLICATION, DIVISION, MODULO};

        public static string[] BINARY_LOGIC = {EQUALS_TO, NOT_EQUALS_TO, GREATER_THAN, GREATER_THAN_EQUAL_TO, LESS_THAN, LESS_THAN_EQUAL_TO};
    }
}