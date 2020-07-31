/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID AMB_DESTROYED = 1549451067U;
        static const AkUniqueID AMB_NATURE = 2327484403U;
        static const AkUniqueID EFFECT_RESSOURCE_EARN = 3812594291U;
        static const AkUniqueID EFFECT_WEAPON_LEVELUP = 2264089113U;
        static const AkUniqueID FOLEYS_CHAR_FOOTSTEPS = 1962994296U;
        static const AkUniqueID FOLEYS_CHAR_HIT = 2368415702U;
        static const AkUniqueID FOLEYS_CHAR_LANDING = 3542894396U;
        static const AkUniqueID FOLEYS_CHAR_TAKEOFF = 1417297257U;
        static const AkUniqueID FOLEYS_TILE_DESTROY = 2560249231U;
        static const AkUniqueID FOLEYS_TILE_HIT = 3547271382U;
        static const AkUniqueID FOLEYS_TILE_RESIST = 1448405397U;
        static const AkUniqueID FOLEYS_WEAPON_RELOAD = 1212767844U;
        static const AkUniqueID FOLEYS_WEAPON_SHOOT = 917610224U;
        static const AkUniqueID FOLEYS_WEAPON_TRIGGER = 672917745U;
        static const AkUniqueID MUSIC_MAIN = 1615767906U;
        static const AkUniqueID PLAY_MUSICTEST = 1414448543U;
        static const AkUniqueID UI_MENU_SELECT = 3474566310U;
        static const AkUniqueID UI_MENU_SUBMIT = 1216325478U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace DESTRUCTIONLEVEL
        {
            static const AkUniqueID GROUP = 1381176751U;

            namespace STATE
            {
                static const AkUniqueID LEVEL_1 = 1290008369U;
                static const AkUniqueID LEVEL_2 = 1290008370U;
                static const AkUniqueID LEVEL_3 = 1290008371U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace DESTRUCTIONLEVEL

        namespace MAIN
        {
            static const AkUniqueID GROUP = 3161908922U;

            namespace STATE
            {
                static const AkUniqueID END = 529726532U;
                static const AkUniqueID INTRODUCTION = 1034344105U;
                static const AkUniqueID LEVEL_01 = 987635873U;
                static const AkUniqueID LEVEL_02 = 987635874U;
                static const AkUniqueID LEVEL_03 = 987635875U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace MAIN

        namespace OVERRALAMB
        {
            static const AkUniqueID GROUP = 4173766728U;

            namespace STATE
            {
                static const AkUniqueID DESTROYED = 1359166010U;
                static const AkUniqueID NATURE = 3501062356U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace OVERRALAMB

        namespace WEAPONLEVEL
        {
            static const AkUniqueID GROUP = 1296044485U;

            namespace STATE
            {
                static const AkUniqueID LEVEL_00 = 987635872U;
                static const AkUniqueID LEVEL_01 = 987635873U;
                static const AkUniqueID LEVEL_02 = 987635874U;
                static const AkUniqueID LEVEL_03 = 987635875U;
                static const AkUniqueID LEVEL_04 = 987635876U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace WEAPONLEVEL

    } // namespace STATES

    namespace SWITCHES
    {
        namespace TEXTURE
        {
            static const AkUniqueID GROUP = 2460634474U;

            namespace SWITCH
            {
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID TOXIC = 1400550014U;
            } // namespace SWITCH
        } // namespace TEXTURE

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID RTPC_PLAYERHP = 4091700968U;
        static const AkUniqueID RTPC_STANDARDTILEHP = 324242700U;
        static const AkUniqueID RTPC_TOXICTILEHP = 2682751840U;
        static const AkUniqueID WEAPONCHARGE = 2807485413U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID AMB_MAIN = 3004847309U;
        static const AkUniqueID EFFECTS_MAIN = 520556019U;
        static const AkUniqueID FOLEYS_MAIN = 3906669067U;
        static const AkUniqueID MUSIC_MAIN = 1615767906U;
        static const AkUniqueID UI_MAIN = 2963962765U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AUX = 983310469U;
        static const AkUniqueID MASTER = 4056684167U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX = 393239870U;
        static const AkUniqueID SFX_AMBIANCES = 2383096572U;
        static const AkUniqueID SFX_EFFECTS = 2324388809U;
        static const AkUniqueID SFX_FOLEYS = 3183352817U;
        static const AkUniqueID SFX_UI = 3862737079U;
        static const AkUniqueID VOICES = 3313685232U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID DEFAULT_MOTION_DEVICE = 4230635974U;
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
