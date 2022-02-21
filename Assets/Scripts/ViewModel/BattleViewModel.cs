using System;
using Battle;
using Conf;
using Core;
using Model;

namespace ViewModel
{
    public class BattleViewModel
    {
        public readonly Observable<BattleStatus> BattleStatus = new ();
        public readonly Observable<(Unit, UnitInfo)> OnUnitCreated = new ();
        public readonly Observable<int> OnUnitDie = new ();
        public readonly Observable<(int, int, int)> OnUnitMoved = new ();
        public readonly Observable<Action> OnGameStart = new ();
        
        public Action OnDestroyAllUnits;
    }
}