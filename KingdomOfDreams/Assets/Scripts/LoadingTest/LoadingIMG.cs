using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIMG : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Loading());
    }
    private IEnumerator Loading()
    {
        while(true)
        {
            this.transform.Rotate(new Vector3(0f, 0f, -30f));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
