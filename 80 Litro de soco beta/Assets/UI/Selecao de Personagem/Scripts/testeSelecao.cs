using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class testeSelecao : MonoBehaviour
{

    public Image personagem1;
    public Image personagem2;
    public Image personagem3;
    
    private List<Image> arrayPersonagens;

    private void Start()
    {
        arrayPersonagens.Add(personagem1);
        arrayPersonagens.Add(personagem2);
        arrayPersonagens.Add(personagem3);
        
        Debug.Log("Array: " + arrayPersonagens);
    }
}
