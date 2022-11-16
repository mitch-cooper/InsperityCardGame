namespace GameState.GameRules
{
    public interface IBoardItemCard : ICard
    {
        AttackValueState Attack { get; }
        HealthValueState Health { get; }
    }
}