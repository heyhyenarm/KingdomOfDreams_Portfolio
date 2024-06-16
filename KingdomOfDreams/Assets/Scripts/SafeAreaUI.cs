﻿using UnityEngine;

using UnityEngine.UI;


public class SafeAreaUI : MonoBehaviour

{
    private void OnDisable()
    {
        Debug.Log("<color=red>SafeAreaUI disable</color>");
    }

    void Start()

    {

        RectTransform rectTransform = GetComponent<RectTransform>();

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;

        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;

        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;

        rectTransform.anchorMax = anchorMax;

    }

}