using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class teste : MonoBehaviour
{
    public Button verde;
    public Button azul;
    public Button amarelo;
    public Button vermelho;

    public Text verdeTxt;
    public Text azulTxt;
    public Text amareloTxt;
    public Text vermelhoTxt;

    private Color verdeSel = new Color(50f/255f,255f/255f,50f/255f,1f);
    private Color azulSel = new Color(0f/255f,165f/255f,255f/255f,1f);
    private Color amareloSel = new Color(240f/255f,255f/255f,40f/255f,1f);
    private Color vermelhoSel = new Color(255f/255f,55f/255f,45f/255f,1f);
    
    private Color verdeNotSel = new Color(117f/255,173f/255f,116f/255f,1f);
    private Color azulNotSel = new Color(89f/255f,173f/255f,204f/255f,1f);
    private Color amareloNotSel = new Color(254f/255f,246f/255f,159f/255f,1f);
    private Color vermelhoNotSel = new Color(220f/255f,112f/255f,112f/255f,1f);
    
    private Color branco = new Color(0.7f,0.7f,0.7f,1);
    private Color brancoInv = new Color(1,1,1,0);
    
    public RectTransform lines;

    private int tamanho;
    private int tamanhoMaior;

    private void Start()
    {
        tamanho = verdeTxt.fontSize;
        tamanhoMaior = verdeTxt.fontSize + 20;
    }

    private void Update()
    {
        //myObject.GetComponent<MyScript>().MyFunction();
        if (verde.GetComponent<ButtonSelection>().getPlaySelected()    && 
            !azul.GetComponent<ButtonSelection>().getPlaySelected()    && 
            !amarelo.GetComponent<ButtonSelection>().getPlaySelected() &&
            !vermelho.GetComponent<ButtonSelection>().getPlaySelected())
        {
            //Set bloco de cor pos
            lines.DOAnchorPosY(3240f, 0.20f, false).SetDelay(0.1f);
            //Set highlight color
            verdeTxt.DOColor(verdeSel, 0.2f);
            azulTxt.DOColor(azulNotSel, 0.2f);
            amareloTxt.DOColor(amareloNotSel, 0.2f);
            vermelhoTxt.DOColor(vermelhoNotSel, 0.2f);
            //Sets Size
            verdeTxt.fontSize = tamanhoMaior;
            azulTxt.fontSize = tamanho;
            amareloTxt.fontSize = tamanho;
            vermelhoTxt.fontSize = tamanho;
            //Sets outline
            verdeTxt.GetComponent<Outline>().DOColor(branco, 0.2f);
            azulTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            amareloTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            vermelhoTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
        }
        else if (!verde.GetComponent<ButtonSelection>().getPlaySelected()   && 
                  azul.GetComponent<ButtonSelection>().getPlaySelected()    && 
                 !amarelo.GetComponent<ButtonSelection>().getPlaySelected() &&
                 !vermelho.GetComponent<ButtonSelection>().getPlaySelected())
        {
            //Set bloco de cor pos
            lines.DOAnchorPosY(1080f, 0.20f, false).SetDelay(0.1f);
            //Set highlight color
            verdeTxt.DOColor(verdeNotSel, 0.2f);
            azulTxt.DOColor(azulSel, 0.2f);
            amareloTxt.DOColor(amareloNotSel, 0.2f);
            vermelhoTxt.DOColor(vermelhoNotSel, 0.2f);
            //Sets Size
            verdeTxt.fontSize = tamanho;
            azulTxt.fontSize = tamanhoMaior;
            amareloTxt.fontSize = tamanho;
            vermelhoTxt.fontSize = tamanho;
            //Sets outline
            verdeTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            azulTxt.GetComponent<Outline>().DOColor(branco, 0.2f);
            amareloTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            vermelhoTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
        }
        else if (!verde.GetComponent<ButtonSelection>().getPlaySelected()   && 
                 !azul.GetComponent<ButtonSelection>().getPlaySelected()    && 
                  amarelo.GetComponent<ButtonSelection>().getPlaySelected() &&
                 !vermelho.GetComponent<ButtonSelection>().getPlaySelected())
        {
            //Set bloco de cor pos
            lines.DOAnchorPosY(-1080f, 0.20f, false).SetDelay(0.1f);
            //Set highlight color
            verdeTxt.DOColor(verdeNotSel, 0.2f);
            azulTxt.DOColor(azulNotSel, 0.2f);
            amareloTxt.DOColor(amareloSel, 0.2f);
            vermelhoTxt.DOColor(vermelhoNotSel, 0.2f);
            //Sets Size
            verdeTxt.fontSize = tamanho;
            azulTxt.fontSize = tamanho;
            amareloTxt.fontSize = tamanhoMaior;
            vermelhoTxt.fontSize = tamanho;
            //Sets outline
            verdeTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            azulTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            amareloTxt.GetComponent<Outline>().DOColor(branco, 0.2f);
            vermelhoTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
        }
        else if (!verde.GetComponent<ButtonSelection>().getPlaySelected()   && 
                 !azul.GetComponent<ButtonSelection>().getPlaySelected()    && 
                 !amarelo.GetComponent<ButtonSelection>().getPlaySelected() &&
                  vermelho.GetComponent<ButtonSelection>().getPlaySelected())
        {
            //Set bloco de cor pos
            lines.DOAnchorPosY(-3240f, 0.20f, false).SetDelay(0.1f);
            //Set highlight color
            verdeTxt.DOColor(verdeNotSel, 0.2f);
            azulTxt.DOColor(azulNotSel, 0.2f);
            amareloTxt.DOColor(amareloNotSel, 0.2f);
            vermelhoTxt.DOColor(vermelhoSel, 0.2f);
            //Sets Size
            verdeTxt.fontSize = tamanho;
            azulTxt.fontSize = tamanho;
            amareloTxt.fontSize = tamanho;
            vermelhoTxt.fontSize = tamanhoMaior;
            //Sets outline
            verdeTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            azulTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            amareloTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            vermelhoTxt.GetComponent<Outline>().DOColor(branco, 0.2f);
        }
            
    }
}
