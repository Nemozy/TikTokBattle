using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Conf;
using Core;
using UnityEngine;
using UnityEngine.UI;
using View;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform _unitContainer = default;
    [SerializeField] private Button _button = default;
    
    private IBattle _battle;
    
    /*private void Awake()
    {
        UnitView.Preload();
        
        _battle = new Battle(_unitContainer);
        _button.onClick.AddListener(GameStart);
        
        GameStart();
    }*/

    private void GameStart()
    {
        _button.gameObject.SetActive(false);
        _battle.Start(new DemoBattleInfo());
        StartCoroutine(GamePlay());
    }

    private void GameOver()
    {
        _battle.Finish();
        _button.gameObject.SetActive(true);
    }
    
    private IEnumerator GamePlay()
    {
        while(!_battle.IsFinished())
        {
            _battle.Tick();
            yield return new WaitForSeconds(0.5f);
        }
        GameOver();
    }

    private void OnApplicationQuit()
    {
        _button.onClick.RemoveListener(GameStart);
        StopAllCoroutines();
    }
    
    public async Task Load(CancellationToken ct)
    {
        //await UnitView.Preload(ct);
        
        _battle = new Battle(_unitContainer);
        _button.onClick.AddListener(GameStart);
        
        GameStart();
    }
}