using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILife : MonoBehaviour
{
    public GameObject lifePrefab;
    public List<GameObject> lifes;
    private Color white = new Color(1, 1, 1, 1);
    public GameObject txt;

    public void Init()
    {
        this.GetComponent<Image>().color = white;
        this.txt.SetActive(true);

        while (lifes.Count < 3)
        {
            lifes.Add(Instantiate(this.lifePrefab, this.transform));
        }

        foreach(var life in lifes)
        {
            life.SetActive(true);
        }
    }
}
