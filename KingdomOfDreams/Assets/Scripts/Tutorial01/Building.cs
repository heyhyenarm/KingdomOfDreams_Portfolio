using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Building : MonoBehaviour
{
    public int id;
    //õ��
    public GameObject tent;
    public Button btn_UseDream;
    public Button btn_PlayAD;
    public GameObject dreamPrice;
    public TMP_Text txt_DreamPrice;
    //public BuildingUIDirector director;

    public GameObject[] structures;
    public GameObject foundation;
    private List<List<MeshRenderer>> list = new List<List<MeshRenderer>>();

    //����
    public GameObject reviveVideo;
    public VideoPlayer videoPlayer;

    //����� �ϴ� �ǹ�?
    public bool isDone;
    //�Ǽ� �ð� ��?
    public bool endTime;
    //������ �ǹ�?
    public bool isBuild;
    private float delta = 0;
    public int windowIndex;

    //�̼� �Ϸ� �˸� ����Ʈ
    public bool isClear;
    public GameObject effect;

    private System.DateTime buildStartTime;
    public TMP_Text txt_remainTime;
    public bool isComplete = true;

    private float animTime = 3f;

    [HideInInspector]
    public int buildTime;

    [HideInInspector]
    public BuildingData buildData;

    //���
    public AudioSwitcher audioSwitcher;
    public void Init(int id)
    {
        this.id = id;

        this.buildData = DataManager.instance.GetBuildingData(id);

        this.buildTime = this.buildData.build_time;
        //�ӽ�
        //this.buildTime = 5;

        Debug.LogFormat("this.id: {0}", this.id);
        var info = InfoManager.instance.BuildingInfos.Find(x => x.id == this.id);
        Debug.LogFormat("building info: {0}", info);

        this.gameObject.SetActive(true);

        if (info != null)
        {
            this.isDone = info.isDone;
            this.isBuild = info.isBuild;
            this.endTime = info.endTime;
            this.isComplete = info.isComplete;
            this.buildStartTime = info.buildStartTime;
            if (info.isBuild)
            {
                this.tent.SetActive(false);
                this.foundation.SetActive(true);
                this.isClear = true;
                foreach (GameObject go in this.structures)
                {
                    go.SetActive(true);
                }
            }
            else if(info.endTime || !info.isComplete)
            {
                this.tent.SetActive(true);
                
                if (this.endTime)
                {
                    this.txt_remainTime.text = string.Format("COMPLETE!");
                    this.btn_PlayAD.gameObject.SetActive(false);
                    this.btn_UseDream.gameObject.SetActive(false);
                    this.dreamPrice.SetActive(false);
                }
                else
                {
                    this.btn_PlayAD.gameObject.SetActive(true);
                    this.btn_UseDream.gameObject.SetActive(true);
                    this.dreamPrice.SetActive(true);
                }
                this.foundation.SetActive(false);

                foreach (GameObject go in this.structures)
                {
                    go.SetActive(false);
                    MeshRenderer[] mesh = go.GetComponentsInChildren<MeshRenderer>();
                    List<MeshRenderer> meshlist = mesh.ToList();
                    //Debug.LogFormat("matlist.Count: {0}", meshlist.Count);
                    for (int i = 0; i < mesh.Length; i++)
                    {
                        Material mat = mesh[i].material;
                        //Material.Rendering Mode => Transparent=====
                        mat.SetFloat("_Mode", 3);
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_ZWrite", 0);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.DisableKeyword("_ALPHABLEND_ON");
                        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = 3000;
                        //=============================================
                        Color matColor = mat.color;
                        matColor.a = 0.1f;
                        mat.color = matColor;
                    }
                    list.Add(meshlist);
                }
            }
            else
            {
                this.tent.SetActive(false);
                this.foundation.SetActive(true);

                foreach (GameObject go in this.structures)
                {
                    go.SetActive(false);
                    MeshRenderer[] mesh = go.GetComponentsInChildren<MeshRenderer>();
                    List<MeshRenderer> meshlist = mesh.ToList();
                    //Debug.LogFormat("matlist.Count: {0}", meshlist.Count);
                    for (int i = 0; i < mesh.Length; i++)
                    {
                        Material mat = mesh[i].material;
                        //Material.Rendering Mode => Transparent=====
                        mat.SetFloat("_Mode", 3);
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_ZWrite", 0);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.DisableKeyword("_ALPHABLEND_ON");
                        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = 3000;
                        //=============================================
                        Color matColor = mat.color;
                        matColor.a = 0.1f;
                        mat.color = matColor;
                    }
                    list.Add(meshlist);
                }
            }
        }
        else
        {
            this.isDone = false;
            this.endTime = false;
            this.isComplete = true;
            this.isBuild = false;

            this.tent.SetActive(false);
            this.foundation.SetActive(true);

            foreach (GameObject go in this.structures)
            {
                go.SetActive(false);
            }
            InfoManager.instance.BuildingInfos.Add(new BuildingInfo(this.id));
            InfoManager.instance.SaveBuildingInfos();
        }
    }
    //�Ǽ� ����
    public void StartBuild()
    {
        var info = InfoManager.instance.BuildingInfos.Find(x => x.id == buildData.id);

        this.effect.SetActive(false);
        //this.isDone = false;
        //info.isDone = this.isDone;
        //InfoManager.instance.SaveBuildingInfos();

        foreach (GameObject go in this.structures)
        {
            go.SetActive(false);
            MeshRenderer[] mesh = go.GetComponentsInChildren<MeshRenderer>();
            List<MeshRenderer> meshlist = mesh.ToList();
            //Debug.LogFormat("matlist.Count: {0}", meshlist.Count);
            for (int i = 0; i < mesh.Length; i++)
            {
                Material mat = mesh[i].material;
                //Material.Rendering Mode => Transparent=====
                mat.SetFloat("_Mode", 3);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
                //=============================================
                Color matColor = mat.color;
                matColor.a = 0.1f;
                mat.color = matColor;
            }
            list.Add(meshlist);
        }
        this.foundation.SetActive(false);
        //õ��
        this.tent.SetActive(true);
        this.btn_PlayAD.gameObject.SetActive(true);
        this.btn_UseDream.gameObject.SetActive(true);
        this.dreamPrice.SetActive(true);
        this.buildStartTime = System.DateTime.Now;
        info.buildStartTime = this.buildStartTime;

        this.isComplete = false;
        info.isComplete = this.isComplete;
        InfoManager.instance.SaveBuildingInfos();
        //Debug.Log(buildStartTime);
    }
    //�Ǽ� �Ϸ�
    public void BuildObject()
    {
        var info = InfoManager.instance.BuildingInfos.Find(x => x.id == buildData.id);
        this.isBuild = true;
        info.isBuild = this.isBuild;
        InfoManager.instance.SaveBuildingInfos();

        this.tent.SetActive(false);
        //this.txt_remainTime.gameObject.SetActive(false);

        this.foundation.SetActive(true);
        foreach (GameObject go in this.structures)
        {
            go.SetActive(true);
        }
        StartCoroutine(Rebuilding());
    }
    public void Update()
    {
        if (!this.isComplete)
        {
            var time = System.DateTime.Now - this.buildStartTime;
            //Debug.LogFormat("time : {0}", time);
            int timeHours = time.Hours;
            int timeMinutes = time.Minutes;
            int timeSeconds = time.Seconds;
            int hour = (this.buildTime - 3600 * timeHours - 60 * timeMinutes - timeSeconds) / 3600;
            int min = (this.buildTime - 60 * timeMinutes - timeSeconds) / 60 % 60;
            int sec = (this.buildTime - timeSeconds) % 60;
            this.txt_remainTime.gameObject.SetActive(true);
            this.txt_remainTime.text = string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
            var remaintime = this.buildTime - 3600 * timeHours - 60 * timeMinutes - timeSeconds;
            if(remaintime % 20 == 0) this.txt_DreamPrice.text = string.Format("{0:#,###}", remaintime / 20);
            else this.txt_DreamPrice.text = string.Format("{0:#,###}", remaintime / 20 + 1);

            var passedTime = 3600 * timeHours + 60 * timeMinutes + timeSeconds;
            if (passedTime >= this.buildTime)
            {
                this.txt_remainTime.text = string.Format("COMPLETE!");
                var info = InfoManager.instance.BuildingInfos.Find(x => x.id == buildData.id);
                this.isComplete = true;
                info.isComplete = this.isComplete;
                this.endTime = true;
                info.endTime = this.endTime;
                InfoManager.instance.SaveBuildingInfos();

                this.btn_PlayAD.gameObject.SetActive(false);
                this.btn_UseDream.gameObject.SetActive(false);
                this.dreamPrice.SetActive(false);
            }
        }
    }
    IEnumerator Rebuilding()
    {
        //���� �� ������ ��
        int i = 0;
        while (true)
        {
            this.delta += Time.deltaTime;
            //1�� => �ǹ� ���� �ð�/this.list.Count
            if (this.delta > this.animTime / this.list.Count)
            {
                for (int j = 0; j < this.list[i].Count; j++)
                {
                    Material mat = this.list[i][j].material;
                    Color matColor = mat.color;
                    if (i != this.windowIndex)
                    {
                        //Material.Rendering Mode => Opaque=====
                        mat.SetFloat("_Mode", 0);
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        mat.SetInt("_ZWrite", 1);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.DisableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = -1;
                        //======================================
                        matColor.a = 1;
                        mat.color = matColor;
                    }
                    else matColor.a = 0.5f; //â��
                    mat.color = matColor;
                }
                this.delta = 0;
                i++;
            }
            if (i == this.list.Count)
            {
                foreach(var stageInfo in InfoManager.instance.StageInfos)
                {
                    if (!stageInfo.isClear)
                    {
                        if (stageInfo.stage < 5)
                        {
                            this.reviveVideo = null;
                        }
                    }
                }
                if (this.reviveVideo != null)
                {
                    //��� ����
                    SoundManager.Pause();

                    //���� ����
                    this.videoPlayer.SetDirectAudioVolume(0, PlayerPrefs.GetFloat("SoundVolume"));

                    this.reviveVideo.SetActive(true);
                    Debug.LogFormat("<color=red>�������� ���� Ŭ���� ���� ���</color>");

                    yield return new WaitForSeconds(8f);

                    this.reviveVideo.SetActive(false);

                    //��� ���
                    SoundManager.UnPause();


                    int dialogueId = InfoManager.instance.DialogueInfo.id;
                    if (dialogueId!=10070&& dialogueId != 10077&& dialogueId!=10081)
                        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.END_REVIVE_VIDEO);

                    //��� ü����
                    //this.audioSwitcher.SwitchAudioSource();
                    var info = InfoManager.instance.BuildingInfos.Find(x => x.id == buildData.id);
                    Debug.LogFormat("this.id : {0}, info.id : {1}", this.id, info.id);
                    //���� npc �̵�
                    if (info.id == 4005)
                    {
                        EventManager.instance.blackToColor(5);
                        EventManager.instance.npcMove(4005);
                    }
                    else if (info.id == 4006)
                    {
                        EventManager.instance.blackToColor(6);
                        EventManager.instance.npcMove(4006);
                    }
                    else if (info.id == 4007)
                    {
                        EventManager.instance.blackToColor(7);
                        //EventManager.instance.npcMove(4007);
                    }
                    else if (info.id == 4008)
                    {
                        EventManager.instance.blackToColor(8);
                        //EventManager.instance.npcMove(4008);
                    }
                    else if (info.id == 4009)
                    {
                        EventManager.instance.blackToColor(9);
                        //EventManager.instance.npcMove(4009);
                    }

                    if ((this.id-4000)>=5) EventManager.instance.blackToColor(info.id - 4000);
                }
                this.isClear = true;

                yield break;
            }
            yield return null;
        }
    }
    //�÷��̾� ����� ���� Ŭ�� ����
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        this.gameObject.tag = "Building";
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        this.gameObject.tag = "Untagged";
    //    }
    //}
}