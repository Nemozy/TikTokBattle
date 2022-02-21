using System;
using System.Threading.Tasks;
using Battle;
using Conf;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using View;
using ViewModel;
using Object = UnityEngine.Object;

namespace Modules
{
    public class BattleModule : ModuleBase
    {
        private Core.Battle _battle;
        private BattleViewModel _battleViewModel;
        private BattleStatus _battleStatus;
        
        private async Task<BattleView> CreateView()
        {
            var handle = Addressables.LoadAssetAsync<GameObject>("Assets/Media/Prefabs/Battle/BattleView.prefab");
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded) 
            {
                var view = Object.Instantiate(handle.Result);
                return view.GetComponent<BattleView>();
            }
            
            throw new Exception(handle.OperationException.Message);
        }
        
        public override async Task Connect()
        {
            _battleStatus = BattleStatus.LOADING;
            var battleViewCreatingTask = CreateView();
            await battleViewCreatingTask;
            
            var battleView = battleViewCreatingTask.Result;
            //TODO: убрать ссылку на вьюшку
            _battle = new Core.Battle(battleView);
            _battleViewModel = new BattleViewModel(_battle, GameStart);
            //_battleView.Init();
            //_battleView.SetSceneCamera(sceneCamera);
            battleView.Connect(_battleViewModel);
        }

        public override void Tick()
        {
            base.Tick();
            
            OnTick();
        }

        public override Task Stop()
        {
            //TODO: передать в _battle информацию, что бой закончен.
            _battleStatus = BattleStatus.FINISHED_LOST;
            GameOver();
            return Task.CompletedTask;
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