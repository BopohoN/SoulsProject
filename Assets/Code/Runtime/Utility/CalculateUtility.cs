using Code.Configuration;
using Code.Runtime.Utility.PolishExpression;
using UnityEngine;

namespace Code.Runtime.Utility
{
    public static class CalculateUtility
    { 
        public static class FormulaId
        {
            public const int BaseHp = 101;
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