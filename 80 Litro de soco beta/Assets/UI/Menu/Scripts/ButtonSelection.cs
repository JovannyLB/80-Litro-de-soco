using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public class ButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler// required interface when using the OnSelect method.
{
    
    public bool playSelected;

    public ButtonSelection()
    {
        playSelected = false;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        playSelected = true;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        playSelected = false;
    }
    

    private void FixedUpdate()
    {
        if (playSelected)
        {
            Debug.Log(this.gameObject.name + " selected");
        }
        else
        {
            Debug.Log(this.gameObject.name + " deselected");
        }
    }
}

