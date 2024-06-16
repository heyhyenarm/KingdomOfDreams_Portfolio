using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using System.Linq;

public class DataManager
{
    public static readonly DataManager instance = new DataManager();

    private DataManager() { }

    //컬렉션 초기화 (링큐 사용 안할때)
    private Dictionary<int, MissionData> dicMissionData;
    private Dictionary<int, DialogueData> dicDialogueData;
    private Dictionary<int, IngredientData> dicIngredientData;
    private Dictionary<int, StageData> dicStageData;
    private Dictionary<int, BuildingData> dicBuildingData;
    private Dictionary<int, MissionData> dicTestMissionData;
    public Dictionary<int, BookData> dicBookData;
    public Dictionary<int, BookItemData> dicBookItemData;
    public Dictionary<int, DreamPieceData> dicDreamPieceData;
    public Dictionary<int, MagicToolData> dicMagicToolData;
    public Dictionary<int, MagicToolLevelData> dicMagicToolLevelData;
    public Dictionary<int, ChestData> dicChestData;
    private Dictionary<int, NpcData> dicNpcData;

    public string realMissionDataPath = "Datas/mission_data";
    public string testMisisonDataPath = "Datas/test_data";
    public string missionDataPath;

    public NpcData GetNpcData(int id)
    {
        //Debug.LogFormat("GetNpcData : {0}", id);
        return this.dicNpcData[id];
    }
    public void LoadNpcData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/npc_data");

        var json = asset.text;
        Debug.LogFormat("<color=red>npd data load:{0}</color>", json);

        //역직렬화
        NpcData[] npcDatas = JsonConvert.DeserializeObject<NpcData[]>(json);

        this.dicNpcData = npcDatas.ToDictionary(x => x.id);
        //Debug.LogFormat("npc data loaded : {0}", this.dicNpcData);
    }


    //ChestData
    public ChestData GetChestData(int id)
    {
        //Debug.LogFormat("GetChestData : {0}", id);
        return this.dicChestData[id];
    }
    public void LoadChestData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/Chest_data");

        var json = asset.text;

        //역직렬화
        ChestData[] chestDatas = JsonConvert.DeserializeObject<ChestData[]>(json);

        this.dicChestData = chestDatas.ToDictionary(x => x.id);
        //Debug.LogFormat("chest data loaded : {0}", this.dicChestData);
    }

    //StageData
    public StageData GetStageData(int id)
    {
        //Debug.LogFormat("GetStageData : {0}", id);
        return this.dicStageData[id];
    }
    public void LoadStageData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/Stage_data");

        var json = asset.text;

        //역직렬화
        StageData[] stageDatas = JsonConvert.DeserializeObject<StageData[]>(json);

        this.dicStageData = stageDatas.ToDictionary(x => x.id);
        //Debug.LogFormat("stage data loaded : {0}", this.dicStageData);
    }


    public IngredientData GetIngredientData(int id)
    {
        //Debug.LogFormat("GetIngredientData:{0}", id);
        return this.dicIngredientData[id];
    }

    public void LoadIngredientData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/LHMDatas/Titem_data");

        var json = asset.text;
        //Debug.Log(json);

        //역직렬화 
        IngredientData[] arrItemDatas = JsonConvert.DeserializeObject<IngredientData[]>(json);

        this.dicIngredientData = arrItemDatas.ToDictionary(x => x.id);    //새로운 사전 객체가 생성 반환 
        Debug.LogFormat("item data loaded : {0}", this.dicIngredientData.Count);
    }


    public DialogueData GetDialogueData(int id)
    {
        return this.dicDialogueData[id];
    }

    public void LoadDialogueData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/HSYDatas/dialogue_data");

        var json = asset.text;
        Debug.Log(json);

        //역직렬화 
        DialogueData[] arrDialogueDatas = JsonConvert.DeserializeObject<DialogueData[]>(json);

        this.dicDialogueData = arrDialogueDatas.ToDictionary(x => x.id);    //새로운 사전 객체가 생성 반환 
        //Debug.LogFormat("dialogue data loaded : {0}", this.dicDialogueData.Count);
    }


    public MissionData GetMissionData(int id)
    {
        //Debug.LogFormat("<color=cyan>getmissiondata({0})</color>", id);
        return this.dicMissionData[id];
    }

    public void LoadMissionData()
    {
        TextAsset asset = Resources.Load<TextAsset>(this.realMissionDataPath);

        //TextAsset asset = Resources.Load<TextAsset>("Datas/HSYDatas/mission_data");

        var json = asset.text;
        Debug.Log(json);

        //역직렬화 
        MissionData[] arrMissionDatas = JsonConvert.DeserializeObject<MissionData[]>(json);

        this.dicMissionData = arrMissionDatas.ToDictionary(x => x.id);    //새로운 사전 객체가 생성 반환 
        Debug.LogFormat("mission data loaded : {0}", this.dicMissionData.Count);
    }

    public List<MissionData> GetMissionDatas()
    {
        Debug.LogFormat("missiondataCount:{0}", this.dicMissionData.Values.ToList().Count);
        return this.dicMissionData.Values.ToList();
    }
    public BuildingData GetBuildingData(int id)
    {
        return this.dicBuildingData[id];
    }
    public void LoadBuildingData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/building_data");

        var json = asset.text;

        BuildingData[] arrBuldingDatas = JsonConvert.DeserializeObject<BuildingData[]>(json);

        this.dicBuildingData = arrBuldingDatas.ToDictionary(x => x.id);
        //Debug.LogFormat("building_data count: {0}", this.dicBuildingData.Count);
    }

    //도감 데이터
    public BookData GetBookData(int id)
    {
        return this.dicBookData[id];
    }

    public void LoadBookData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/LHMDatas/book_data");
        var json = asset.text;
        Debug.Log(json);

        BookData[] arrItemDatas = JsonConvert.DeserializeObject<BookData[]>(json);

        this.dicBookData = arrItemDatas.ToDictionary(x => x.id);

        Debug.LogFormat("book data loaded: {0}", this.dicBookData.Count);
    }

    public List<BookData> GetBookDatas()
    {
        return this.dicBookData.Values.ToList();
    }

    //도감 아이템 데이터
    public BookItemData GetBookItemData(int id)
    {
        return this.dicBookItemData[id];
    }

    public void LoadBookItemData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/LHMDatas/book_item_data");
        var json = asset.text;
        Debug.Log(json);

        BookItemData[] arrItemDatas = JsonConvert.DeserializeObject<BookItemData[]>(json);

        this.dicBookItemData = arrItemDatas.ToDictionary(x => x.id);

        //Debug.LogFormat("TbookItem data loaded: {0}", this.dicBookItemData.Count);
    }

    public List<BookItemData> GetBookItemDatas()
    {
        return this.dicBookItemData.Values.ToList();
    }

    //마법조각 데이터
    public DreamPieceData GetDreamPieceData(int id)
    {
        return this.dicDreamPieceData[id];
    }

    public void LoadDreamPieceData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/LHMDatas/dream_piece_data");
        var json = asset.text;
        //Debug.Log(json);

        DreamPieceData[] arrItemDatas = JsonConvert.DeserializeObject<DreamPieceData[]>(json);

        this.dicDreamPieceData = arrItemDatas.ToDictionary(x => x.id);

        //Debug.LogFormat("DreamPiece data loaded: {0}", this.dicDreamPieceData.Count);

    }

    public List<DreamPieceData> GetDreamPieceDatas()
    {
        return this.dicDreamPieceData.Values.ToList();
    }

    //마법도구 데이터
    public MagicToolData GetMagicToolData(int id)
    {
        return this.dicMagicToolData[id];
    }

    public void LoadMagicToolData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/LHMDatas/magicTool_data");
        var json = asset.text;
        Debug.Log(json);

        MagicToolData[] arrItemDatas = JsonConvert.DeserializeObject<MagicToolData[]>(json);

        this.dicMagicToolData = arrItemDatas.ToDictionary(x => x.id);

        //Debug.LogFormat("TMagicTool data loaded: {0}", this.dicMagicToolData.Count);
    }

    public List<MagicToolData> GetMagicToolDatas()
    {
        return this.dicMagicToolData.Values.ToList();
    }

    //마법도구 레벨데이터
    public MagicToolLevelData GetMagicToolLevelData(int id)
    {
        return this.dicMagicToolLevelData[id];
    }

    public void LoadMagicToolLevelData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/LHMDatas/magicTool_level_data");
        var json = asset.text;
        //Debug.Log(json);

        MagicToolLevelData[] arrItemDatas = JsonConvert.DeserializeObject<MagicToolLevelData[]>(json);

        this.dicMagicToolLevelData = arrItemDatas.ToDictionary(x => x.id);

        //Debug.LogFormat("TMagicToolLevel data loaded: {0}", this.dicMagicToolLevelData.Count);
    }

    public List<MagicToolLevelData> GetMagicToolLevelDatas()
    {
        return this.dicMagicToolLevelData.Values.ToList();
    }

}
