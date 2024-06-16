using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionDirector : MonoBehaviour
{
    public UIMissionMini missionMini;
    public UIMissionDetail missionDetail;
    public UIDialogue dialogue;
    public int testMissionId=1000;

    public void MissionDirectorInit(int stageNum) 
    {
        this.dialogue.GetComponent<Button>().onClick.AddListener(() => {
            
            this.dialogue.gameObject.SetActive(false);           
            //InfoManager.instance.AddMissionInfo(testMissionId);
            //InfoManager.instance.SaveMissionInfos();

           // this.missionMini.Init();       
        }) ;
        
        this.missionMini.onMissionMiniClick=(() =>
        {            
            if(this.missionDetail.clickCount==0)
               this.missionDetail.Init(stageNum);

            this.missionDetail.gameObject.SetActive(true);
        });

    }

    private void Start()
    {
        Invoke("WaitDialogue", 3f);
    }

    private void Update()
    {
        if (InfoManager.instance.MissionInfos.Count != 0 )
            this.missionMini.gameObject.SetActive(true);
        if (InfoManager.instance.MissionInfos.Count == 0)
            this.missionMini.gameObject.SetActive(false);
    }

    //public void ShowDialogue()
    // {
    //     Debug.Log("director showdialogue 실행");
    //     this.cube.SetActive(true);
    //     this.dialogue.gameObject.SetActive(true);
    //     Debug.Log(this.dialogue.gameObject.activeSelf);
    // }

    public void WaitDialogue()
    {
        this.dialogue.gameObject.SetActive(true); // GameObject 활성화
    }

}
