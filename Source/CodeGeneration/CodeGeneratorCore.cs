using Krypton.Analysis;
using Krypton.Analysis.Ast;
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
        private readonly Dictionary<string, string> constants = new();
        private int globalLoopCount = 0;
        private readonly Dictionary<ILoopStatementNode, int> associatedLoopIds = new();
        private readonly StringBuilder output = new();
        private readonly string template;

        public string GenerateCode()
        {
            output.Append(template);
            EmitTopLevelStatements();
            return output.ToString();
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
