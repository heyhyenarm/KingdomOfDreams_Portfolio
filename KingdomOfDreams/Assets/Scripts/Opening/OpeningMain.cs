using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningMain : MonoBehaviour
{
    public GameObject opening;

    private void Awake()
    {
        //������ ���
        EventManager.instance.playOpening = () =>
        {
            //������ ���� ���
            this.opening.SetActive(true);
            StartCoroutine(WaitForOpening(39f));

        };


    }

    private void Update()
    {
        //������ ��ŵ
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
