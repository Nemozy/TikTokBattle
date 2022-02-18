using System.Collections;
using UnityEngine;

public class GameLoopComponent : MonoBehaviour
{
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