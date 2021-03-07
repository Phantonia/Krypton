using Krypton.Analysis;
using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Statements;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Krypton.CodeGeneration
{
    internal sealed partial class CodeGeneratorCore
    {
        public CodeGeneratorCore(Compilation compilation, string template, CodeGenerationMode mode)
        {
            this.compilation = compilation;
            this.template = template;
            this.mode = mode;
        }

        private readonly Compilation compilation;
        private int globalLoopCount = 0;
        private readonly Dictionary<ILoopStatementNode, int> associatedLoopIds = new();
        private readonly StringBuilder output = new();
        private readonly string template;
        private readonly CodeGenerationMode mode;

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
            switch (mode)
            {
                case CodeGenerationMode.None:
                    Debug.Fail(message: null);
                    break;
                case CodeGenerationMode.ToFile:
                    output.Append("function $main()");
                    EmitStatementBlock(compilation.Program.TopLevelStatementNodes);
                    output.Append("$main();function Output(s){console.log(s)};");
                    break;
                case CodeGenerationMode.Run:
                    output.Append(@"var o="""";function Output(s){o+=s+""\r\n"";}");
                    output.Append("module.exports=callback=>{");
                    EmitStatementBlock(compilation.Program.TopLevelStatementNodes);
                    output.Append("callback(null,o);}");
                    break;
            }
        }
    }
}
