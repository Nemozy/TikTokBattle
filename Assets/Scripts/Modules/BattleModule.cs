using System;
using System.Linq;
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
            var battleViewCreatingTask = CreateView();
            await battleViewCreatingTask;
            
            var battleView = battleViewCreatingTask.Result;
            _battleViewModel = new BattleViewModel();
            _battleViewModel.OnGameStart.Set(GameStart);
            _battle = new Core.Battle(_battleViewModel);
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
            GameOver(false);
            return Task.CompletedTask;
        }
        
        private void OnTick()
        {
            if (_battle.BattleStatus != BattleStatus.STARTED)
            {
                return;
            }
            _battle.Tick();
            if(_battle.IsFinished())
            {
                GameOver(_battle.GetPlayerTeam().Any());
            }
        }
        
        private void GameStart()
        {
            _battle.Start(new DemoBattleInfo());
        }
    
        private void GameOver(bool won)
        {
            _battle.Finish(won);
        }
    }
}