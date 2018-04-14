using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public class ReturnFormula
    {
        public bool Result { set; get; }
        public double Value { set; get; }
    }

    public class StringToFormula
    {
        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, ReturnFormula>[] _operations = {
            (a1, a2) => new ReturnFormula { Result = true, Value =  a1 - a2 },
            (a1, a2) => new ReturnFormula { Result = true, Value = a1 + a2 },
            (a1, a2) => new ReturnFormula { Result = true, Value = a1 / a2 },
            (a1, a2) => new ReturnFormula { Result = true, Value = a1 * a2 },
            (a1, a2) => new ReturnFormula { Result = true, Value = Math.Pow(a1, a2) }
        };

        public Stack Reverse(Stack input)
        {
            Stack temp = new Stack();

            while (input.Count != 0)
                temp.Push(input.Pop());

            return temp;
        }

        public ReturnFormula Eval(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<ReturnFormula> operandStack = new Stack<ReturnFormula>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];
                if (token == "(")
                {
                    string subExpr = getSubExpression(tokens, ref tokenIndex);

                    if (subExpr == "Mis-matched parentheses in expression") return new ReturnFormula { Result = false };

                    operandStack.Push(Eval(subExpr));
                    continue;
                }
                if (token == ")")
                {
                    return new ReturnFormula { Result = false };
                }
                //If this is an operator  
                if (Array.IndexOf(_operators, token) >= 0)
                {
                    while (operatorStack.Count > 0)
                    {
                        string op = operatorStack.Pop();
                        ReturnFormula arg2 = operandStack.Pop();
                        ReturnFormula arg1 = operandStack.Pop();
                        operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1.Value, arg2.Value));
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    double parseToken;
                    bool ifSuccess = double.TryParse(token, out parseToken);

                    if (!ifSuccess) return new ReturnFormula { Result = false };

                    operandStack.Push(new ReturnFormula { Result = true, Value = parseToken });
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();
                ReturnFormula arg2 = operandStack.Pop();
                ReturnFormula arg1 = operandStack.Pop();
                operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1.Value, arg2.Value));
            }
            return operandStack.Pop();
        }

        private string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                return "Mis-matched parentheses in expression";
            }
            return subExpr.ToString();
        }

        private List<string> getTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
    }
}
