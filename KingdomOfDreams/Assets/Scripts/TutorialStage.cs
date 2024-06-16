using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialStage : MonoBehaviour
{
    public GameObject ground;
    //public GameObject npc;
    public Material stageMat;


    void Start()
    {
    }

    public void clearTutorial(int tutorialNum)
    {
        var txt = string.Format("Materials/Tutorial{0}", tutorialNum+1);
        Debug.Log(txt);
        var blackTex = Resources.Load<Texture>("Black/KingdomBlack");

        Resources.Load<Material>(txt).SetTexture("Albedo", blackTex);

        var groundColorMat = Resources.Load<Material>("Materials/GroundBlack");
        //MaterialManager.instance.ChangeOthers(this.grounds, groundColorMat);

        //Debug.LogFormat("<color>stageMat : {0}, blackTex : {1}</color>", stageMat, blackTex);
        //MaterialManager.instance.ChangeTexture(this.stageMat, blackTex);
    }
}
