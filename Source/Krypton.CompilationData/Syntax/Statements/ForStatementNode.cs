using Krypton.CompilationData.Syntax.Clauses;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    /* This is the full form of the For statement fml
    [0] [1] [2] [ 3  ]  [4] [5] [ 6 ] [ 7  ] [8 ] [9] [10] [11 ]
    For Var  i  As Int   =   0  While i < 10 With  i   =   i + 2
     */
    public sealed record ForStatementNode : LoopStatementNode
    {
        public ForStatementNode(ReservedKeywordToken forKeyword,
                                ReservedKeywordToken? varKeyword,
                                IdentifierToken iterationVariableDeclarationIdentifier,
                                AsClauseNode? iterationVariableAsClause,
                                SyntaxCharacterToken? assignmentEquals,
                                ExpressionNode? initialValue,
                                ReservedKeywordToken? whileKeyword,
                                ExpressionNode? condition,
                                ReservedKeywordToken? withKeyword,
                                IdentifierToken? iterationVariableWithIdentifier,
                                SyntaxCharacterToken? withEquals,
                                ExpressionNode? withNewValue,
                                BodyNode body)
            : base(body)
        {
            // Assert that all keywords and syntax chars are correct
            Debug.Assert(forKeyword.Keyword == ReservedKeyword.For);
            Debug.Assert(varKeyword == null || varKeyword.Keyword == ReservedKeyword.Var);
            Debug.Assert(assignmentEquals == null || assignmentEquals.SyntaxCharacter == SyntaxCharacter.Equals);
            Debug.Assert(whileKeyword == null || whileKeyword.Keyword == ReservedKeyword.While);
            Debug.Assert(withKeyword == null || withKeyword.Keyword == ReservedKeyword.With);
            Debug.Assert(withEquals == null || withEquals.SyntaxCharacter == SyntaxCharacter.Equals);

            // Now to the complicated part: figure out if null fits for all...
            Debug.Assert(varKeyword == null || (assignmentEquals != null) && (initialValue != null));
            Debug.Assert((assignmentEquals == null) == (initialValue == null));
            Debug.Assert((whileKeyword == null) == (condition == null));
            Debug.Assert((withKeyword == null)
                      == (iterationVariableWithIdentifier == null)
                      == (withEquals == null)
                      == (withNewValue == null));

            ForKeywordToken = forKeyword;
            VarKeywordToken = varKeyword;
            IterationVariableDeclarationIdentifierToken = iterationVariableDeclarationIdentifier;
            IterationVariableAsClauseNode = iterationVariableAsClause;
            AssignmentEqualsToken = assignmentEquals;
            InitialValueNode = initialValue;
            WhileKeywordToken = whileKeyword;
            ConditionNode = condition;
            WithKeywordToken = withKeyword;
            IterationVariableWithIdentifierToken = iterationVariableWithIdentifier;
            WithEqualsToken = withEquals;
            WithNewValueNode = withNewValue;
        }

        public ReservedKeywordToken ForKeywordToken { get; init; } // [0]

        public ReservedKeywordToken? VarKeywordToken { get; init; } // [1]

        public IdentifierToken IterationVariableDeclarationIdentifierToken { get; init; } // [2]

        public AsClauseNode? IterationVariableAsClauseNode { get; init; } // [3]

        public SyntaxCharacterToken? AssignmentEqualsToken { get; init; } // [4]

        public ExpressionNode? InitialValueNode { get; init; } // [5]

        public ReservedKeywordToken? WhileKeywordToken { get; init; } // [6]

        public ExpressionNode? ConditionNode { get; init; } // [7]

        public ReservedKeywordToken? WithKeywordToken { get; init; } // [8]

        public IdentifierToken? IterationVariableWithIdentifierToken { get; init; } // [9]

        public SyntaxCharacterToken? WithEqualsToken { get; init; } // [10]

        public ExpressionNode? WithNewValueNode { get; init; } // [11]

        public override bool IsLeaf => false;

        public override void WriteCode(TextWriter output)
        {
            ForKeywordToken.WriteCode(output);
            VarKeywordToken?.WriteCode(output);
            IterationVariableDeclarationIdentifierToken.WriteCode(output);
            IterationVariableAsClauseNode?.WriteCode(output);
            AssignmentEqualsToken?.WriteCode(output);
            InitialValueNode?.WriteCode(output);
            WhileKeywordToken?.WriteCode(output);
            ConditionNode?.WriteCode(output);
            WithKeywordToken?.WriteCode(output);
            IterationVariableWithIdentifierToken?.WriteCode(output);
            WithEqualsToken?.WriteCode(output);
            WithNewValueNode?.WriteCode(output);
            BodyNode.WriteCode(output);
        }
    }
}
