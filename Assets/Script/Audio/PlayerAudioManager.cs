using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public GameObject parentObject;

    public void PostFootstepsAudio()
    {
        AudioManager.instance.FOLEYS_Char_Foosteps.Post(parentObject);
    }

    public void PostLandingAudio()
    {
        AudioManager.instance.FOLEYS_Char_Landing.Post(parentObject);
    }

    public void PostJumpAudio()
    {
        AudioManager.instance.FOLEYS_Char_TakeOff.Post(parentObject);
    }
}
