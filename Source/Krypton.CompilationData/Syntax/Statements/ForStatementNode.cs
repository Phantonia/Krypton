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
    public sealed class ForStatementNode : LoopStatementNode
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
                                BodyNode body,
                                SyntaxNode? parent = null)
            : base(body, parent)
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
            IterationVariableAsClauseNode = iterationVariableAsClause?.WithParent(this);
            AssignmentEqualsToken = assignmentEquals;
            InitialValueNode = initialValue?.WithParent(this);
            WhileKeywordToken = whileKeyword;
            ConditionNode = condition?.WithParent(this);
            WithKeywordToken = withKeyword;
            IterationVariableWithIdentifierToken = iterationVariableWithIdentifier;
            WithEqualsToken = withEquals;
            WithNewValueNode = withNewValue?.WithParent(this);
        }

        public SyntaxCharacterToken? AssignmentEqualsToken { get; } // [4]

        public ExpressionNode? ConditionNode { get; } // [7]

        public ReservedKeywordToken ForKeywordToken { get; } // [0]

        public ExpressionNode? InitialValueNode { get; } // [5]

        public override bool IsLeaf => false;

        public AsClauseNode? IterationVariableAsClauseNode { get; } // [3]

        public IdentifierToken IterationVariableDeclarationIdentifierToken { get; } // [2]

        public IdentifierToken? IterationVariableWithIdentifierToken { get; } // [9]

        public ReservedKeywordToken? VarKeywordToken { get; } // [1]

        public ReservedKeywordToken? WhileKeywordToken { get; } // [6]

        public SyntaxCharacterToken? WithEqualsToken { get; } // [10]

        public ReservedKeywordToken? WithKeywordToken { get; } // [8]

        public ExpressionNode? WithNewValueNode { get; } // [11]

        // Heavenly god...
        public ForStatementNode WithChildren(ReservedKeywordToken? forKeyword = null,
                                             ReservedKeywordToken? varKeyword = null,
                                             bool overwriteVarKeyword = false,
                                             IdentifierToken? iterationVariableDeclarationIdentifier = null,
                                             AsClauseNode? iterationVariableAsClause = null,
                                             bool overwriteIterationVariableAsClauseNode = false,
                                             SyntaxCharacterToken? assignmentEquals = null,
                                             bool overwriteAssignmentEquals = false,
                                             ExpressionNode? initialValue = null,
                                             bool overwriteInitialValue = false,
                                             ReservedKeywordToken? whileKeyword = null,
                                             bool overwriteWhileKeyword = false,
                                             ExpressionNode? condition = null,
                                             bool overwriteCondition = false,
                                             ReservedKeywordToken? withKeyword = null,
                                             bool overwriteWithKeyword = false,
                                             IdentifierToken? iterationVariableWithIdentifier = null,
                                             bool overwriteIterationVariableWithIdentifier = false,
                                             SyntaxCharacterToken? withEquals = null,
                                             bool overwriteWithEquals = false,
                                             ExpressionNode? withNewValue = null,
                                             bool overwriteWithNewValue = false,
                                             BodyNode? body = null)
            => new ForStatementNode(forKeyword ?? ForKeywordToken,
                                    varKeyword ?? (overwriteVarKeyword ? null : VarKeywordToken),
                                    iterationVariableDeclarationIdentifier ?? IterationVariableDeclarationIdentifierToken,
                                    iterationVariableAsClause ?? (overwriteIterationVariableAsClauseNode ? null : IterationVariableAsClauseNode),
                                    assignmentEquals ?? (overwriteAssignmentEquals ? null : AssignmentEqualsToken),
                                    initialValue ?? (overwriteInitialValue ? null : InitialValueNode),
                                    whileKeyword ?? (overwriteWhileKeyword ? null : WhileKeywordToken),
                                    condition ?? (overwriteCondition ? null : ConditionNode),
                                    withKeyword ?? (overwriteWithKeyword ? null : WithKeywordToken),
                                    iterationVariableWithIdentifier ?? (overwriteIterationVariableWithIdentifier ? null : IterationVariableWithIdentifierToken),
                                    withEquals ?? (overwriteWithEquals ? null : WithEqualsToken),
                                    withNewValue ?? (overwriteWithNewValue ? null : WithNewValueNode),
                                    body ?? BodyNode);

        public override ForStatementNode WithParent(SyntaxNode newParent)
            => new(ForKeywordToken, VarKeywordToken, IterationVariableDeclarationIdentifierToken,
                   IterationVariableAsClauseNode, AssignmentEqualsToken, InitialValueNode,
                   WhileKeywordToken, ConditionNode, WithKeywordToken, IterationVariableWithIdentifierToken,
                   WithEqualsToken, WithNewValueNode, BodyNode, newParent);

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
