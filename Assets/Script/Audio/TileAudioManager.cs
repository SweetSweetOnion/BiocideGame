using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAudioManager : MonoBehaviour
{
    public static TileAudioManager instance;

    public GameObject audioSourcePrefab;

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void PostTileHitSound(Vector3 position, float tileHP, bool damaged)
    {
        GameObject _audioSource = audioSourcePrefab;

        Instantiate(_audioSource, gameObject.transform);
        _audioSource.transform.position = position;

        AkSoundEngine.SetRTPCValue("RTPC_TileHP", tileHP, _audioSource);

        if (damaged)
            _audioSource.GetComponent<TileSound>().eventToPost = AudioManager.instance.FOLEYS_Tile_Hit;
        else
            _audioSource.GetComponent<TileSound>().eventToPost = AudioManager.instance.FOLEYS_Tile_Resist;
    }

    public void PostTileDestroySound(Vector3 position)
    {
        GameObject _audioSource = audioSourcePrefab;

        Instantiate(_audioSource, gameObject.transform);
        _audioSource.transform.position = position;

        _audioSource.GetComponent<TileSound>().eventToPost = AudioManager.instance.FOLEYS_Tile_Destroy;
    }

    IEnumerator DestroyDelay(GameObject audioSource)
    {
        yield return new WaitForSeconds(2);
        Destroy(audioSource);
    }

}
