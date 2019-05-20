using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public GameObject helperVerde;
    public GameObject helperAzul;
    public GameObject helperAmarelo;
    public GameObject helperVermelho;

    private Color verdeSel = new Color(50f/255f,255f/255f,50f/255f,1f);
    private Color azulSel = new Color(0f/255f,165f/255f,255f/255f,1f);
    private Color amareloSel = new Color(240f/255f,255f/255f,40f/255f,1f);
    private Color vermelhoSel = new Color(255f/255f,55f/255f,45f/255f,1f);
    
    private Color verdeNotSel = new Color(117f/255,173f/255f,116f/255f,1f);
    private Color azulNotSel = new Color(89f/255f,173f/255f,204f/255f,1f);
    private Color amareloNotSel = new Color(254f/255f,246f/255f,159f/255f,1f);
    private Color vermelhoNotSel = new Color(220f/255f,112f/255f,112f/255f,1f);
    
    private Color branco = new Color(1.0f,1.0f,1.0f,1);
    private Color brancoInv = new Color(1,1,1,0);
    
    public RectTransform lines;

    private int tamanho;
    private int tamanhoMaior;

    private bool verdeS;
    private bool azulS;
    private bool amareloS;
    private bool vermelhoS;
    
    private Outline[] outlinesVe;
    private Outline[] outlinesAz;
    private Outline[] outlinesAm;
    private Outline[] outlinesVo;

    private float defaultVerde;
    private float defaultAzul;
    private float defaultAmarelo;
    private float defaultVermelho;
    

    private void Start()
    {
        defaultVerde = verde.GetComponent<RectTransform>().position.x;
        
        Debug.Log(defaultVerde);
        //-6
        
        outlinesVe = verdeTxt.GetComponents<Outline>();
        outlinesAz = azulTxt.GetComponents<Outline>();
        outlinesAm = amareloTxt.GetComponents<Outline>();
        outlinesVo = vermelhoTxt.GetComponents<Outline>();
        
        verdeS = false;
        azulS = false;
        amareloS = false;
        vermelhoS = false;
        
        tamanho = verdeTxt.fontSize;
        tamanhoMaior = verdeTxt.fontSize + 20;
    }

    private void Update()
    {
        //myObject.GetComponent<MyScript>().MyFunction();
        if (verde.GetComponent<ButtonSelection>().getPlaySelected()    && 
            !azul.GetComponent<ButtonSelection>().getPlaySelected()    && 
            !amarelo.GetComponent<ButtonSelection>().getPlaySelected() &&
            !vermelho.GetComponent<ButtonSelection>().getPlaySelected()&&
            !verdeS)
        {
            //Booleans if selected
            verdeS = true;
            azulS = false;
            amareloS = false;
            vermelhoS = false;

            helperVerde.transform.DOLocalMoveX(200f,0.4f,false);
            helperAzul.transform.DOLocalMoveX(0f,0.4f,false);
            helperAmarelo.transform.DOLocalMoveX(0f,0.4f,false);
            helperVermelho.transform.DOLocalMoveX(0f,0.4f,false);

            /*
            verde.GetComponent<RectTransform>().DOAnchorPosX(verde.GetComponent<RectTransform>().position.x+ 20, 0.4f, false);
            */
            
            /*
            verdeTxt.GetComponent<RectTransform>().DOAnchorPosX(100f, 0.4f, false);
            azulTxt.GetComponent<RectTransform>().DOAnchorPosX(-30, 0.4f, false);
            amareloTxt.GetComponent<RectTransform>().DOAnchorPosX(-30, 0.4f, false);
            vermelhoTxt.GetComponent<RectTransform>().DOAnchorPosX(-80, 0.4f, false);
            */

            
            //teste
            Debug.Log("verdeS: " + verdeS);
            Debug.Log("azulS: " + azulS);
            Debug.Log("amareloS: " + amareloS);
            Debug.Log("vermelhoS: " + vermelhoS);
            
            //Set bloco de cor pos
            lines.DOAnchorPosY(3240f, 0.5f, false).SetDelay(0.1f);
            
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
            
            outlinesVe[2].enabled = true;
            outlinesVe[3].enabled = true;
            
            outlinesAz[2].enabled = false;
            outlinesAz[3].enabled = false;
            
            outlinesAm[2].enabled = false;
            outlinesAm[3].enabled = false;
            
            outlinesVo[2].enabled = false;
            outlinesVo[3].enabled = false;
            
            /*
            outlinesVe[0].enabled = true;
            outlinesVe[1].enabled = true;
            outlinesVe[2].enabled = true;
            outlinesVe[3].enabled = true;
            
            outlinesAz[0].enabled = false;
            outlinesAz[1].enabled = false;
            outlinesAz[2].enabled = false;
            outlinesAz[3].enabled = false;
            
            outlinesAm[0].enabled = false;
            outlinesAm[1].enabled = false;
            outlinesAm[2].enabled = false;
            outlinesAm[3].enabled = false;
            
            outlinesVo[0].enabled = false;
            outlinesVo[1].enabled = false;
            outlinesVo[2].enabled = false;
            outlinesVo[3].enabled = false;
            */

            //Sets outline
            /*verdeTxt.GetComponent<Outline>().DOColor(branco, 0.2f);
            azulTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            amareloTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            vermelhoTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);*/
        }
        else if (!verde.GetComponent<ButtonSelection>().getPlaySelected()   && 
                  azul.GetComponent<ButtonSelection>().getPlaySelected()    && 
                 !amarelo.GetComponent<ButtonSelection>().getPlaySelected() &&
                 !vermelho.GetComponent<ButtonSelection>().getPlaySelected()&&
                 !azulS)
        {
            verdeS = false;
            azulS = true;
            amareloS = false;
            vermelhoS = false;
            
            helperVerde.transform.DOLocalMoveX(0f,0.4f,false);
            helperAzul.transform.DOLocalMoveX(60f,0.4f,false);
            helperAmarelo.transform.DOLocalMoveX(0f,0.4f,false);
            helperVermelho.transform.DOLocalMoveX(0f,0.4f,false);
            
            /*
            verdeTxt.GetComponent<RectTransform>().DOAnchorPosX(-30f, 0.4f, false);
            azulTxt.GetComponent<RectTransform>().DOAnchorPosX(100f, 0.4f, false);
            amareloTxt.GetComponent<RectTransform>().DOAnchorPosX(-30, 0.4f, false);
            vermelhoTxt.GetComponent<RectTransform>().DOAnchorPosX(-80, 0.4f, false);
            */
            
            Debug.Log("verdeS: " + verdeS);
            Debug.Log("azulS: " + azulS);
            Debug.Log("amareloS: " + amareloS);
            Debug.Log("vermelhoS: " + vermelhoS);
            
            //Set bloco de cor pos
            lines.DOAnchorPosY(1080f, 0.5f, false).SetDelay(0.1f);
            
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
            
            outlinesVe[2].enabled = false;
            outlinesVe[3].enabled = false;
            
            outlinesAz[2].enabled = true;
            outlinesAz[3].enabled = true;
            
            outlinesAm[2].enabled = false;
            outlinesAm[3].enabled = false;
            
            outlinesVo[2].enabled = false;
            outlinesVo[3].enabled = false;
            
            /*

            outlinesVe[0].enabled = false;
            outlinesVe[1].enabled = false;
            outlinesVe[2].enabled = false;
            outlinesVe[3].enabled = false;
            
            outlinesAz[0].enabled = true;
            outlinesAz[1].enabled = true;
            outlinesAz[2].enabled = true;
            outlinesAz[3].enabled = true;
            
            outlinesAm[0].enabled = false;
            outlinesAm[1].enabled = false;
            outlinesAm[2].enabled = false;
            outlinesAm[3].enabled = false;
            
            outlinesVo[0].enabled = false;
            outlinesVo[1].enabled = false;
            outlinesVo[2].enabled = false;
            outlinesVo[3].enabled = false;*/
            
            //Sets outline
            /*verdeTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            azulTxt.GetComponent<Outline>().DOColor(branco, 0.2f);
            amareloTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            vermelhoTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);*/
        }
        else if (!verde.GetComponent<ButtonSelection>().getPlaySelected()   && 
                 !azul.GetComponent<ButtonSelection>().getPlaySelected()    && 
                  amarelo.GetComponent<ButtonSelection>().getPlaySelected() &&
                 !vermelho.GetComponent<ButtonSelection>().getPlaySelected()&&
                 !amareloS)
        {
            verdeS = false;
            azulS = false;
            amareloS = true;
            vermelhoS = false;
            
            helperVerde.transform.DOLocalMoveX(0f,0.4f,false);
            helperAzul.transform.DOLocalMoveX(0f,0.4f,false);
            helperAmarelo.transform.DOLocalMoveX(85f,0.4f,false);
            helperVermelho.transform.DOLocalMoveX(0f,0.4f,false);
            
            /*
            verdeTxt.GetComponent<RectTransform>().DOAnchorPosX(-30f, 0.4f, false);
            azulTxt.GetComponent<RectTransform>().DOAnchorPosX(-30, 0.4f, false);
            amareloTxt.GetComponent<RectTransform>().DOAnchorPosX(100, 0.4f, false);
            vermelhoTxt.GetComponent<RectTransform>().DOAnchorPosX(-80, 0.4f, false);
            */
            
            Debug.Log("verdeS: " + verdeS);
            Debug.Log("azulS: " + azulS);
            Debug.Log("amareloS: " + amareloS);
            Debug.Log("vermelhoS: " + vermelhoS);
            
            //Set bloco de cor pos
            lines.DOAnchorPosY(-1080f, 0.5f, false).SetDelay(0.1f);
            
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
            
            outlinesVe[2].enabled = false;
            outlinesVe[3].enabled = false;
            
            outlinesAz[2].enabled = false;
            outlinesAz[3].enabled = false;
            
            outlinesAm[2].enabled = true;
            outlinesAm[3].enabled = true;
            
            outlinesVo[2].enabled = false;
            outlinesVo[3].enabled = false;
            
            /*
            outlinesVe[0].enabled = false;
            outlinesVe[1].enabled = false;
            outlinesVe[2].enabled = false;
            outlinesVe[3].enabled = false;
            
            outlinesAz[0].enabled = false;
            outlinesAz[1].enabled = false;
            outlinesAz[2].enabled = false;
            outlinesAz[3].enabled = false;
            
            outlinesAm[0].enabled = true;
            outlinesAm[1].enabled = true;
            outlinesAm[2].enabled = true;
            outlinesAm[3].enabled = true;
            
            outlinesVo[0].enabled = false;
            outlinesVo[1].enabled = false;
            outlinesVo[2].enabled = false;
            outlinesVo[3].enabled = false;*/

            //Sets outline
            /*verdeTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            azulTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            amareloTxt.GetComponent<Outline>().DOColor(branco, 0.2f);
            vermelhoTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);*/
        }
        else if (!verde.GetComponent<ButtonSelection>().getPlaySelected()   && 
                 !azul.GetComponent<ButtonSelection>().getPlaySelected()    && 
                 !amarelo.GetComponent<ButtonSelection>().getPlaySelected() &&
                  vermelho.GetComponent<ButtonSelection>().getPlaySelected()&&
                 !vermelhoS)
        {
            verdeS = false;
            azulS = false;
            amareloS = false;
            vermelhoS = true;
            
            helperVerde.transform.DOLocalMoveX(0f,0.4f,false);
            helperAzul.transform.DOLocalMoveX(0f,0.4f,false);
            helperAmarelo.transform.DOLocalMoveX(0f,0.4f,false);
            helperVermelho.transform.DOLocalMoveX(180f,0.4f,false);
            
            /*
            verdeTxt.GetComponent<RectTransform>().DOAnchorPosX(-30f, 0.4f, false);
            azulTxt.GetComponent<RectTransform>().DOAnchorPosX(-30, 0.4f, false);
            amareloTxt.GetComponent<RectTransform>().DOAnchorPosX(-30, 0.4f, false);
            vermelhoTxt.GetComponent<RectTransform>().DOAnchorPosX(110, 0.4f, false);
            */
            
            Debug.Log("verdeS: " + verdeS);
            Debug.Log("azulS: " + azulS);
            Debug.Log("amareloS: " + amareloS);
            Debug.Log("vermelhoS: " + vermelhoS);
            
            //Set bloco de cor pos
            lines.DOAnchorPosY(-3240f, 0.5f, false).SetDelay(0.1f);
            
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
            
            outlinesVe[2].enabled = false;
            outlinesVe[3].enabled = false;
            
            outlinesAz[2].enabled = false;
            outlinesAz[3].enabled = false;
            
            outlinesAm[2].enabled = false;
            outlinesAm[3].enabled = false;
            
            outlinesVo[2].enabled = true;
            outlinesVo[3].enabled = true;
            
            /*
            outlinesVe[0].enabled = false;
            outlinesVe[1].enabled = false;
            outlinesVe[2].enabled = false;
            outlinesVe[3].enabled = false;
            
            outlinesAz[0].enabled = false;
            outlinesAz[1].enabled = false;
            outlinesAz[2].enabled = false;
            outlinesAz[3].enabled = false;
            
            outlinesAm[0].enabled = false;
            outlinesAm[1].enabled = false;
            outlinesAm[2].enabled = false;
            outlinesAm[3].enabled = false;
            
            outlinesVo[0].enabled = true;
            outlinesVo[1].enabled = true;
            outlinesVo[2].enabled = true;
            outlinesVo[3].enabled = true;*/

            //Sets outline
            /*verdeTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            azulTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            amareloTxt.GetComponent<Outline>().DOColor(brancoInv, 0.2f);
            vermelhoTxt.GetComponent<Outline>().DOColor(branco, 0.2f);*/
        }
            
    }
}
