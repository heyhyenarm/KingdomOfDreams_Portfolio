using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    private DateTime lastGiftTime;
    private DateTime offlineTime;
    private int giftIntervalInMinutes = 1;
    public TimeSpan offlineTimeSpan;
    private string ExitTimeKey = "ExitTime";

    private float timer;  // 타이머 변수
    private float interval = 300f;  // 아이템 지급 간격 (5분 = 300초)
    //테스트스테이지번호
    private int stageNum;


    private void Start()
    {
        Debug.Log("<color=yellow> 인벤토리 Start</color>");
        string.IsNullOrEmpty(DataManager.instance.GetNpcData(3005).prefab_name);
        // 게임을 시작할 때 마지막으로 아이템을 준 시간을 현재 시간으로 초기화
        lastGiftTime = DateTime.Now;

        foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
        {
            Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
            if (stageInfo.isClear == false)
            {
                //원하는곳에 stageInfo.num 저장
                this.stageNum = stageInfo.stage;
                break;
            }
        }

        //if (stageNum >= 5) //6스테이지부터 오프라인 자동공급
        //{
        //    this.OfflineAutoGift();
        //}
    }

    public void Init()
    {
        Debug.Log("<color=yellow> 인벤토리 init</color>");

        //재료 획득 이벤트 처리
        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.GET_INGREDIENT, this.GetIngredientEventHandler);

        //조합하기 이벤트 초기화
        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.COMBINE_INGREDIENT, this.CombineIngredientEventHandler);

        EventDispatcher.instance.AddEventHandler<DateTime>((int)LHMEventType.eEventType.GIVE_OFFLINE_INGREDIENT, this.GiveOfflineIngredientEventHandler);


    }

    private void GetIngredientEventHandler(short type, int a)
    {
        //Debug.LogFormat("<color=red>GET_INGREDIENT 실행</color>");

        //this.GetIngredient(a);

        //마법도구 레벨에 따른 재료 획득 수량 증가
        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;
        if (myLevel == 0)
        {
            this.GetIngredient(a);
        }
        else if (myLevel >= 1)
        {
            var data = DataManager.instance.GetMagicToolLevelDatas().Find(x => x.level == myLevel);
            for (int i = 0; i < data.add_magic_property; i++)
            {
                this.GetIngredient(a);
            }
        }
        //EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.MISSION_UPDATE,a);

        //튜토리얼 도감 획득
        if (a == 2004 && InfoManager.instance.IngredientInfos.Find(x => x.id == 2004).amount == 1)
        {

        }
        else if (a == 2004 && InfoManager.instance.IngredientInfos.Find(x => x.id == 2004).amount == 2)
        {
            EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_BOOK_ITEM, 6000);
        }

        //나머지 도감 획득
        //float normalItemProb = 0.14f;
        
        //테스트
        float normalItemProb = 0.5f;
        float rareItemProb = 0.08f; //0.08

        int[] actions = new int[] { 2000, 2001, 2002, 2003, 2004 };

        foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
        {
            Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
            if (stageInfo.isClear == false)
            {
                //원하는곳에 stageInfo.num 저장
                this.stageNum = stageInfo.stage;
                break;
            }
        }

        //6스테이지 이상일때
        if (this.stageNum >= 5)
        {
            foreach (int action in actions)
            {
                int itemAmount = InfoManager.instance.IngredientInfos.Find(x => x.id == action).amount;
                int magicToolLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;

                if (a == action && magicToolLevel == 0)//마법도구X일때 도감 획득하기
                {
                    float randomGetValue = Random.value;
                    if (randomGetValue <= normalItemProb) //노멀아이템 획득하기
                    {
                        int randomItemValue = 0;
                        switch (action)
                        {
                            case 2000:
                                randomItemValue = Random.Range(6002, 6005);
                                break;
                            case 2001:
                                randomItemValue = Random.Range(6009, 6012);
                                break;
                            case 2002:
                                randomItemValue = Random.Range(6016, 6019);
                                break;
                            case 2003:
                                randomItemValue = Random.Range(6023, 6026);
                                break;
                            case 2004:
                                randomItemValue = Random.Range(6030, 6033);
                                break;
                        }
                        EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_BOOK_ITEM, randomItemValue);
                    }
                    else // 도감 아이템 못 얻음
                    {
                        Debug.Log("No item acquired");
                    }
                }
                else if (a == action && magicToolLevel >= 1)//마법도구O(존재)일때 도감 획득하기
                {
                    var wisdom = InfoManager.instance.MagicToolInfo.Find(x => x.id == 330);
                    var data = DataManager.instance.GetMagicToolLevelDatas().Find(x => x.level == wisdom.level);
                    float plusProb = data.add_wisdom_property * 0.01f;

                    float randomGetValue = Random.value;
                    int randomItemValue = 0;
                    if (randomGetValue <= normalItemProb) // 14% 확률로 노멀 아이템을 획득
                    {
                        switch (action)
                        {
                            case 2000:
                                randomItemValue = Random.Range(6002, 6005);
                                break;
                            case 2001:
                                randomItemValue = Random.Range(6009, 6012);
                                break;
                            case 2002:
                                randomItemValue = Random.Range(6016, 6019);
                                break;
                            case 2003:
                                randomItemValue = Random.Range(6023, 6026);
                                break;
                            case 2004:
                                randomItemValue = Random.Range(6030, 6033);
                                break;
                        }
                        EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_BOOK_ITEM, randomItemValue);
                    }
                    else if (randomGetValue > normalItemProb && randomGetValue <= normalItemProb + rareItemProb + plusProb) // 지혜 레벨 확률로 레어 아이템을 획득
                    {
                        Debug.LogFormat("레어 아이템 {0}%로 나옴", rareItemProb + plusProb);
                        switch (action)
                        {
                            case 2000:
                                randomItemValue = Random.Range(6005, 6009);
                                break;
                            case 2001:
                                randomItemValue = Random.Range(6012, 6016);
                                break;
                            case 2002:
                                randomItemValue = Random.Range(6019, 6023);
                                break;
                            case 2003:
                                randomItemValue = Random.Range(6026, 6030);
                                break;
                            case 2004:
                                randomItemValue = Random.Range(6033, 6037);
                                break;
                        }
                        EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_BOOK_ITEM, randomItemValue);
                    }
                    else // 도감 아이템 못 얻음
                    {
                        Debug.Log("No item acquired");
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler<int>((int)LHMEventType.eEventType.COMBINE_INGREDIENT, this.CombineIngredientEventHandler);
        EventDispatcher.instance.RemoveEventHandler<DateTime>((int)LHMEventType.eEventType.GIVE_OFFLINE_INGREDIENT, this.GiveOfflineIngredientEventHandler);
        EventDispatcher.instance.RemoveEventHandler<int>((int)LHMEventType.eEventType.GET_INGREDIENT, this.GetIngredientEventHandler);

    }

    private void Update()
    {        
        if (stageNum >= 5) //6스테이지부터 자동공급
        {
            // 시간 업데이트
            timer += Time.deltaTime;
            // 아이템 지급
            if (timer >= interval)
            {
                timer = 0f;  // 타이머 초기화

                // 아이템 생성
                Debug.Log("<color=#FFB6C1>자동공급 호출</color>");
                GiveGift();

            }
        }

    }
    
    private void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            // 게임 종료 시 현재 시간을 저장합니다.
            DateTime currentExitTime = DateTime.Now;
            string exitTimeStr = Convert.ToString(currentExitTime.ToBinary());

            PlayerPrefs.SetString(ExitTimeKey, exitTimeStr);
            PlayerPrefs.Save();

            Debug.Log("종료 시간 저장: " + currentExitTime.ToString());
        }
    }
    private void OnApplicationQuit()
    {
        // 게임 종료 시 현재 시간을 저장합니다.
        DateTime currentExitTime = DateTime.Now;
        string exitTimeStr = Convert.ToString(currentExitTime.ToBinary());

        PlayerPrefs.SetString(ExitTimeKey, exitTimeStr);
        PlayerPrefs.Save();

        Debug.Log("종료 시간 저장: " + currentExitTime.ToString());
    }

    private void GiveOfflineIngredientEventHandler(short type, DateTime time)
    {
        this.OfflineAutoGift(time);
    }

    public void OfflineAutoGift(DateTime lastTime)
    {
        // 오프라인시간과 현재시간 차 계산
        var currentTime = DateTime.Now;

        TimeSpan spanStart = new TimeSpan(lastTime.Day, lastTime.Hour, lastTime.Minute, lastTime.Second);
        TimeSpan spanEnd = new TimeSpan(currentTime.Day, currentTime.Hour, currentTime.Minute, currentTime.Second);
        TimeSpan spanGap = spanEnd.Subtract(spanStart);

        //this.offlineTimeSpan = spanGap;

        Debug.Log("lastTIme: " + lastTime);
        Debug.Log("currentTime: " + currentTime);
        Debug.Log("spanGap: " + spanGap);

        TimeSpan twoHours = new TimeSpan(2, 0, 0);
        int maxGiftCount = (int)(spanGap.TotalMinutes / 5); // 5분마다 1번씩 GiveGift() 메서드를 실행하기 때문에 최대 실행 횟수를 계산
        
        Debug.LogFormat("<color=yellowr>{0}</color>", maxGiftCount);

        // 2시간 이상 지난 경우, 2시간에 해당하는 GiveGift() 메서드를 실행
        if (spanGap >= twoHours)
        {
            Debug.Log("<color=yellowr>2시간에 해당하는 오프라인 지급");
            for (int i = 0; i < 24; i++) // 2시간에 해당하는 24번의 실행 횟수를 지정
            {
                OfflineGiveGift();
            }
        }
        // 2시간 미만인 경우, spanGap에 해당하는 GiveGift() 메서드를 실행
        else
        {
            Debug.Log("<color=yellowr>2시간 미만 오프라인 지급");
            for (int i = 0; i < maxGiftCount; i++)
            {
                OfflineGiveGift();
            }
        }

        Debug.Log("<color=pink>오프라인설명창 이벤트</color>");
        EventDispatcher.instance.SendEvent<TimeSpan>((int)LHMEventType.eEventType.SHOW_OFFLINE_GIFT_EXPLAIN, spanGap);
    }
    private void OfflineGiveGift()
    {
        // 아이템을 주는 코드
        Debug.Log("<color=yellow>자동공급실행</color>");

        int[] ingredientIds = { 2000, 2001, 2002, 2003, 2004 };

        foreach (int id in ingredientIds)
        {
            this.GetIngredient(id);
        }

    }

    private void GiveGift()
    {
        // 아이템을 주는 코드
        Debug.Log("<color=yellow>자동공급실행</color>");

        int[] ingredientIds = { 2000, 2001, 2002, 2003, 2004 };

        foreach (int id in ingredientIds)
        {
            int autoCount = InfoManager.instance.IngredientInfos.Find(x => x.id == id)?.auto ?? 0;

            for (int i = 0; i < autoCount; i++)
            {
                this.GetIngredient(id);
            }
        }

    }
    private void CombineIngredientEventHandler(short type, int num)
    {
        this.GetIngredient(num);
    }

    public void GetIngredient(int num)
    {
        //Debug.LogFormat("<color=red>GetIngredient 실행</color>");
        var data = DataManager.instance.GetIngredientData(num);

        var id = data.id;
        var foundInfo = InfoManager.instance.IngredientInfos.Find(x => x.id == id);

        if (foundInfo == null)
        {
            IngredientInfo info = new IngredientInfo(id);
            InfoManager.instance.IngredientInfos.Add(info);
            if (info.id == 2004)
            {
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_BOOK_ITEM, 6001);
            }
        }
        else
        {
            foundInfo.amount++;

            //Debug.Log("<color=red>인벤토리에서 증가시킴</color>");
        }

        InfoManager.instance.SaveIngredientInfos();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_INVENTORY);

        this.UpdateMission(num);
    }

    private void UpdateMission(int ingredientId)
    {
        foreach (MissionInfo missionInfo in InfoManager.instance.MissionInfos)
        {
            if (!missionInfo.isClear)
            {
                var data = DataManager.instance.GetMissionData(missionInfo.id);
                //Debug.LogFormat("<color=magenta>a:{0}, data.request_ingredient_id0:{1}</color>", a, data.request_ingredient_id0);
                //Debug.LogFormat("<color=magenta>a:{0}, data.request_ingredient_id1:{1}</color>", a, data.request_ingredient_id1);
                //Debug.LogFormat("<color=magenta>a:{0}, data.request_ingredient_id2:{1}</color>", a, data.request_ingredient_id2);
                //Debug.LogFormat("<color=magenta>a:{0}, data.request_ingredient_id3:{1}</color>", a, data.request_ingredient_id3);
                //Debug.LogFormat("<color=magenta>a:{0}, data.request_ingredient_id4:{1}</color>", a, data.request_ingredient_id4);

                if (ingredientId == data.request_ingredient_id0)
                {
                    //Debug.LogFormat("<color=yellow>{0} mission update +{1}</color>", data.id, ingredientId);
                    EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.MISSION_UPDATE,ingredientId);
                    //EventManager.instance.onMissionUpdate(ingredientId);
                    break;
                }
                if (ingredientId == data.request_ingredient_id1)
                {
                    //Debug.LogFormat("<color=yellow>{0} mission update +{1}</color>", data.id, ingredientId);
                    EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.MISSION_UPDATE, ingredientId);
                    //EventManager.instance.onMissionUpdate(ingredientId);
                    break;
                }
                if (ingredientId == data.request_ingredient_id2)
                {
                    //Debug.LogFormat("<color=yellow>{0} mission update +{1}</color>", data.id, ingredientId);
                    EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.MISSION_UPDATE, ingredientId);
                    //EventManager.instance.onMissionUpdate(ingredientId);
                    break;
                }
                if (ingredientId == data.request_ingredient_id3)
                {
                    //Debug.LogFormat("<color=yellow>{0} mission update +{1}</color>", data.id, ingredientId);
                    EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.MISSION_UPDATE, ingredientId);
                    //EventManager.instance.onMissionUpdate(ingredientId);
                    break;
                }
                if (ingredientId == data.request_ingredient_id4)
                {
                    //Debug.LogFormat("<color=yellow>{0} mission update +{1}</color>", data.id, ingredientId);
                    EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.MISSION_UPDATE, ingredientId);
                    //EventManager.instance.onMissionUpdate(ingredientId);
                    break;
                }
            }
        }
    }
}
