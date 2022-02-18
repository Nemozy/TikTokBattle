using Battle;
using Conf;
using UnityEngine;
using View;
using ViewModel;

namespace Modules
{
    public class BattleModule : ModuleBase
    {
        private Core.Battle _battle;
        private BattleViewModel _battleViewModel;
        private BattleStatus _battleStatus;
        
        private static BattleView CreateView()
        {
            var viewPrefab = Resources.Load<GameObject>("Prefabs/Battle/BattleView");
            var view = Object.Instantiate(viewPrefab);
            
            return view.GetComponent<BattleView>();
        }

        public BattleModule()
        {
            _battleStatus = BattleStatus.LOADING;
            var battleView = CreateView();
            //TODO: убрать ссылку на вьюшку
            _battle = new Core.Battle(battleView);
            _battleViewModel = new BattleViewModel(_battle, GameStart);
            /*_battleView.Init();
            _battleView.SetSceneCamera(sceneCamera);*/
            battleView.Connect(_battleViewModel);
        }
        
        public override void Connect()
        {
            base.Connect();
        }

        public override void Tick()
        {
            base.Tick();
            
            OnTick();
        }

        public override void Stop()
        {
            //TODO: передать в _battle информацию, что бой закончен.
            _battleStatus = BattleStatus.FINISHED_LOST;
            GameOver();
        }
        
        private void OnTick()
        {
            _battleViewModel.OnTick();
            
            if (_battleStatus != BattleStatus.STARTED)
            {
                return;
            }
            if(_battle.IsFinished())
            {
                GameOver();
                
                return;
            }
            _battle.Tick();
        }
        
        private void GameStart()
        {
            _battle.Start(new DemoBattleInfo());
            _battleStatus = BattleStatus.STARTED;
        }
    
        private void GameOver()
        {
            _battle.Finish();
            //TODO: брать из боя результат победа или поражение
            _battleStatus = BattleStatus.FINISHED_LOST;
        }
    }
}