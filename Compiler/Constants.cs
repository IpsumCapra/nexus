namespace Compiler
{
    public static class Constants
    {
        // Order Control
        public static string LINE_TERMINATOR = ";";
        public static string ORDER_CONTROL_OPEN = "(";
        public static string ORDER_CONTROL_CLOSE = ")";
        public static string DEFINITION_OPEN = "{";
        public static string DEFINITION_CLOSE = "}";
        
        // Comments
        public static string COMMENT = "//";
        
        // Two Input Arithmetic Operators
        public static string ADDITION = "+";
        public static string SUBTRACTION = "-";
        public static string MULTIPLICATION = "*";
        public static string DIVISION = "/";
        public static string MODULO = "%";
        
        // One Input Arithmetic Operators
        public static string AUTO_INCREMENT = "++";
        public static string AUTO_DECREMENT = "--";
        
        // Assignment Operators
        public static string EQUALS = "=";
        public static string EQUALS_ADDITION = "+=";
        public static string EQUALS_SUBTRACTION = "-=";
        public static string EQUALS_MULTIPLICATION = "*=";
        public static string EQUALS_DIVISION = "/=";
        public static string EQUALS_MODULO = "%=";
        
        // Two Input Logic Operators
        public static string EQUALS_TO = "==";
        public static string NOT_EQUALS_TO = "!=";
        public static string GREATER_THAN = ">";
        public static string GREATER_THAN_EQUAL_TO = ">=";
        public static string LESS_THAN = "<";
        public static string LESS_THAN_EQUAL_TO = "<=";
        public static string AND = "&&";
        public static string OR = "||";
        
        // One Input Logic Operators
        public static string NOT = "!";
        
        // Variables
        public static string LIST_OPEN = "[";
        public static string LIST_CLOSE = "]";
    }
}