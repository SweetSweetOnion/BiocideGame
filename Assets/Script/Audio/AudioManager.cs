using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AK.Wwise.Event MUSIC_Main;
    public AK.Wwise.Event AMB_Destroyed;
    public AK.Wwise.Event AMB_Nature;
    public AK.Wwise.Event EFFECT_Ressource_Earn;
    public AK.Wwise.Event EFFECT_Weapon_LevelUp;
    public AK.Wwise.Event FOLEYS_Char_Foosteps;
    public AK.Wwise.Event FOLEYS_Char_Hit;
    public AK.Wwise.Event FOLEYS_Char_Landing;
    public AK.Wwise.Event FOLEYS_Char_TakeOff;
    public AK.Wwise.Event FOLEYS_Char_Death;
    public AK.Wwise.Event FOLEYS_Bird_Death;
    public AK.Wwise.Event FOLEYS_Tile_Destroy;
    public AK.Wwise.Event FOLEYS_Tile_Hit;
    public AK.Wwise.Event FOLEYS_Tile_Resist;
    public AK.Wwise.Event FOLEYS_Weapon_Reload;
    public AK.Wwise.Event FOLEYS_Weapon_Shoot;
    public AK.Wwise.Event FOLEYS_Weapon_Trigger;
    public AK.Wwise.Event UI_Menu_Select;
    public AK.Wwise.Event UI_Menu_Submit;

    public static int weaponLevel = 0;

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        AkSoundEngine.SetState("MainMusic", "Introduction");
        AkSoundEngine.SetState("DestructionLevel", "Level_00");
    }

    private void Start()
    {
        MUSIC_Main.Post(gameObject);
        AMB_Nature.Post(gameObject);
    }
}
