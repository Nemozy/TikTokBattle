using System;
using Battle;
using Core;
using Model;

namespace ViewModel
{
    public class BattleViewModel
    {
        public readonly Observable<BattleStatus> BattleStatus = new ();
        
        private readonly IBattle _battle;
        private Action _gameStartCallback;

        public BattleViewModel(IBattle battle, Action gameStartCallback)
        {
            _battle = battle;
            _gameStartCallback = gameStartCallback;
            BattleStatus.Set(Battle.BattleStatus.LOADING);
        }

        public void OnTick()
        {
            if (BattleStatus != _battle.BattleStatus)
            {
                BattleStatus.Set(_battle.BattleStatus);
            }
        }
        
        public void GameStart()
        {
            _gameStartCallback?.Invoke();
        }
    }
}