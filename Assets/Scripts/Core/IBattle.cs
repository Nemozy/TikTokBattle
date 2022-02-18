using Battle;

namespace Core
{
    public interface IBattle
    {
        BattleStatus BattleStatus { get; }
        bool IsFinished();
    }
}