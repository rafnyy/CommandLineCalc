using System;

namespace calculator
{
    public interface ICommand
    {
        double Eval();
    }

    abstract public class ITwoParamCommand : ICommand
    {
        protected double left;
        protected double right;

        public ITwoParamCommand(double left, double right)
        {
            this.left = left;
            this.right = right;
        }

        abstract public double Eval();
    }

    public class Add : ITwoParamCommand
    {
        public Add(double left, double right) : base(left, right) { }

        public override double Eval()
        {
            return left + right;
        }
    }

    public class Subtract : ITwoParamCommand
    {
        public Subtract(double left, double right) : base(left, right) { }

        public override double Eval()
        {
            return left - right;
        }
    }

    public class Multiply : ITwoParamCommand
    {
        public Multiply(double left, double right) : base(left, right) { }

        public override double Eval()
        {
            return left * right;
        }
    }

    public class Divide : ITwoParamCommand
    {
        public Divide(double left, double right) : base(left, right) { }

        public override double Eval()
        {
            if (right == 0)
            {
                throw new DivideByZeroException();
            }

            return left / right;
        }
    }

    public class Reciprocal : ICommand
    {
        private double constant;

        public Reciprocal(double constant)
        {
            this.constant = constant;
        }

        public double Eval()
        {
            if (constant == 0)
            {
                throw new DivideByZeroException();
            }

            return 1 / constant;
        }
    }

    public class Factorial : ICommand
    {
        private int constant;

        public Factorial(int constant)
        {
            this.constant = constant;
        }

        public double Eval()
        {
            if (constant < 0)
            {
                throw new NotSupportedException();
            }

            int product = 1;

            for (int i = 2; i <= constant; i++)
            {
                product *= i;
            }

            return product;
        }
    }

    public class EquationTree : ICommand
    {
        Node tree = null;
        public void addValue(double value)
        {
            if(tree is OperatorNode)
            {
                ((OperatorNode)tree).addRightNode(new ValueNode(value));
            }
            else
            {
                tree = new ValueNode(value);
            }
        }

        public void addOperator(char op)
        {
            // logic for order of operators must be here
            int precedence = 0;
            if (op == '/' || op == '*')
            {
                precedence = 2;
            }
            else
            {
                precedence = 1;
            }

            if (tree.getPrecedence() >= precedence)
            {
                tree = new OperatorNode(tree, op);
            }
            else
            {
                if (tree is OperatorNode)
                {
                    Node valueNode = ((OperatorNode)tree).right;
                    ((OperatorNode)tree).right = null;
                    ((OperatorNode)tree).addRightNode(new OperatorNode(valueNode, op));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void clear()
        {
            tree = tree.clear();
        }

        public double Eval()
        {
            return tree.Eval();
        }
    }

    public abstract class Node : ICommand
    {
        abstract public double Eval();

        abstract public int getPrecedence();

        abstract public Node clear();
    }

    public class ValueNode : Node
    {
        double value;

        public ValueNode(double value)
        {
            this.value = value;
        }

        public override double Eval()
        {
            return value;
        }

        public override int getPrecedence()
        {
            return 3;
        }

        public override Node clear()
        {
            return null;
        }
    }

    public class OperatorNode : Node
    {
        char op;
        public Node right;
        public Node left;

        public OperatorNode(Node node, char op)
        {
            this.op = op;
            left = node;
            right = null;
        }

        public void addRightNode(Node node)
        {
            if (right is OperatorNode)
            {
               ((OperatorNode)right).addRightNode(node);
            }
            else
            {
                 right = node;
            }
        }

        public override double Eval()
        {
            if (op == '/')
            {
                return new Divide(left.Eval(), right.Eval()).Eval();
            }
            else if (op == '*')
            {
                return new Multiply(left.Eval(), right.Eval()).Eval();
            }
            else if (op == '-')
            {
                return new Subtract(left.Eval(), right.Eval()).Eval();
            }
            else if (op == '+')
            {
                return new Add(left.Eval(), right.Eval()).Eval();
            }
            else
            {
                throw new Exception();
            }
        }

        public override int getPrecedence()
        {
            if (op == '/' || op == '*')
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public override Node clear()
        {
            if (this.right is ValueNode || this.right == null)
            {
                return this.left;
            }
            else
            {
                return this.right.clear();
            }
        }
    }

    public class Parser : ICommand
    {
        EquationTree equationTree = new EquationTree();
        String line;
        double evaluatedValue = 0;

        public Parser(String line)
        {
            this.line = line;
        }

        public void resume(String line)
        {
            this.line = line;
        }

        public double Eval()
        {
            String num = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if ((num == "" && c == '-') || c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == '.')
                {
                    num += c;
                }
                else if (c == 'A' || c == 'a')
                {
                    num = "";
                    equationTree = new EquationTree();
                    equationTree.addValue(0);
                    evaluatedValue = 0;
                }
                else if (c == 'C' || c == 'c')
                {
                    num = "";
                    equationTree.clear();
                }
                else if (c == '!')
                {
                    int x = Int32.Parse(num);

                    Factorial factorial = new Factorial(x);
                    equationTree.addValue(factorial.Eval());
                    num = "";
                }
                else if (c == '/')
                {
                    if (line[i + 1] == 'x')
                    {
                        if (line[i - 1] != '1')
                        {
                            throw new Exception();
                        }
                        num = num.Substring(0, num.Length - 1);
                        double x = Convert.ToDouble(num);
                        Reciprocal reciprocal = new Reciprocal(x);
                        equationTree.addValue(reciprocal.Eval());
                        num = "";
                        i++;
                    }
                    else
                    {
                        if (num != "")
                        {
                            double x = Convert.ToDouble(num);
                            equationTree.addValue(x);
                        }
                        equationTree.addOperator(c);
                        num = "";
                    }
                }
                else if (c == '*' || c == '-' || c == '+')
                {
                    if (num != "")
                    {
                        double x = Convert.ToDouble(num);
                        equationTree.addValue(x);
                    }
                    equationTree.addOperator(c);
                    num = "";
                }

            }

            if (num != "")
            {
                equationTree.addValue(Convert.ToDouble(num));
            }

            evaluatedValue = equationTree.Eval();
            equationTree = new EquationTree();
            equationTree.addValue(evaluatedValue);
            return evaluatedValue;
        }

        /* The test class or client */
        internal class Program
        {
            public static void Main(string[] arguments)
            {
                char ch;
                string equation = "";

                do
                {
                    int input = Console.Read();
                    try
                    {
                        ch = Convert.ToChar(input);
                        equation += ch;
                        if (ch == '=')
                        {
                            Parser parser = new Parser(equation);
                            double result = parser.Eval();
                            Console.WriteLine(result);
                            equation = System.Convert.ToString(result);

                        }
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine(e.Message);
                        ch = 'q';
                    }
                } while (ch != 'q' && ch != 'Q');
            }
        }
    }
}