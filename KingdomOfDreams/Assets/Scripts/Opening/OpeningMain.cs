using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningMain : MonoBehaviour
{
    public GameObject opening;

    private void Awake()
    {
        //¿ÀÇÁ´× Àç»ý
        EventManager.instance.playOpening = () =>
        {
            //¿ÀÇÁ´× ¿µ»ó Àç»ý
            this.opening.SetActive(true);
            StartCoroutine(WaitForOpening(39f));

        };


    }

    private void Update()
    {
        //¿ÀÇÁ´× ½ºÅµ
        if (Input.GetMouseButtonDown(0))
        {
            EventManager.instance.onClickedSkip();
        }

    }

    private IEnumerator WaitForOpening(float time)
    {
        this.opening.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);

        EventManager.instance.EndOpening();
        //this.opening.SetActive(false);
    }

}
