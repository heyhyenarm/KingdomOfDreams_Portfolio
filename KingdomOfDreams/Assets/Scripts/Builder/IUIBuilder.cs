using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUIBuilder
{
    public IUIBuilder SetRectTrans(RectTransform rectTrans);
    public IUIBuilder SetName(string name);
    public IUIBuilder SetNum(int num);
    public IUIBuilder SetPosition(float x, float y);
    public IUIBuilder SetSize(float w, float h);
    public IUIBuilder SetSprite(Sprite spriteColor);
}
