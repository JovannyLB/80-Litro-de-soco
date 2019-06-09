using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class testeSelecao1 : MonoBehaviour
{

    public GameObject personagem0;

    public GameObject helper0;
    public GameObject helper1;
    public GameObject helper2;
    public GameObject helper3;
    public GameObject helper4;
    public GameObject helper5;
    public GameObject helper6;
    
    private List<GameObject> arrayPersonagens;
    private List<GameObject> arrayPersonagensTela;
    private List<GameObject> arrayHelpers;

    public bool playerReady;

    private int j;
    private bool waiting;
    
    //Wait
    WaitForSecondsRealtime waitForSecondsRealtime;

    public testeSelecao1()
    {
        arrayHelpers = new List<GameObject>();
        arrayPersonagens = new List<GameObject>();
        arrayPersonagensTela = new List<GameObject>(6);
        waiting = false;
    }

    private void Start()
    {
        playerReady = false;
        
        //Funcionando mas fora do teste atual
        arrayHelpers.Add(helper0);
        arrayHelpers.Add(helper1);
        arrayHelpers.Add(helper2);
        arrayHelpers.Add(helper3);
        arrayHelpers.Add(helper4);
        arrayHelpers.Add(helper5);
        arrayHelpers.Add(helper6);

        arrayPersonagens.Add(personagem0);
        
        arrayPersonagensTela.Add(null);
        arrayPersonagensTela.Add(null);
        arrayPersonagensTela.Add(null);
        arrayPersonagensTela.Add(null);
        arrayPersonagensTela.Add(null);
        arrayPersonagensTela.Add(null);
        arrayPersonagensTela.Add(null);

        j = 0;


        for (int i = 0; arrayPersonagensTela[6] == null; i++)
        {
            try
            {
                //Cria o objeto
                arrayPersonagensTela[i] = Instantiate(
                    arrayPersonagens[j],
                    new Vector3(0f, 0f, 0f),
                    new Quaternion(0f,0f,0f,0f),
                    arrayHelpers[i].transform);
                
                arrayPersonagensTela[i].GetComponent<IndexTest>().SetIndex(j);
                //Provavelmente n aparece no inspetor pois o codigo nao esta no update, 
                //mas o index é sim alterado como comprova o debug.log
                //Debug.Log("INDEX: "+arrayPersonagensTela[i].GetComponent<IndexTest>().index);
                
                //Reseta a posicao
                arrayPersonagensTela[i].GetComponent<RectTransform>().localPosition = new Vector3(
                    0f,
                    0f,
                    /*arrayPersonagensTela[i].GetComponent<RectTransform>().localPosition.z*/
                    0f);
                
                
                j++;
            }
            catch (Exception e)
            {
                j = 0;
                arrayPersonagensTela[i] = Instantiate(
                    arrayPersonagens[j],
                    new Vector3(0f, 0f, 0f),
                    new Quaternion(0f,0f,0f,0f),
                    arrayHelpers[i].transform);
                
                
                //Debug.Log("INDEX: "+arrayPersonagensTela[i].GetComponent<IndexTest>().index);
                
                arrayPersonagensTela[i].GetComponent<RectTransform>().localPosition = new Vector3(
                    0f,
                    0f,
                    /*arrayPersonagensTela[i].GetComponent<RectTransform>().localPosition.z*/
                    0f);
                j++;
                
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !waiting)
        {
            waiting = true;
            OnDownPressed();
            MovingBetweenHelpers(false);
            StartCoroutine(DoWaitTest());
        }
        
        else if (Input.GetKeyDown(KeyCode.W) && !waiting)
        {
            waiting = true;
            OnUpPressed();
            MovingBetweenHelpers(true);
            StartCoroutine(DoWaitTest());
        }
        
        arrayPersonagensTela[3].GetComponent<Button>().Select();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerReady = !playerReady;
            Debug.Log("ENTER APERTADO");
        }

    }

    private void OnDownPressed()
    {
        Debug.Log("BaixoPressionado");

        for (int i = 0; i < arrayPersonagensTela.Count; i++)
        {
            try
            {
                arrayPersonagensTela[i].transform.SetParent(arrayHelpers[i + 1].transform);
            }
            catch(Exception e)
            {
                try
                {
                    int objeto = arrayPersonagensTela[0].GetComponent<IndexTest>().GetIndex();

                    if (objeto == 0)
                    {
                        objeto = arrayPersonagens.Count - 1;
                    }
                    else
                    {
                        objeto -= 1;
                    } // Nao é o problema

                    Destroy(arrayPersonagensTela[i], 0f); //Nao é o problema

                    arrayPersonagensTela[i] = null;
                    

                    arrayPersonagensTela[i]     = arrayPersonagensTela[i - 1];
                    arrayPersonagensTela[i - 1] = arrayPersonagensTela[i - 2];
                    arrayPersonagensTela[i - 2] = arrayPersonagensTela[i - 3];
                    arrayPersonagensTela[i - 3] = arrayPersonagensTela[i - 4];
                    arrayPersonagensTela[i - 4] = arrayPersonagensTela[i - 5];
                    arrayPersonagensTela[i - 5] = arrayPersonagensTela[i - 6];
                    arrayPersonagensTela[i - 6] = Instantiate(
                        arrayPersonagens[objeto].gameObject, //Nao é o problema
                        new Vector3(-100f, 0f, 0f),
                        new Quaternion(0f, 0f, 0f, 0f),
                        arrayHelpers[0].transform
                    );
                    
                    arrayPersonagensTela[0].GetComponent<IndexTest>().SetIndex(objeto);
                    //Instantiate dentro de Insert nao funciona
                }
                catch (Exception e2)
                {
                }
            }
        }
    }
    
    private void OnUpPressed()
    {
        Debug.Log("CimaPressionado");
        for (int i = 6; i > -1; i--)
        {
            try
            {
                arrayPersonagensTela[i].transform.SetParent(arrayHelpers[i - 1].transform);
            }
            catch(Exception e)
            {
                try
                {
                    int objeto = arrayPersonagensTela[arrayPersonagensTela.Count - 1].GetComponent<IndexTest>().GetIndex();

                    if (objeto == arrayPersonagens.Count - 1)
                    {
                        objeto = 0;
                    }
                    else
                    {
                        objeto += 1;
                    } // Nao é o problema

                    Destroy(arrayPersonagensTela[0], 0f); //Nao é o problema


                    arrayPersonagensTela[0] = arrayPersonagensTela[1];
                    arrayPersonagensTela[1] = arrayPersonagensTela[2];
                    arrayPersonagensTela[2] = arrayPersonagensTela[3];
                    arrayPersonagensTela[3] = arrayPersonagensTela[4];
                    arrayPersonagensTela[4] = arrayPersonagensTela[5];
                    arrayPersonagensTela[5] = arrayPersonagensTela[6];
                    
                    arrayPersonagensTela[6] = Instantiate(
                        arrayPersonagens[objeto].gameObject, //Nao é o problema
                        new Vector3(0f, 0f, 0f),
                        new Quaternion(0f, 0f, 0f, 0f),
                        arrayHelpers[6].transform
                    );
                    
                    arrayPersonagensTela[6].GetComponent<IndexTest>().SetIndex(objeto);
                    //Instantiate dentro de Insert nao funciona
                }
                catch (Exception e2)
                {
                }
            }
        }
    }

    private void MovingBetweenHelpers(bool up)
    {
        for (int i = 0; i < arrayPersonagensTela.Count; i++)
        {
            if (up)
            {
                float index = 0f;
                
                if (i == 0)
                {
                    index = 0.25f;
                }if (i == 1)
                {
                    index = 0.5f;
                }if (i == 2)
                {
                    index = 0.75f;
                }if (i == 3)
                {
                    index = 1f;
                }if (i == 4)
                {
                    index = 0.75f;
                }

                if (i == 5)
                {
                    index = 0.5f;
                }
                
                if (i == 6)
                {
                    index = 0.25f;
                    arrayPersonagensTela[i].GetComponent<RectTransform>().DOScale(new Vector3(index, index, index), 0.25f);
                    arrayPersonagensTela[i].GetComponent<RectTransform>().localPosition = new Vector2(0f,0f);
                    
                }
                else
                {
                    arrayPersonagensTela[i].GetComponent<RectTransform>().DOScale(new Vector3(index, index, index), 0.25f);
                    arrayPersonagensTela[i].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f,0f),0.25f);
                }
                
                
                
            }
            else
            {
                float index = 0f;
                
                if (i == 0)
                {
                    index = 0.25f;
                }else if (i == 1)
                {
                    index = 0.5f;
                }else if (i == 2)
                {
                    index = 0.75f;
                }else if (i == 3)
                {
                    index = 1f;
                }else if (i == 4)
                {
                    index = 0.75f;
                }

                else if (i == 5)
                {
                    index = 0.5f;
                }
                
                else if (i == 6)
                {
                    index = 0.25f;
                }

                arrayPersonagensTela[i].GetComponent<RectTransform>().DOScale(new Vector3(index, index, index), 0.25f);
                arrayPersonagensTela[i].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f,0f), 0.25f);
            }
            
            
        }
    }
    
    IEnumerator DoWaitTest()
    {
       
        yield return (new WaitForSeconds(0.25f));
        waiting = false;
    }
}
