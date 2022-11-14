using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GameState.Cards.Collection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class CardTest
    {
        [TestMethod]
        public void PrintGameToString_Minion()
        {
            var sut = NeutralCards.Employee().Build(1);

            sut.PrintGameToString();
        }

        [TestMethod]
        public void PrintGameToString_DamagedMinion()
        {
            var sut = NeutralCards.Developer().Build(1);
            sut.TakeDamage(1);

            sut.PrintGameToString();
        }

        [TestMethod]
        public void PrintGameToString()
        {
            var sut = NeutralCards.Employee().Build(1);

            sut.PrintGameToString();
        }
    }
}
