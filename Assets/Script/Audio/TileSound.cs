using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSound : MonoBehaviour
{
    public AK.Wwise.Event eventToPost;

    private void Start()
    {
        StartCoroutine(DestroyDelay());
        PostEvent();
    }

    private void PostEvent()
    {
        eventToPost.Post(gameObject);
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
