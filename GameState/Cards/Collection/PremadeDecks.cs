using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.Cards.Collection
{
    public static class PremadeDecks
    {
        public static Deck BenefitsDeck1(Guid playerId)
        {
            return new Deck(playerId, new List<Card>(Constants.BaseDeckSize)
            {
                NeutralCards.Intern().Build(playerId),
                BenefitsCards.TeamMember().Build(playerId),
                BenefitsCards.TeamMember().Build(playerId),
                BenefitsCards.MissedOpenEnrollment().Build(playerId),
                BenefitsCards.MissedOpenEnrollment().Build(playerId),
                BenefitsCards.BenefitBoy().Build(playerId),
                BenefitsCards.BenefitBoy().Build(playerId),
                BenefitsCards.FSAFreezer().Build(playerId),
                BenefitsCards.FSAFreezer().Build(playerId),
                NeutralCards.Assistant().Build(playerId),
                NeutralCards.Assistant().Build(playerId),
                BenefitsCards.FreezeContributions().Build(playerId),
                BenefitsCards.FreezeContributions().Build(playerId),
                NeutralCards.Developer().Build(playerId),
                NeutralCards.Developer().Build(playerId),
                NeutralCards.Employee().Build(playerId),
                BenefitsCards.HSAContributer().Build(playerId),
                BenefitsCards.HSAContributer().Build(playerId),
                NeutralCards.Boss().Build(playerId),
                BenefitsCards.WorkedToDeath().Build(playerId),
                BenefitsCards.WorkedToDeath().Build(playerId),
                BenefitsCards.PerformanceReview().Build(playerId),
                BenefitsCards.PerformanceReview().Build(playerId),
                NeutralCards.Manager().Build(playerId),
                NeutralCards.Manager().Build(playerId),
                BenefitsCards.Volunteer().Build(playerId),
                BenefitsCards.Volunteer().Build(playerId),
                BenefitsCards.CommunityLeader().Build(playerId),
                BenefitsCards.CommunityLeader().Build(playerId),
                BenefitsCards.SolutionsDesigner().Build(playerId),
            });
        }

        public static Deck PayrollDeck1(Guid playerId)
        {
            return new Deck(playerId, new List<Card>(Constants.BaseDeckSize)
            {
                PayrollCards.Paycheck().Build(playerId),
                PayrollCards.Overtime().Build(playerId),
                PayrollCards.Overtime().Build(playerId),
                PayrollCards.Bonus().Build(playerId),
                PayrollCards.Bonus().Build(playerId),
                PayrollCards.ReportedToHR().Build(playerId),
                PayrollCards.ReportedToHR().Build(playerId),
                PayrollCards.BusinessAnalyst().Build(playerId),
                PayrollCards.BusinessAnalyst().Build(playerId),
                PayrollCards.QuickWithdrawer().Build(playerId),
                PayrollCards.QuickWithdrawer().Build(playerId),
                PayrollCards.FourZeroOneKInvestment().Build(playerId),
                PayrollCards.FourZeroOneKInvestment().Build(playerId),
                NeutralCards.Employee().Build(playerId),
                NeutralCards.Employee().Build(playerId),
                NeutralCards.Developer().Build(playerId),
                PayrollCards.Healthcare().Build(playerId),
                PayrollCards.Healthcare().Build(playerId),
                NeutralCards.Manager().Build(playerId),
                NeutralCards.Boss().Build(playerId),
                NeutralCards.Boss().Build(playerId),
                PayrollCards.Layoffs().Build(playerId),
                PayrollCards.Layoffs().Build(playerId),
                PayrollCards.Payroller().Build(playerId),
                PayrollCards.Payroller().Build(playerId),
                PayrollCards.BigPresentation().Build(playerId),
                PayrollCards.BigPresentation().Build(playerId),
                NeutralCards.CEO().Build(playerId),
                PayrollCards.Recruited().Build(playerId)
            });
        }
    }
}
