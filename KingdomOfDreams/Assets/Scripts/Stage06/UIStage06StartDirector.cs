using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStage06StartDirector : MonoBehaviour
{
    public UIDialogue dialogue;

    public int dialogueID;

    public float speed = 500f;
    public GameObject yPos1;
    public GameObject yPos2;

    public Image black1;
    public Image black2;

    private void Start()
    {
        InfoManager.instance.DialogueInfo.id = 10043;

        Invoke("DialogueInit", 2f);

        this.AddButton();
    }
    private void Update()
    {
        MoveImage();

    }

    public void MoveImage()
    {
        black1.transform.position = Vector3.MoveTowards(black1.transform.position, yPos1.transform.position, speed * Time.deltaTime);
        black2.transform.position = Vector3.MoveTowards(black2.transform.position, yPos2.transform.position, speed * Time.deltaTime);

    }

    public void DialogueInit()
    {
        Debug.LogFormat("<color=cyan>다이얼로그 이닛</color>");
        var data = this.GetDialogueData();
        this.dialogue.Init(data);
        this.dialogue.gameObject.SetActive(true);
    }

    private DialogueData GetDialogueData()
    {
        var dialogueId = InfoManager.instance.DialogueInfo.id;
        var data = DataManager.instance.GetDialogueData(dialogueId);
        return data;
    }
 
    private void AddButton()
    {
        this.dialogue.GetComponent<Button>().onClick.AddListener(() => {
            var data = this.GetDialogueData();
            InfoManager.instance.DialogueInfo.id = data.id + 1;
            InfoManager.instance.SaveDialogueInfo();
            if (data.id==10047)
            {
                this.dialogue.gameObject.SetActive(false);

                NPCController[] npcs = FindObjectsOfType<NPCController>();
                foreach (NPCController npc in npcs)
                {
                    npc.MoveToNextDestination();
                }

                StartCoroutine(WaitTime());
            }
            else
            {
                this.dialogue.Init(this.GetDialogueData());
            }
        });
    }

    public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(4f);

        EventManager.instance.onStage06StartEnd();
    }
    //public void ShowDialogue()
    //{

    //    //this.dialogue.GetComponent<Button>().onClick.AddListener(() =>
    //    //{
    //    //    this.dialogue.gameObject.SetActive(false);
    //    //});



    //    this.dialogue.GetComponent<Button>().onClick.AddListener(() =>
    //    {
    //        var data = this.GetDialogueData();
    //        InfoManager.instance.DialogueInfo.id = data.id + 1;
    //        InfoManager.instance.SaveDialogueInfo();

    //        this.dialogue.Init(this.GetDialogueData());

    //        ++this.dialogueID ;

    //        DialogueData dialogueData = DataManager.instance.GetDialogueData(dialogueID);
    //        this.dialogue.txtDialogue.text = dialogueData.content0;

    //        this.dialogue.GetComponent<Button>().onClick.AddListener(() =>
    //        {
    //            ++this.dialogueID;

    //            DialogueData dialogueData = DataManager.instance.GetDialogueData(dialogueID);
    //            this.dialogue.txtDialogue.text = dialogueData.content0;

    //            this.dialogue.GetComponent<Button>().onClick.AddListener(() =>
    //            {
    //                ++this.dialogueID;

    //                DialogueData dialogueData = DataManager.instance.GetDialogueData(dialogueID);
    //                this.dialogue.txtDialogue.text = dialogueData.content0;

    //                this.dialogue.GetComponent<Button>().onClick.AddListener(() =>
    //                {
    //                    ++this.dialogueID;

    //                    DialogueData dialogueData = DataManager.instance.GetDialogueData(dialogueID);
    //                    this.dialogue.txtDialogue.text = dialogueData.content0;

    //                    this.dialogue.GetComponent<Button>().onClick.AddListener(() =>
    //                    {
    //                        this.dialogue.gameObject.SetActive(false);
    //                        InfoManager.instance.DialogueInfo.id = 10048;
    //                        InfoManager.instance.SaveDialogueInfo();



    //                    });

    //                });

    //            });

    //        });
    //    });


    //}


}
