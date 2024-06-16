using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeStage : MonoBehaviour
{
    public GameObject ground;
    //public GameObject npc;
    public Material stageMat;


    void Start()
    {
    }

    public void clearTutorial(int tutorialNum)
    {
        var colorTex = Resources.Load<Texture>("Color/ColorKingdom");

        var txtTag = string.Format("Tutorial{0}", tutorialNum + 1);
        MaterialManager.instance.ChangeOthers(txtTag, this.stageMat, colorTex);

        var groundColorMat = Resources.Load<Material>("Materials/GroundColor");
        if (tutorialNum == 4) groundColorMat = Resources.Load<Material>("Materials/Tutorial05GroundColor");

        if (tutorialNum != 3) MaterialManager.instance.ChangeGround(this.ground, groundColorMat);

        //Debug.LogFormat("<color>stageMat : {0}, colorTex : {1}</color>", stageMat, colorTex);

    }
    public void clearArea(int stageNum, int areaNum)
    {
        var colorTex = Resources.Load<Texture>("Color/ColorKingdom");

        var txtTag = string.Format("Stage{0}_{1}", stageNum, areaNum);
        MaterialManager.instance.ChangeOthers(txtTag, this.stageMat, colorTex);

        var groundColorMat = Resources.Load<Material>("Materials/GroundColor");

        MaterialManager.instance.ChangeGround(this.ground, groundColorMat);

        //Debug.LogFormat("<color>stageMat : {0}, colorTex : {1}</color>", stageMat, colorTex);

    }
}