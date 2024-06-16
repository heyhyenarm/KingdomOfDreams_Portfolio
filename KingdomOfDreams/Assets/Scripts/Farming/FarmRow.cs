using UnityEngine;
using System.Collections;

public class FarmRow : MonoBehaviour
{
    public Transform[] Poses;
    public FarmingCabbage cabbage;
    public int stage;
    public float regenTime=10f;
    private int poisonPercentage=10;

    public void Init()
    {
        for (int i = 0; i < Poses.Length; i++)
        {
            StartCoroutine(this.GenerateCabbage(i,0));
        }
        if (this.stage != 3)
        {
            this.AddEvent();
        }
    }

    public void OnDestroy()
    {
        // FarmRow 파괴 시 이벤트 핸들러 제거
        EventDispatcher.instance.RemoveEventHandler<FarmingCabbage>((int)LHMEventType.eEventType.CABBAGE_INACTIVE, CabbageInactiveHandler);
    }

    private void AddEvent()
    {
        // 이벤트 핸들러 등록
        EventDispatcher.instance.AddEventHandler<FarmingCabbage>((int)LHMEventType.eEventType.CABBAGE_INACTIVE, CabbageInactiveHandler);
    }

    private void CabbageInactiveHandler(short type, FarmingCabbage cabbage)
    {
        // 이벤트 핸들러 내용
        Debug.LogFormat("cabbageType:{0}", cabbage.type);
        if (cabbage.type == Enums.eCabbageType.Normal)
        {
            StartCoroutine(GenerateCabbage(cabbage.transNum, regenTime));
        }
        else
        {
            StartCoroutine(SetActiveCabbage(cabbage));
        }
    }

    private void AddEvent1()
    {
        short eventType = (short)LHMEventType.eEventType.CABBAGE_INACTIVE;
        EventDispatcher.EventHandlerListData eventHandlerList = EventDispatcher.instance.GetEventHandlerList(eventType);

        if (eventHandlerList != null)
        {
            //EventDispatcher.instance.RemoveEventHandler<FarmingCabbage>(eventType);
        }

        EventDispatcher.instance.AddEventHandler<FarmingCabbage>((int)LHMEventType.eEventType.CABBAGE_INACTIVE, new EventHandler<FarmingCabbage>((type, cabbage) =>
        {
            Debug.LogFormat("cabbageType:{0}", cabbage.type);

            if (cabbage.type == Enums.eCabbageType.Normal)
            {
                StartCoroutine(this.GenerateCabbage(cabbage.transNum, regenTime));
            }
            else
            {
                StartCoroutine(this.SetActiveCabbage(cabbage));
            }
        }));
    }

    


    public IEnumerator SetActiveCabbage(FarmingCabbage cabbage)
    {
        yield return YieldCache.WaitForSeconds(0.38f);
        cabbage.poisonEffectGo.SetActive(false);
        cabbage.gameObject.SetActive(false);
        yield return YieldCache.WaitForSeconds(regenTime);

        int percentage = Random.Range(1, 101);
        if (percentage <= poisonPercentage)
        {
            cabbage.type = Enums.eCabbageType.Poison;
            cabbage.poisonCabbageGo.SetActive(true);
        }
        else
        {
            cabbage.type = Enums.eCabbageType.Normal;
            cabbage.normalCabbageGo.SetActive(true);
        }
        cabbage.gameObject.SetActive(true);
        Poses[cabbage.transNum].GetComponent<TargetPos>().cabbageList.Add(cabbage);
        yield break;
    }

    public IEnumerator GenerateCabbage(int i,float time)
    {
        yield return YieldCache.WaitForSeconds(time);

        if (Poses[i].GetComponent<TargetPos>().cabbageList.Count == 0)
        {
            //Debug.LogFormat("양배추{0} 생성", i);
            var cabbage = this.cabbage.Clone();
            cabbage.transform.parent = Poses[i];
            cabbage.transform.position = Poses[i].position;

            //var crob = Instantiate(cabbagePrefab, crobPoses[i]).GetComponent<HSYCrob>();

            //Debug.Log(percentage);
            if (this.stage == 3)
            {
                cabbage.type = Enums.eCabbageType.Normal;
                cabbage.normalCabbageGo.SetActive(true);
            }
            else
            {
                int percentage = Random.Range(1, 101);
                if (percentage <= poisonPercentage)
                {
                    cabbage.type = Enums.eCabbageType.Poison;
                    cabbage.poisonCabbageGo.SetActive(true);
                }
                else
                {
                    cabbage.type = Enums.eCabbageType.Normal;
                    cabbage.normalCabbageGo.SetActive(true);
                }
            }

            cabbage.transNum = i;
            Poses[i].GetComponent<TargetPos>().cabbageList.Add(cabbage);

            cabbage.Init();
        }
        yield break;
    }
}
