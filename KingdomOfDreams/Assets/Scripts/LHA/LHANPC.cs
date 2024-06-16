using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHANPC : MonoBehaviour
{
    public SkinnedMeshRenderer smr;
    private Material mat;
    private System.Action onLerpComplete;
    

    void Start()
    {
        this.mat = smr.material;
        this.Lerp();

        this.onLerpComplete = () => {
            Debug.Log("lerp complete");
        };
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        this.Lerp();
    //    }
    //}

    private void Lerp()
    {
        Debug.Log("lerp");
        this.StartCoroutine(this.CoLerp());
    }

    private IEnumerator CoLerp()
    {

        float val = 0;
        float speed = 1f;

        while (true)
        {
            val = Mathf.Lerp(val, 1, Time.deltaTime * speed * 0.5f);
            Debug.Log(val);
            this.mat.SetFloat("_Gradation", val);
            if (val > 0.7f)
            {
                this.mat.SetFloat("_Gradation", 1);
                break;
            }
            yield return null;
        }
        this.onLerpComplete();
    }
}