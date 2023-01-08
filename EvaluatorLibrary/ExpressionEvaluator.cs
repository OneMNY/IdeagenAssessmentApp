namespace EvaluatorLibrary
{
    public static class ExpressionEvaluator
    {
        private static readonly IDictionary<string, Func<double, double, double>> _operations =
            new Dictionary<string, Func<double, double, double>>
            {
                {"+", (x, y) => x + y},
                {"-", (x, y) => x - y},
                {"*", (x, y) => x * y},
                {"/", (x, y) => x / y},
            };

        private static readonly IDictionary<string, int> _precedence =
            new Dictionary<string, int>
            {
                {"+", 1},
                {"-", 1},
                {"*", 2},
                {"/", 2},
            };

        public static double Calculate(string expression)
        {
            // Check if expression < 5 then its not a valid expression, min eg. 1 + 1
            if (expression.Length < 5)
            {
                throw new InvalidOperationException("Invalid expression");
            }

            //check for opening bracket and perform operation for this expression first
            string revisedExpr = expression;

            // Loop for open and closing bracket first to search for expression
            while (revisedExpr.Contains('('))
            {
                if (revisedExpr.IndexOf(')') == -1)
                {
                    throw new InvalidOperationException("Invalid expression");
                }
                int openBracketIndex = revisedExpr.IndexOf('(');
                int closeBracketIndex = revisedExpr.IndexOf(')');
                string subExpression = revisedExpr.Substring(openBracketIndex, closeBracketIndex - openBracketIndex + 1);

                var result = EvaluateWithPrecedence(subExpression);

                revisedExpr = revisedExpr.Replace(subExpression, result.ToString());
            };

            // final evaluation
            return EvaluateWithPrecedence(revisedExpr);
        }

        private static double EvaluateWithPrecedence(string expression)
        {
            //if a single character and is a double the return immediately
            if (double.TryParse(expression, out double output))
            {
                return output;
            }

            // Assuming input is ( 1 + 3 - 2 / 1 * 4 ), First thing to do is to check the operator precedence first 
            // If i input into a stack would be +,-,/,* so while i execute from left to right, have to check for next operator if the precedence is higher 
            // If higher then have to execute those first then replace the value of the 2nd operand of previous operation if exist
            // If not then just evaluate then replace the 1st operand of the next operation, if exist

            var opList = new List<SimpleOperation>();

            var charArray = expression.Split(' ');
            for(int i = 0; i < charArray.Length; i++)
            {
                if (_operations.ContainsKey(charArray[i]))
                {
                    var firstOperand = double.Parse(charArray[i - 1]);
                    var secondOperand = double.Parse(charArray[i + 1]);
                    opList.Add(new SimpleOperation
                    { 
                        Operator = charArray[i], 
                        FirstOperand = firstOperand, 
                        SecondOperand = secondOperand 
                    });
                }
            }

            // throw error if no operation
            if (!opList.Any())
            {
                throw new InvalidOperationException("Invalid expression");
            }

            //loop through all operations and evaluate using precedence
            double result = 0;
            int currentMarker = 0;
            while (opList.Count > 0)
            {
                var op = opList[currentMarker];
                SimpleOperation prevOp = null;
                SimpleOperation nextOp = null;

                if ((currentMarker + 1) < opList.Count)
                {
                    nextOp = opList[currentMarker + 1];
                }

                if (currentMarker > 0 && opList.Count > currentMarker)
                {
                    prevOp = opList[currentMarker - 1];
                }

                // check if have next operation
                if (nextOp != null)
                {
                    // compare and skip if next op have greater precedence
                    if (_precedence[nextOp.Operator] > _precedence[op.Operator])
                    {
                        currentMarker++;
                        continue;
                    }
                    else
                    {
                        //execute operations if lower precedence or equal
                        var tempResult = _operations[op.Operator](op.FirstOperand, op.SecondOperand);

                        //assign as the first operand for next operation
                        nextOp.FirstOperand = tempResult;

                        //check if have previous operation
                        if (prevOp != null)
                        {
                            //if have then assign the 2nd operand and reset marker since we are at the last marker
                            prevOp.SecondOperand = tempResult;
                        }

                        // remove operation from list
                        opList.RemoveAt(currentMarker);
                    }
                }
                else
                {
                    //if dont have then just perform the operations
                    var tempResult =  _operations[op.Operator](op.FirstOperand, op.SecondOperand);

                    //check if have previous operation
                    if (prevOp != null)
                    {
                        //if have then assign the 2nd operand and reset marker since we are at the last marker
                        prevOp.SecondOperand = tempResult;

                        // remove operation from list
                        opList.RemoveAt(currentMarker);

                        currentMarker = 0;
                    }
                    else
                    {
                        // final result
                        result = tempResult;
                        break;
                    }
                }
            }

            return result;

        }

        public static double CalculateWithNestedBracketAndPrecedence(string expression)
        {
            // Check if expression < 5 then its not a valid expression, min eg. 1 + 1
            if (expression.Length < 5)
            {
                throw new InvalidOperationException("Invalid expression");
            }

            var numStack = new Stack<double>();
            var operatorStack = new Stack<string>();

            // Split the expression into tokens
            var tokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double operand))
                {
                    // If the token is an operand, push it to the operand stack
                    numStack.Push(operand);
                }
                else if (_operations.ContainsKey(token))
                {
                    // If the token is an operator, push it to the operator stack
                    // after performing any pending operations with higher precedence
                    while (operatorStack.Count > 0 &&
                           (operatorStack.Peek() != "(" &&
                           _precedence[token] <= _precedence[operatorStack.Peek()]))
                    {
                        var rightOperand = numStack.Pop();
                        if (numStack.Count == 0)
                        {
                            throw new InvalidOperationException("Invalid expression");
                        }
                        var leftOperand = numStack.Pop();
                        var op = operatorStack.Pop();
                        numStack.Push(_operations[op](leftOperand, rightOperand));
                    }
                    operatorStack.Push(token);
                }
                else if (token == "(")
                {
                    // If the token is an opening parenthesis, push it to the operator stack
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    // If the token is a closing parenthesis, pop the operand and operator stacks
                    // and perform the operation until we find an opening parenthesis
                    while (operatorStack.Peek() != "(")
                    {
                        var rightOperand = numStack.Pop();
                        if (numStack.Count == 0)
                        {
                            throw new InvalidOperationException("Invalid expression");
                        }
                        var leftOperand = numStack.Pop();
                        var oper = operatorStack.Pop();
                        numStack.Push(_operations[oper](leftOperand, rightOperand));
                    }

                    // Pop the opening parenthesis from the operator stack
                    operatorStack.Pop();
                }
            }

            // After all the token have been processed, perform any remaining operations
            while (operatorStack.Count > 0)
            {
                var rightOperand = numStack.Pop();
                if (numStack.Count == 0)
                {
                    throw new InvalidOperationException("Invalid expression");
                }
                var leftOperand = numStack.Pop();
                var op = operatorStack.Pop();
                numStack.Push(_operations[op](leftOperand, rightOperand));
            }

            // The result will be the top of the operand stack
            return numStack.Pop();
        }
    } 
}
