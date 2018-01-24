using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using calculator;


namespace calculatorTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TwoPlusTwo()
        {
            Parser parser = new Parser("2+2");
            double actual = parser.Eval();
            Assert.AreEqual(4, actual);
        }

        [TestMethod]
        public void TwoEquals()
        {
            Parser parser = new Parser("2+2=");
            double actual = parser.Eval();
            Assert.AreEqual(4, actual);
            parser.resume("+5=");
            actual = parser.Eval();
            Assert.AreEqual(9, actual);
        }

        [TestMethod]
        public void Clear()
        {
            Parser parser = new Parser("7 + 8 C + 7 =");
            double actual = parser.Eval();
            Assert.AreEqual(14, actual);
        }

        [TestMethod]
        public void NegativeOrderOfOperationsDecimal()
        {
            Parser parser = new Parser("-5*5/3=");
            double actual = parser.Eval();
            Assert.AreEqual(-8.3333333333333333, actual);
        }

        [TestMethod]
        public void Negative()
        {
            Parser parser = new Parser("7 + - 6 =");
            double actual = parser.Eval();
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void NegativeOrderOfOperations()
        {
            Parser parser = new Parser("-5 * 5 - 15 / 3 =");
            double actual = parser.Eval();
            Assert.AreEqual(-30, actual);
        }

        [TestMethod]
        public void FactorialAllClear()
        {
            Parser parser = new Parser("5! / 12 A + 9 =");
            double actual = parser.Eval();
            Assert.AreEqual(9, actual);
        }

        [TestMethod]
        public void Reciprocal()
        {
            Parser parser = new Parser("0.5 1/x * 2 =");
            double actual = parser.Eval();
            Assert.AreEqual(4, actual);
        }
    }
}
