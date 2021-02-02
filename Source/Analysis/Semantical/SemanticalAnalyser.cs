﻿using Krypton.Analysis.Ast;
using System;

namespace Krypton.Analysis.Semantical
{
    public sealed class SemanticalAnalyser
    {
        public SemanticalAnalyser(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public SyntaxTree SyntaxTree { get; }

        public bool PerformSemanticalAnalysis()
        {
            Binder binder = new(SyntaxTree);

            bool success = binder.PerformBinding();

            if (!success)
            {
                throw new NotImplementedException();
                //return false;
            }

            TypeChecker typeChecker = new(SyntaxTree, binder.TypeManager);

            success = typeChecker.PerformTypeChecking();

            if (!success)
            {
                return false;
            }

            return true;
        }
    }
}
