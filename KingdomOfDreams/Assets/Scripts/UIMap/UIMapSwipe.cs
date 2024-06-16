using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMapSwipe : MonoBehaviour
{
    public GameObject scrollbar;
    public GameObject content;

    private float[] pos;
    private float scrollPos = 0;


    void Update()
    {
        this.pos = new float[this.transform.childCount];
        float distance = 1f / (this.pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            this.pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            this.scrollPos = this.scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (this.scrollPos < pos[i] + (distance / 2) && this.scrollPos > pos[i] - (distance / 2))
                {
                    this.scrollbar.GetComponent<Scrollbar>().value =
                        Mathf.Lerp(this.scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (this.scrollPos < this.pos[i] + (distance / 2) && this.scrollPos > this.pos[i] - (distance / 2))
            {
                Debug.LogFormat("{0} selected", i);
                this.transform.GetChild(i).localScale =
                    Vector2.Lerp(this.transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                this.content.transform.GetChild(i).localScale =
                    Vector2.Lerp(this.content.transform.GetChild(i).localScale, new Vector2(2f, 2f), 0.1f);
                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        this.content.transform.GetChild(j).localScale
                            = Vector2.Lerp(this.content.transform.GetChild(j).localScale, new Vector2(1f, 1f), 0.1f);
                        this.transform.GetChild(j).localScale
                            = Vector2.Lerp(this.transform.GetChild(j).localScale, new Vector2(1f, 1f), 0.1f);
                    }
                }
            }
        }
    }
}
