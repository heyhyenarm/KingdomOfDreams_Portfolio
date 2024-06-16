using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStagePlayer : MonoBehaviour
{
    public float speed = 200f;
    public bool isMoving = false;

    public void MoveOn(Transform targetTrans)
    {
        Debug.Log("UIStagePlayer Move");
        this.StartCoroutine(this.CMove(targetTrans));
    }

    public IEnumerator CMove(Transform targetTrans)
    {
        Debug.Log("UIStagePlayer CMove");
        this.isMoving = true;
        Vector3 startPos = this.transform.position;

        float distance = Vector3.Distance(startPos, targetTrans.position);
        float totlaTime = distance / speed;
        float time = 0f;

        while (time < totlaTime)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / totlaTime);

            this.transform.position = Vector3.Lerp(startPos, targetTrans.position, t);

            yield return null;
        }
        this.isMoving = false;
        Debug.Log("<color=yellow>Move done</color>");
        //EventManager.instance.onUIPlayerMoveDone();
    }
}
