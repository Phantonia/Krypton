using Krypton.Analysis;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Semantical.IdentifierMaps;
using Krypton.Framework;
using Krypton.Framework.Literals;
using Krypton.Framework.Symbols;
using NUnit.Framework;
using System;
using System.Linq;

namespace UnitTests
{
    public sealed class FrameworkTests
    {
        private FrameworkVersion frw = null!;

        [SetUp]
        public void Setup()
        {
            frw = FrameworkProvider.GetFrameworkVersion0();
        }

        [Test]
        public void RationalAndComplexTests()
        {
            Rational r = new(1, 2);
            Assert.AreEqual(1, r.Numerator);
            Assert.AreEqual(2, r.Denominator);

            r = new Rational(4, 8);
            Assert.AreEqual(1, r.Numerator);
            Assert.AreEqual(2, r.Denominator);

            r = new Rational(3, 8);
            Assert.AreEqual(3, r.Numerator);
            Assert.AreEqual(8, r.Denominator);

            r = new Rational(6, 8);
            Assert.AreEqual(3, r.Numerator);
            Assert.AreEqual(4, r.Denominator);

            Complex z = new(new Rational(4, 1), new Rational(3, 1));
            Assert.AreEqual(4, z.Real.Numerator);
            Assert.AreEqual(3, z.Imaginary.Numerator);
        }

        [Test]
        public void TypeTest()
        {
            Assert.AreEqual(6, frw.Types.Count);
            Assert.AreEqual(FrameworkType.String, frw.Types[FrameworkType.String].FrameworkType);
            Assert.AreEqual(3, frw.Types[FrameworkType.String].BinaryOperations.Count);
        }

        [Test]
        public void FuncTest()
        {
            Assert.AreEqual(1, frw.Functions.Count);
            Assert.AreEqual(1, frw.Functions.First(f => f.Name == "Output").Parameters?.Count);
        }

        [Test]
        public void IntegrationTest()
        {
            HoistedIdentifierMap gl = new();
            TypeIdentifierMap tp = new();

            FrameworkIntegration.PopulateWithFrameworkSymbols(gl, tp);

            string[] types = { "String", "Int", "Char", "Complex", "Bool", "Rational" };
            foreach (string t in types)
            {
                Assert.IsTrue(tp.TryGet(t, out _));
            }

            Assert.IsTrue(gl.TryGet("Output", out _));
        }

        [Test]
        public void ConstantTest()
        {
            var pi = frw.Constants.First(c => c.Name == "PI") as ConstantSymbol<Rational>;
            Assert.NotNull(pi);

            double piValue = (double)pi!.Value.Numerator / pi.Value.Denominator;
            Assert.AreEqual(Math.PI, piValue);
        }

        [Test]
        public void ConstantMapTest()
        {
            HoistedIdentifierMap gl = new();
            TypeIdentifierMap tp = new();

            FrameworkIntegration.PopulateWithFrameworkSymbols(gl, tp);

            string[] constants = { "PI", "E", "TAU", "PHI" };
            foreach (string c in constants)
            {
                Assert.IsTrue(gl.TryGet(c, out var sym));
                Assert.IsInstanceOf<ConstantSymbolNode<Rational>>(sym);
            }
        }

        [Test]
        public void OperationsTest()
        {
            TypeIdentifierMap tp = new();

            FrameworkIntegration.PopulateWithFrameworkSymbols(new HoistedIdentifierMap(), tp);

            Assert.IsTrue(tp.TryGet("String", out TypeSymbolNode? stringType));
            Assert.AreEqual(3, stringType!.BinaryOperationNodes.Count);
            Assert.IsTrue(stringType.BinaryOperationNodes.ContainsKey(Operator.Plus));
            Assert.IsTrue(ReferenceEquals(stringType.BinaryOperationNodes[Operator.Plus].LeftOperandTypeNode, stringType));
        }
    }
}
