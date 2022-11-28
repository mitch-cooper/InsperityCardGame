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
        public Guid TheGuid = Guid.NewGuid();

        [TestMethod]
        public void PrintGameToString_Minion()
        {
            var sut = NeutralCards.Employee().Build(TheGuid);

            sut.PrintGameToString();
        }

        [TestMethod]
        public void PrintGameToString_DamagedMinion()
        {
            var sut = BenefitsCards.HSAContributor().Build(TheGuid);
            sut.TakeDamage(1);

            sut.PrintGameToString();
        }

        [TestMethod]
        public void PrintGameToString()
        {
            var sut = NeutralCards.Employee().Build(TheGuid);

            sut.PrintGameToString();
        }
    }
}
