using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager
{
    public static readonly MaterialManager instance = new MaterialManager();
    private MaterialManager()
    {

    }

    public void ChangeOthers(string targetTag, Material mat, Texture tex)
    {
        //mat.SetTexture("_MainTex", tex);
        Debug.LogFormat("<color=red>ChangeOthers, tag : {0}</color>", targetTag);

        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject target in targets)
        {
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    Material materialInstance = new Material(mat);
                    materialInstance.name = "colorKingdom";
                    materialInstance.mainTexture = materials[i].mainTexture; // 이전 Material의 texture를 새로 생성한 Material에 복사
                    materialInstance.mainTexture = tex;
                    materials[i] = materialInstance;
                }
                renderer.materials = materials;
                //target.layer = 14;
            }
        }

    }
    public void ChangeGround(GameObject targetGo, Material mat)
    {
        targetGo.GetComponent<Renderer>().material = mat;
        //targetGo.layer = 14;
    }
    public void ChangeRoof(GameObject targetGo, Material mat)
    {
        Debug.LogFormat("<color=yellow>target : {0}, mat : {1}, mats0 : {2}, mat1 : {3}</color>", targetGo, mat, 
            targetGo.GetComponent<Renderer>().materials[0], targetGo.GetComponent<Renderer>().materials[1]);
        //targetGo.GetComponent<Renderer>().materials[1] = mat;
        Material[] materials = targetGo.GetComponent<Renderer>().materials;
        materials[1] = mat;
        targetGo.GetComponent<Renderer>().materials = materials;
        //targetGo.layer = 14;
    }
    public void Change8Ground()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Ground8");
        foreach (GameObject target in targets)
        {
            Renderer renderer = target.GetComponent<Renderer>();
            renderer.material.color = Color.HSVToRGB(0.4392157f, 0.4823529f, 0.345098f);
        }
    }
}