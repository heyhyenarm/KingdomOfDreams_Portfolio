using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class InfoManager
{
    public static readonly InfoManager instance = new InfoManager();

    public string missionPath = string.Format("{0}/mission_info.json", Application.persistentDataPath);
    public string ingredientPath = string.Format("{0}/ingredient_info.json", Application.persistentDataPath);
    public string stagePath = string.Format("{0}/stage_info.json", Application.persistentDataPath);
    public string playerPath = string.Format("{0}/player_info.json", Application.persistentDataPath);
    public string chestPath = string.Format("{0}/chest_info.json", Application.persistentDataPath);
    public string chargetimePath = string.Format("{0}/chargetime_info.json", Application.persistentDataPath);
    public string ticketPath = string.Format("{0}/ticket_info.json", Application.persistentDataPath);
    public string buildingPath = string.Format("{0}/building_info.json", Application.persistentDataPath);
    public string dreamPath = string.Format("{0}/dream_info.json", Application.persistentDataPath);
    public string dialoguePath = string.Format("{0}/dialogue_info.json", Application.persistentDataPath);

    public PlayerInfo PlayerInfo { get; set; }
    public List<MissionInfo> MissionInfos { get; set; }

    public List<IngredientInfo> IngredientInfos { get; set; }

    public List<StageInfo> StageInfos { get; set; }
    public List<BookInfo> BookInfos { get; set; }

    public List<BookItemInfo> BookItemInfos { get; set; }

    public List<DreamPieceInfo> DreamPieceInfo { get; set; }

    public List<MagicToolInfo> MagicToolInfo { get; set; }

    public List<ChestInfo> ChestInfos { get; set; }

    public FreeChargeTimeInfo ChargeTimeInfo { get; set; }

    public TicketInfo TicketInfo { get; set; }

    public List<BuildingInfo> BuildingInfos { get; set; }

    public DreamInfo DreamInfo { get; set; }

    public DialogueInfo DialogueInfo { get; set; }

    private InfoManager() { }
    //public ItemInfo ItemInfo { get; set; }

    public void IngredientInfoInit()
    {
        this.IngredientInfos = new List<IngredientInfo>();
        this.SaveIngredientInfos();
    }
    public void MissionInfoInit()
    {
        this.MissionInfos = new List<MissionInfo>();
        this.SaveMissionInfos();
    }
    public void StageInfoInit()
    {
        this.StageInfos = new List<StageInfo>();
        this.AddStageInfo(0);
        this.SaveStageInfos();
    }
    public void ChestInfoInit()
    {
        this.ChestInfos = new List<ChestInfo>();
        this.AddChestInfo(100, 1);
        this.AddChestInfo(101, 0);
        this.AddChestInfo(102, 0);
        this.SaveChestInfos();
    }
    public void BuildingInfoInit()
    {
        this.BuildingInfos = new List<BuildingInfo>();
        this.SaveBuildingInfos();
    }
    public void TicketInfoInit()
    {
        this.TicketInfo = new TicketInfo(5, System.DateTime.Now);
        this.SaveTicketInfo();
    }

    public bool IsNewbie(string path)
    {
        //string path = string.Format("{0}/mission_info.json", Application.persistentDataPath);
        //Debug.LogFormat("<color=cyan>{0}</color>", path);

        //mission_info.json �ִ��� �Ǻ� 
        bool existFile = File.Exists(path);
        //Debug.LogFormat("Exists: {0}", existFile);
        return !existFile;
    }

    public IngredientInfo GetIngredientInfo(int id)
    {
        //id�� ��� id
        //Debug.LogFormat("GetIngredientInfo id:{0}", id);
        var info = InfoManager.instance.IngredientInfos.Find(x => x.id == id);
        return info;
    }

    public MissionInfo GetMissionInfo(int id)
    {
        //id�� �̼� id
        //Debug.LogFormat("GetMissionInfo id:{0}", id);
        var info = InfoManager.instance.MissionInfos.Find(x => x.id == id);
        return info;
    }


    public void AddMissionInfo(int missionId)
    {
        //Debug.LogFormat("addmissioninfo");
        MissionInfo missionInfo = new MissionInfo(missionId);
        InfoManager.instance.MissionInfos.Add(missionInfo);
    }

    public void SaveMissionInfos()
    {
        //var path = string.Format("{0}/mission_info.json", Application.persistentDataPath);
        //����ȭ 
        var json = JsonConvert.SerializeObject(this.MissionInfos);

        //Debug.Log("---------------------");
        //Debug.Log(json);
        //Debug.Log("---------------------");

        //���Ϸ� ���� 
        File.WriteAllText(missionPath, json);
        //Debug.Log("<color=yellow>[save success] mission_info.json</color>");
    }

    public void LoadMissionInfo()
    {
        //var path = string.Format("{0}/mission_info.json", Application.persistentDataPath);
        var json = File.ReadAllText(missionPath);
        //������ȭ 
        this.MissionInfos = JsonConvert.DeserializeObject<List<MissionInfo>>(json);
        //Debug.Log("<color=yellow>[load success] mission_info.json</color>");

    }

    public void SaveDialogueInfo()
    {
        var json = JsonConvert.SerializeObject(this.DialogueInfo);
        //Debug.LogFormat("<color=red>dialogue info:{0}</color>", json);
        File.WriteAllText(dialoguePath, json);
        //Debug.Log("<color=yellow>[save success] dialogue_info.json</color>");
    }
    public void LoadDialogueInfo()
    {
        var json = File.ReadAllText(dialoguePath);
        this.DialogueInfo = JsonConvert.DeserializeObject<DialogueInfo>(json);
        //Debug.Log("<color=yellow>[load success] dialogue_info.json</color>");
    }

    public void SaveIngredientInfos()
    {
        var path = string.Format("{0}/ingredient_info.json", Application.persistentDataPath);

        var json = JsonConvert.SerializeObject(this.IngredientInfos);

        File.WriteAllText(ingredientPath, json);
        //Debug.Log("<color=yellow>[save success] ingredient_info.json</color>");
    }

    public void LoadIngredientInfos()
    {
        var path = string.Format("{0}/ingredient_info.json", Application.persistentDataPath);
        var json = File.ReadAllText(ingredientPath);
        //������ȭ 
        this.IngredientInfos = JsonConvert.DeserializeObject<List<IngredientInfo>>(json);
        //Debug.Log("<color=yellow>[load success] ingredient_info.json</color>");

    }
    public void LoadPlayerInfo()
    {
        var json = File.ReadAllText(playerPath);
        //������ȭ 
        this.PlayerInfo = JsonConvert.DeserializeObject<PlayerInfo>(json);
        //Debug.Log("<color=yellow>[load success] player_info.json</color>");
    }

    public void SavePlayerInfo()
    {
        var json = JsonConvert.SerializeObject(this.PlayerInfo);
        File.WriteAllText(playerPath, json);
        //Debug.Log("<color=yellow>[save success] player_info.json</color>");
    }

    //�������� ����
    public void SaveStageInfos()
    {
        var json = JsonConvert.SerializeObject(this.StageInfos);

        File.WriteAllText(stagePath, json);
        //Debug.Log("<color=yellow>[save success] stage_info.json</color>");
    }
    public void LoadStageInfo()
    {
        var json = File.ReadAllText(stagePath);
        //������ȭ 
        this.StageInfos = JsonConvert.DeserializeObject<List<StageInfo>>(json);
        //Debug.Log("<color=yellow>[load success] stage_info.json</color>");
        //Debug.Log(json);
    }
    public void AddStageInfo(int id)
    {
        StageInfo stageInfo = new StageInfo(id, false);
        InfoManager.instance.StageInfos.Add(stageInfo);
        //Debug.Log("<color=green>AddStageInfo success");
    }
    
    //�� ����
    public void SaveDreamInfo()
    {
        var json = JsonConvert.SerializeObject(this.DreamInfo);
        File.WriteAllText(dreamPath, json);
        //Debug.Log("<color=yellow>[save success] dream_info.json</color>");
    }
    public void LoadDreamInfo()
    {
        var json = File.ReadAllText(dreamPath);
        //������ȭ 
        this.DreamInfo = JsonConvert.DeserializeObject<DreamInfo>(json);
        //Debug.Log("<color=yellow>[load success] dream_info.json</color>");
        //Debug.Log(json);
    }
    public int GetDreamInfo()
    {
        var dreamInfo = InfoManager.instance.DreamInfo;
        return dreamInfo.amount;
    }
    //�帲����
    public void DreamAcount(int amount)
    {
        this.DreamInfo.amount += amount;
        this.SaveDreamInfo();
    }

    //���� ���� ����
    public void SaveBookInfo()
    {
        var path = string.Format("{0}/book_info.json", Application.persistentDataPath);

        var json = JsonConvert.SerializeObject(this.BookInfos);

        File.WriteAllText(path, json);
        //Debug.Log("<color=yellow>[save success] book_info.json</color>");
    }
    //���� �ε�
    public void LoadBookInfo()
    {
        var path = string.Format("{0}/book_info.json", Application.persistentDataPath);
        string json = File.ReadAllText(path);

        //������ȭ
        var bookInfo = JsonConvert.DeserializeObject<List<BookInfo>>(json);

        //Debug.LogFormat("BookInfos: {0}", bookInfo.Count);

        //InfoManager�� ����
        InfoManager.instance.BookInfos = bookInfo;

        //Debug.Log("<color=yellow>[load success] book_info.json</color>");

    }
    //���� ������ ���� ����
    public void SaveBookItemInfo()
    {
        var path = string.Format("{0}/book_item_info.json", Application.persistentDataPath);

        var json = JsonConvert.SerializeObject(this.BookItemInfos);

        File.WriteAllText(path, json);
        //Debug.Log("<color=yellow>[save success] book_item_info.json</color>");
    }
    //���� ������ �ε�
    public void LoadBookItemInfo()
    {
        var path = string.Format("{0}/book_item_info.json", Application.persistentDataPath);
        string json = File.ReadAllText(path);

        //������ȭ
        var bookItemInfo = JsonConvert.DeserializeObject<List<BookItemInfo>>(json);

        //Debug.LogFormat("BookitemInfos: {0}", bookItemInfo.Count);

        //InfoManager�� ����
        InfoManager.instance.BookItemInfos = bookItemInfo;

        //Debug.Log("<color=yellow>[load success] book_item_info.json</color>");
    }
    //�� ���� ���� ����
    public void SaveDreamPieceInfo()
    {
        var path = string.Format("{0}/dream_piece_info.json", Application.persistentDataPath);

        var json = JsonConvert.SerializeObject(this.DreamPieceInfo);

        File.WriteAllText(path, json);
        //Debug.Log("<color=yellow>[save success] dream_piece_info.json</color>");
    }
    //�� ���� �ε�
    public void LoadDreamPieceInfo()
    {
        var path = string.Format("{0}/dream_piece_info.json", Application.persistentDataPath);
        string json = File.ReadAllText(path);

        //������ȭ
        var dreamPieceInfo = JsonConvert.DeserializeObject<List<DreamPieceInfo>>(json);

        //Debug.LogFormat("DreamPieceInfos: {0}", dreamPieceInfo.Count);

        //InfoManager�� ����
        InfoManager.instance.DreamPieceInfo = dreamPieceInfo;

        //Debug.Log("<color=yellow>[load success] dream_piece_info.json</color>");
    }
    //���� ���� ����
    public void SaveMagicToolInfo()
    {
        var path = string.Format("{0}/magicTool_info.json", Application.persistentDataPath);

        var json = JsonConvert.SerializeObject(this.MagicToolInfo);

        File.WriteAllText(path, json);
        //Debug.Log("<color=yellow>[save success] magicTool_info.json</color>");
    }

    //Chest Info
    public void SaveChestInfos()
    {
        var json = JsonConvert.SerializeObject(this.ChestInfos);
        File.WriteAllText(chestPath, json);
        //Debug.Log("<color=yellow>[save success] chest_info.json</color>");
    }
    public void LoadChestInfo()
    {
        var json = File.ReadAllText(chestPath);
        //������ȭ 
        this.ChestInfos = JsonConvert.DeserializeObject<List<ChestInfo>>(json);
        //Debug.Log("<color=yellow>[load success] chest_info.json</color>");
        //Debug.Log(json);
    }
    private void AddChestInfo(int id, int amount)
    {
        ChestInfo chestInfo = new ChestInfo(id, amount);
        InfoManager.instance.ChestInfos.Add(chestInfo);
        //Debug.Log("<color=green>AddChestInfo success");
    }
    public ChestInfo GetChestInfo(int id)
    {
        var chestInfo = InfoManager.instance.ChestInfos[id - 100];
        return chestInfo;
    }
    //ChargeTime Info
    public void SaveChargeTimeInfo()
    {
        var json = JsonConvert.SerializeObject(this.ChargeTimeInfo);
        File.WriteAllText(chargetimePath, json);
        //Debug.Log("<color=yellow>[save success] chargetime_info.json</color>");
    }
    public void LoadChargeTimeInfo()
    {
        var json = File.ReadAllText(chargetimePath);
        //������ȭ 
        this.ChargeTimeInfo = JsonConvert.DeserializeObject<FreeChargeTimeInfo>(json);
        //Debug.Log("<color=yellow>[load success] chargetime_info.json</color>");
        //Debug.Log(json);
    }
    public void RefreshChargeTimeInfo(System.DateTime time, bool AdOn)
    {
        FreeChargeTimeInfo chargeTimeInfo = new FreeChargeTimeInfo(time, AdOn);
        InfoManager.instance.ChargeTimeInfo = chargeTimeInfo;
        //Debug.Log("<color=green>AddChargeTimeInfo success");
    }
    public System.DateTime GetChargeTime()
    {
        var chargeTime = InfoManager.instance.ChargeTimeInfo.chestChargeStartTime;
        return chargeTime;
    }
    //Ticket Info
    public void SaveTicketInfo()
    {
        var json = JsonConvert.SerializeObject(this.TicketInfo);
        File.WriteAllText(ticketPath, json);
        //Debug.Log("<color=yellow>[save success] ticket_info.json</color>");
    }
    public void LoadTicketInfo()
    {
        var json = File.ReadAllText(ticketPath);
        //������ȭ 
        this.TicketInfo = JsonConvert.DeserializeObject<TicketInfo>(json);
        //Debug.Log("<color=yellow>[load success] ticket_info.json</color>");
        //Debug.Log(json);
    }
    public void UseTicket(System.DateTime time)
    {
        InfoManager.instance.TicketInfo.ticketAmount--;
        InfoManager.instance.TicketInfo.ticketChargeStartTime = time;
        InfoManager.instance.SaveTicketInfo();
        //Debug.Log("<color=green>Use ticket success</color>");
    }
    public void GetTicket(System.DateTime time)
    {
        InfoManager.instance.TicketInfo.ticketAmount++;
        InfoManager.instance.TicketInfo.ticketChargeStartTime = time;
        InfoManager.instance.SaveTicketInfo();
        //Debug.Log("<color=green>Use ticket success</color>");
    }
    public TicketInfo GetTicketInfo()
    {
        var ticketInfo = InfoManager.instance.TicketInfo;
        return ticketInfo;
    }


    //���� ���� �ε�
    public void LoadMagicToolInfo()
    {
        var path = string.Format("{0}/magicTool_info.json", Application.persistentDataPath);
        string json = File.ReadAllText(path);

        //������ȭ
        var magicToolInfo = JsonConvert.DeserializeObject<List<MagicToolInfo>>(json);

        //Debug.LogFormat("MgaicToolInfos: {0}", magicToolInfo.Count);

        //InfoManager�� ����
        InfoManager.instance.MagicToolInfo = magicToolInfo;

        //Debug.Log("<color=yellow>[load success] magicTool_info.json</color>");
    }
    //���� ���� ����
    public void SaveBuildingInfos()
    {
        var json = JsonConvert.SerializeObject(this.BuildingInfos);
        File.WriteAllText(this.buildingPath, json);
        //Debug.Log("<color=yellow>[save success] building_info.json</color>");
    }
    //���� ���� �ε�
    public void LoadBuildingInfos()
    {
        string json = File.ReadAllText(this.buildingPath);
        //������ȭ
        this.BuildingInfos = JsonConvert.DeserializeObject<List<BuildingInfo>>(json);
        //Debug.LogFormat("BuildingInfos: {0}", this.BuildingInfos.Count);
        //Debug.Log("<color=yellow>[load success] building_info.json</color>");
    }
}
