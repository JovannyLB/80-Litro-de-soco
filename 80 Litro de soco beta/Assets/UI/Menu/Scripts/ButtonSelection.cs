using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using DG.Tweening;
using UnityEngine.UIElements;


public class ButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler// required interface when using the OnSelect method.
{
    private bool playSelected;
    
    public void OnSelect(BaseEventData eventData)
    {
        playSelected = true;
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        playSelected = false;
    }

    public bool getPlaySelected()
    {
        return playSelected;
    }
}

