using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum eSceneType
    {
        App, Title, Lobby, Loading, Opening, Tutorial01, Tutorial02, Tutorial03, Tutorial04, Tutorial05, Stage06, Stage07, Stage08, Pond, Mine, Dungeon, DreamMine, FairyShop
    }

    //플레이어
    public enum ePlayerLocation
    {
        None = -1, Plain, Pond, Dungeon, Farm, Forest, Mine, DreamMine, Dreamland
    }

    public enum ePlayerState
    {
        None = -1, Idle, Run, TwoHandEquipe_Idle, TwoHandEquipe_Run, OneHandEquipe_Idle, OneHandEquipe_Run, Sword_Idle, Sword_Run, Felling, Felling_Bee, Farming, Farming_Poison, Fishing, Mining, Attacking, Attacking_Stun, Attacking_Hit
    }

    public enum eMonsterState
    {
        None=-1,Idle, Walk, Attack, Hit, Die, Track
    }

    public enum eCabbageType
    {
        Normal, Poison
    }
    public enum ePortalType
    {
        Stage, Pond, Mine, Dungeon
    }
}
