using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GameLoopComponent : MonoBehaviour
{
    private void Test()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var context = SynchronizationContext.Current;
        Task.Run(() => { }, token);
    }
    
    public void AddCoroutineAndStart(IEnumerator action)
    {
        StartCoroutine(action);
    }

    public void StopAndRemoveCoroutine(IEnumerator action)
    {
        StopCoroutine(action);
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }
}