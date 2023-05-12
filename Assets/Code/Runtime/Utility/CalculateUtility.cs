using Code.Configuration;
using Code.Runtime.Utility.PolishExpression;

namespace Code.Runtime.Utility
{
    public static class CalculateUtility
    { 
        public static class FormulaId
        {
            public const int BaseHp = 101;
            public const int BaseStamina = 102;
        }

        public static class ConstId
        {
            public const int StatusMaxLevel = 10101;
            public const int HpSoftMaxValue = 10102;
            public const int HpSoftMaxLevel = 10103;
            public const int MaxHp = 10104;
            public const int StaminaSoftMaxValue = 10105;
            public const int StaminaSoftMaxLevel = 10106;
            public const int MaxStamina = 10107;
            public const int MinStamina = 10108;
        }

        private static readonly string[] FormulaParamTags = new string[]
        {
            "A","B","C","D","E","F","G",
        };

        public static float CalculateValueFromFormula(int formulaId, params string[] values)
        {
            var tag = EquationConfig.D[formulaId].Equation;
            for (var i = 0; i < values.Length; i++)
                tag = tag.Replace(FormulaParamTags[i], values[i]);
            //var postfixString = ShuntingYard.InfixToPostfixString(tag); // 将中缀表达式转换为后缀表达式
            //Debug.log(postfixString);
            var rpnValue = (float) InfixEvaluator.EvaluateInfix(tag);
            return rpnValue;
        }
    }
}