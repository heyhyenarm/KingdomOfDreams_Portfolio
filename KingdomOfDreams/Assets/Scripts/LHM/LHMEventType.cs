using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHMEventType
{
    public enum eEventType
    {
        NONE = -1,
        CREATED_MAGIC_TOOL,
        UPGRADE_MAGIC_TOOL,
        GET_DREAM_PIECE,
        EXCLAIM_ICON_BOOK_ITEM,
        GET_INGREDIENT,
        CREATE_MAGIC_CIRCLE,
        GET_BOOK_ITEM,
        REFRESH_UI_INVENTORY,
        REFRESH_UI_BOOK,
        CHECK_MAGICTOOL_LEVEL,
        REFRESH_UI_MAGICTOOL,
        MISSION_UPDATE,
        ACHIEVE_MISSION,
        GET_POISON,
        TAKE_STUN,
        IN_DUNGEON,
        GET_HIT,
        ENTER_PORTAL,
        APPEAR_BEE,
        ATTACKED_BEE,
        CABBAGE_INACTIVE,
        END_REVIVE_VIDEO,
        SHOW_PRODUCTION_CHAIN,
        GO_DREAMLAND,
        COMBINE_INGREDIENT,
        ENTER_FAIRYSHOP,
        EXIT_FAIRYSHOP,
        CHARACTER_MATERIAL_CHANGE,
        EXIT_DUNGEON,
        EXIT_DREAMLAND,
        MONSTER_DIE,
        GIVE_OFFLINE_INGREDIENT,
        CLAIM_BOOK_ITEM, 
        USE_DREAM, 
        CHANGE_MUSIC_VOLUME, 
        CHANGE_OTHERS_VOLUME,
        END_DREAMLAND_TIME,
        CLEAR_MISSION_REFRESH,
        SHOW_OFFLINE_GIFT_EXPLAIN, 
        AD_REWARD_PLAY, 
        AD_REWARD_LOAD
    }
}
