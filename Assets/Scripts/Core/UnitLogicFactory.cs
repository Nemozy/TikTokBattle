using System;
using Conf;
using Logic;

namespace Core
{
    public static class UnitLogicFactory
    {
        public static UnitLogic Create(UnitInfo info, IUnit unit, ICore core)
        {
            return info switch
            {
                MeleeSoldierUnitInfo i => new MeleeSoldierUnitLogic(i, unit, core),
                MeleeAssassinUnitInfo i => new MeleeAssassinUnitLogic(i, unit, core),
                MeleeHedgehogUnitInfo i => new MeleeHedgehogUnitLogic(i, unit, core),
                RangeArcherUnitInfo i => new RangeArcherUnitLogic(i, unit, core),
                _ => throw new ArgumentOutOfRangeException(nameof(info), info, null)
            };
        }
    }
}