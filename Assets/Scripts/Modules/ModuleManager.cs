using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Modules
{
    public class ModuleManager
    {
        public ModuleBase CurrentModule { get; private set; }

        public async Task LoadBattleModule(CancellationToken ct)
        {
            CurrentModule = new BattleModule();
        }

        public IEnumerator TickModule()
        {
            CurrentModule?.Tick();
            yield return new WaitForSeconds(0.5f);
        }

        public void StopGame()
        {
            CurrentModule.Stop();
        }
    }
}