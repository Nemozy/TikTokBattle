using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Conf;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform _unitContainer;
    [SerializeField] private Button _button;
    
    private IBattle _battle;
    
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
        _battle = new Battle(_unitContainer);
        _button.onClick.AddListener(GameStart);
    }
}