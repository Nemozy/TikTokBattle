using System;
using System.Threading;
using UnityEngine;

public class Application : MonoBehaviour
{
    [SerializeField] private GameObject _gamePrefab;
    private Game _game;

    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        UnityEngine.Application.targetFrameRate = 60;
    }
    
    private void Start()
    {
        Launch();
    }
    
    private async void Launch()
    {
        var game = Instantiate(_gamePrefab, transform).GetComponent<Game>();
        try
        {
            await Library.Load(_cts.Token);
            await game.Load(_cts.Token);
            _game = game;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Destroy(game.gameObject);
        }
    }
}