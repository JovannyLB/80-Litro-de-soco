using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class IndexTest : MonoBehaviour, ISelectHandler
{
    
    
    private bool selected;
    private int index;
    private Button botao;
    public int numeroPersonagem;

    public int GetNumeroPersonagem()
    {
        return numeroPersonagem;
    }

    public void setNumeroPersonagem(int numeroPersonagemSet)
    {
        numeroPersonagem = numeroPersonagemSet;
    }

    public int GetIndex()
    {
        return index;
    }
    
    public bool GetSelected()
    {
        return selected;
    }

    public void SetIndex(int indexNum)
    {
        index = indexNum;
    }

    public void SetPosicaoFila(bool num)
    {
        selected = num;
    }

    public void OnSelect(BaseEventData eventData)
    {
    }
    public void OnDeselect(BaseEventData eventData)
    {
    }

}
