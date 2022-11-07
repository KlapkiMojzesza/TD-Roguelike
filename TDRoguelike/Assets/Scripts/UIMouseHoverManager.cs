using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMouseHoverManager : MonoBehaviour
{
    bool mouseOverButton = false;

    public static event Action OnMouseButtonEnter;
    public static event Action OnMouseButtonExit;

    public void mousceOverButtonEnter()
    {
        mouseOverButton = true;
        OnMouseButtonEnter?.Invoke();
    }

    public void mousceOverButtonExit()
    {
        mouseOverButton = false;
        OnMouseButtonExit?.Invoke();
    }
}
