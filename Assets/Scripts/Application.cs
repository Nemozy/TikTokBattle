using System;
using System.Threading;
using UnityEngine;

public class Application : MonoBehaviour
{
    private Game _game;
    private GameLoopComponent _gameLoopComponent;
    private readonly CancellationTokenSource _cts = new ();
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        UnityEngine.Application.targetFrameRate = 60;
        _gameLoopComponent = gameObject.GetComponent<GameLoopComponent>();
    }
    
    private void Start()
    {
        Launch();
    }
    
    private async void Launch()
    {
        try
        {
            await Library.Load(_cts.Token);
            var game = new Game();
            game.Connect(_gameLoopComponent);
            await game.Load(_cts.Token);
            _game = game;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            _game?.OnApplicationQuit();
            _gameLoopComponent.StopCoroutines();
            _game = null;
        }
    }
}