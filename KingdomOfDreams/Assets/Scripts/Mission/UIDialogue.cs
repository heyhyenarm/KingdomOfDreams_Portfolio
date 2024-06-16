using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIDialogue : MonoBehaviour
{
    //public Text txtDialogue;
    public TMP_Text txtDialogue;

    public Image imgNPC;
    public Text txtName;

    public Image imgPlayer;
    public Text txtPlayer;

    private Vector3 npcDialPos =new Vector3(68,8.34f,0);
    private Vector3 playerDialPos = new Vector3(-68, 8.34f, 0);

    public int id;
    public int missionId;



    private void OnEnable()
    {
        Debug.LogFormat("<color=yellow>uidialogue OnEnable</color>");
    }

    public void Init(DialogueData data )
    {
        Debug.LogFormat("<color=yellow>uidialogue init »£√‚</color>");

        //this.txtDialogue.text = "";
        this.ButtonOff();

        if (data.npc_id == 0)
        {
            this.imgNPC.gameObject.SetActive(false);
            this.txtName.gameObject.SetActive(false);
            this.imgPlayer.gameObject.SetActive(true);
            this.txtPlayer.gameObject.SetActive(true);
            this.txtDialogue.transform.localPosition = playerDialPos;

            var characterId = InfoManager.instance.PlayerInfo.nowCharacterId*4+InfoManager.instance.PlayerInfo.nowMatNum+3007;
            Debug.LogFormat("<color=yellow>dialogue characterId:{0}</color",characterId);
            var atlas = AtlasManager.instance.GetAtlasByName("character");
            Debug.LogFormat("<color=yellow>sprite name:{0}</color>", DataManager.instance.GetNpcData(characterId).sprite_name);

            this.imgPlayer.sprite = atlas.GetSprite(DataManager.instance.GetNpcData(characterId).sprite_name);
            this.txtDialogue.text = data.content0;
        }
        else
        {
            this.imgNPC.gameObject.SetActive(true);
            this.txtName.gameObject.SetActive(true);
            this.imgPlayer.gameObject.SetActive(false);
            this.txtPlayer.gameObject.SetActive(false);
            this.txtDialogue.transform.localPosition = npcDialPos;

            var atlas = AtlasManager.instance.GetAtlasByName("character");
            this.imgNPC.sprite = atlas.GetSprite(DataManager.instance.GetNpcData(data.npc_id).sprite_name);
            this.txtName.text = DataManager.instance.GetNpcData(data.npc_id).name;
            this.txtDialogue.text = data.content0;
        }
        this.id = data.id;
        this.missionId = data.mission_id;
    }

    public void BuTttonOn()
    {
        this.GetComponent<Button>().interactable = true;
    }
    private void ButtonOff()
    {
        this.GetComponent<Button>().interactable=false;
    }
}
