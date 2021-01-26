﻿using Krypton.Framework;
using Krypton.Framework.Literals;
using NUnit.Framework;
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
    }
}