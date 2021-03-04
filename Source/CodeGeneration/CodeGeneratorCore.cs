using Krypton.Analysis;
using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Statements;
using System.Collections.Generic;
using System.Text;

namespace Krypton.CodeGeneration
{
    internal sealed partial class CodeGeneratorCore
    {
        public CodeGeneratorCore(Compilation compilation, string template)
        {
            this.compilation = compilation;
            this.template = template;
        }

        private readonly Compilation compilation;
        private int globalLoopCount = 0;
        private readonly Dictionary<ILoopStatementNode, int> associatedLoopIds = new();
        private readonly StringBuilder output = new();
        private readonly string template;

        public string GenerateCode()
        {
            output.Append(template);

            foreach (ConstantDeclarationNode constantDeclaration in compilation.Program.Constants)
            {
                EmitConstantDeclaration(constantDeclaration);
            }

            foreach (FunctionDeclarationNode functionDeclaration in compilation.Program.Functions)
            {
                EmitFunctionDeclaration(functionDeclaration);
            }

            EmitTopLevelStatements();

            return output.ToString();
        }

        private void EmitConstantDeclaration(ConstantDeclarationNode constantDeclaration)
        {
            output.Append("const ");
            output.Append(constantDeclaration.Identifier);
            output.Append('=');
            EmitExpression(constantDeclaration.ValueNode);
            output.Append(';');
        }

        private void EmitFunctionDeclaration(FunctionDeclarationNode functionDeclaration)
        {
            output.Append("function ");
            output.Append(functionDeclaration.Identifier);

            output.Append('(');

            using IEnumerator<ParameterDeclarationNode> enumerator
                = functionDeclaration.ParameterNodes.GetEnumerator();

            if (enumerator.MoveNext())
            {
                while (true)
                {
                    output.Append(enumerator.Current.Identifier);

                    if (enumerator.MoveNext())
                    {
                        output.Append(',');
                    }
                    else
                    {
                        break;
                    }
                }
            }

            output.Append(')');

            EmitStatementBlock(functionDeclaration.BodyNode);
        }

        private void EmitStatementBlock(StatementCollectionNode statements)
        {
            output.Append('{');
            foreach (StatementNode statement in statements)
            {
                EmitStatement(statement);
            }
            output.Append('}');
        }

        private void EmitTopLevelStatements()
        {
            output.Append("function $main()");
            EmitStatementBlock(compilation.Program.TopLevelStatementNodes);
            output.Append("$main();");
        }
    }
}
