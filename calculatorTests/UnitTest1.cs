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

        [TestMethod]
        public void ComplexOrderOfOperations()
        {
            Parser parser = new Parser("1+2*3-4*5-14/7=");
            double actual = parser.Eval();
            Assert.AreEqual(-15, actual);
        }

        [TestMethod]
        public void ComplexOrderOfOperations2()
        {
            Parser parser = new Parser("2*2+3*3-7*7=");
            double actual = parser.Eval();
            Assert.AreEqual(-36, actual);
        }

        [TestMethod]
        public void ComplexOrderOfOperations3()
        {
            Parser parser = new Parser("2*2+3*3-30/5=");
            double actual = parser.Eval();
            Assert.AreEqual(7, actual);
        }

        [TestMethod]
        public void ComplexOrderOfOperations4()
        {
            Parser parser = new Parser("2*2+3*3-30/8=");
            double actual = parser.Eval();
            Assert.AreEqual(9.25, actual);
        }

        [TestMethod]
        public void ReciprocalTwiceEqualsOriginalValue()
        {
            Parser parser = new Parser("21/x1/x=");
            double actual = parser.Eval();
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void ComplexOrderOfOperations5()
        {
            Parser parser = new Parser("3!1/x1/x!=");
            double actual = parser.Eval();
            Assert.AreEqual(6, actual);
        }
    }
}
