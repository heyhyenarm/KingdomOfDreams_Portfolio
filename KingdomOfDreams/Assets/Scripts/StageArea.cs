using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageArea : MonoBehaviour
{
    public GameObject[] grounds;
    public GameObject roof;
    private Material stageMat;

    private void Awake()
    {
        this.stageMat = Resources.Load<Material>("Materials/DefaultKingdom");
        Debug.LogFormat("<color>stageMat = {0}</color>", this.stageMat);
    }

    void Start()
    {
    }

    //public void clearGround(int stageNum, int areaNum)
    //{
    //    var colorTex = Resources.Load<Texture>("Color/ColorKingdom");

    //    var txtTag = string.Format("StageGround{0}", areaNum + 1);
    //    MaterialManager.instance.ChangeOthers(txtTag, this.stageMat, colorTex);

    //    var groundColorMat = Resources.Load<Material>("Materials/GroundColor");
    //     MaterialManager.instance.ChangeGround(this.grounds, groundColorMat);

    //    //Debug.LogFormat("<color>stageMat : {0}, colorTex : {1}</color>", stageMat, colorTex);

    //}
    public void clearArea(int stageNum, int areaNum)
    {
        Debug.Log("clearArea");
        var colorTex = Resources.Load<Texture>("Color/ColorKingdom");
        string txtTag;
        if (areaNum >= 6)
        {
            txtTag = string.Format("Stage{0}_{1}", stageNum, areaNum - 1);
        }
        else
        {
            txtTag = string.Format("Stage{0}_{1}", stageNum, areaNum);
        }

        MaterialManager.instance.ChangeOthers(txtTag, this.stageMat, colorTex);
        if (this.grounds != null)
        {
            var groundColorMat = Resources.Load<Material>("Materials/GroundColor");
            foreach(var ground in this.grounds)
            {
                MaterialManager.instance.ChangeGround(ground, groundColorMat);
            }
        }
        if(this.roof != null)
        {
            Debug.Log("<color=yellow>roof change</color>");
            var roofColorMat = Resources.Load<Material>("Materials/Castle_Roof");
            MaterialManager.instance.ChangeRoof(this.roof, roofColorMat);
        }

        //Debug.LogFormat("<color>stageMat : {0}, colorTex : {1}</color>", stageMat, colorTex);
    }
}