namespace Krypton.Analysis.Semantics
{
    public static class SpecialNames
    {
        public const string OperatorNegation = "op_UnaryNegation";
        public const string OperatorLogicalNot = "op_LogicalNot";
        public const string OperatorBitwiseNot = "op_OnesComplement";

        // this one is not usual in .NET
        public const string OperatorPower = "op_Power";

        public const string OperatorMultiplication = "op_Multiply";
        public const string OperatorIntegerDiv = "op_Division";
        public const string OperatorRationalDiv = "op_Division";
        public const string OperatorMod = "op_Modulus";

        public const string OperatorAddition = "op_Addition";
        public const string OperatorSubtraction = "op_Subtraction";

        public const string OperatorBitwiseAnd = "op_BitwiseAnd";
        public const string OperatorBitwiseOr = "op_BitwiseOr";
        public const string OperatorBitwiseXor = "op_ExclusiveOr";
        
        public const string OperatorLeftShift = "op_LeftShift";
        public const string OperatorRightShift = "op_RightShift";

        public const string OperatorEquals = "op_Equality";
        public const string OperatorNotEquals = "op_Inequality";

        public const string OperatorLessThan = "op_LessThan";
        public const string OperatorGreaterThan = "op_GreaterThan";
        public const string OperatorLessThanEquals = "op_LessThanOrEqual";
        public const string OperatorGreaterThanEquals = "op_GreaterThanOrEqual";

        public const string ImplicitConversion = "op_Implicit";
        public const string ExplicitConversion = "op_Explicit";
    }
}
