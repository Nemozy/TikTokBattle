using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Modules;

public class Game
{
    private ModuleManager _moduleManager;
    private GameLoopComponent _gameLoopComponent;
    
    public void Connect(GameLoopComponent gameLoopComponent)
    {
        _gameLoopComponent = gameLoopComponent;
        _moduleManager = new ModuleManager();
    }

    public void OnApplicationQuit()
    {
        StopLogicTick();
        _moduleManager.StopGame();
    }
    
    public async Task Load(CancellationToken ct)
    {
        await _moduleManager.LoadBattleModule(ct);
        await _moduleManager.CurrentModule.PreloadAssets();
        await _moduleManager.CurrentModule.Connect();
    }

    public void Start()
    {
        StartLogicTick();
    }

    private void StartLogicTick()
    {
        _gameLoopComponent.AddCoroutineAndStart(LogicTick());
    }

    private void StopLogicTick()
    {
        _gameLoopComponent.StopAndRemoveCoroutine(LogicTick());
    }

    private IEnumerator LogicTick()
    {
        while (true)
        {
            yield return _moduleManager.TickModule();
        }
    }
}