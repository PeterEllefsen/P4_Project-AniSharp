using AnimationLanguage.ASTNodes;

namespace Tests;

public class Helper
{
    public String Visit(KeyValuePairNode node)
    {
        string output = "";
        foreach (var Child in node.GetChildren())
        {

            if (Child is IdentifierNode identifierNode)
            {
                    
                output += $"setup.{identifierNode.ToString()} =";
            }

            if (Child is IntegerLiteralNode integerLiteralNode)
            {

                output += $"{integerLiteralNode.ToString()};";
            }

            if (Child is StringLiteralNode stringLiteralNode)
            {
                    
                output += $"{stringLiteralNode.ToString()};";
            }

            if (Child is FunctionCallNode functionCallNode)
            {
                    
                output += $"Functions.{functionCallNode.ToString().ToLower()};";
            }
           
           
        }

        return output;
    }
    
    public String Visit(AssignmentNode node)
    {
        String output = "";
        if (node.IsDeclaration)
        {
            output += $"{node.VariableType.ToString().ToLower()} {node.Identifier}";
        }
        else
        {
            output += $"{node.Identifier}";
        }

        if (node.IsDeclaration)
        {
            output += " = ";
        }
        else
        {
            switch (node.AssignmentOperator)
            {
                case AssignmentOperator.Assign:
                    output += " = ";
                    break;
                case AssignmentOperator.PlusEqual:
                    output += " += ";
                    break;
                case AssignmentOperator.MinusEqual:
                    output += " -= ";
                    break;
                // Add cases for other assignment operators as needed
                default:
                    // Handle the default case, if necessary
                    break;
            }
        }


        //insert expression
        output += Visit(node.Expression);
        output += ";";


        return output;
    }
    
    public String Visit(ExpressionNode node)
    {
        String output = "";
        if (node.RightOperand != null)
        {
            if (node.LeftOperand is ExpressionNode leftOperand)
            {
                output += leftOperand.ToString();
            }

            if (node.OperatorNode is OperatorNode operatorNode)
            {
                switch (node.OperatorNode.OperatorSymbol)
                {
                    case "and":
                        output += " && ";
                        break;
                    case "or":
                        output += " || ";
                        break;
                    default:
                        output += " " + operatorNode.OperatorSymbol + " ";
                        break;
                }
            }

            if (node.RightOperand is ExpressionNode rightOperand)
            {
                output += rightOperand.ToString();
            }
        }
        else
        {
            output += $"{node.ToString()}";
        }


        return output;
    }
}