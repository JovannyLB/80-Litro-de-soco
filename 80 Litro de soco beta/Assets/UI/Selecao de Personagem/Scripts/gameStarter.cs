using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameStarter : MonoBehaviour
{
    public GameObject eventController0;
    public GameObject eventController1;

    public static Mensageiro mensageiro;

    private void Start()
    {
        //Pega o mensageiro de um gameObject na cena
        mensageiro = GameObject.FindWithTag("Mensageiro").GetComponent<Mensageiro>();
    }

    private void Update()
    {
        
        if (eventController0.GetComponent<testeSelecao>().playerReady && 
            eventController1.GetComponent<testeSelecao>().playerReady)
        {
            mensageiro.SetLeftPlayerIndex(1);
            mensageiro.SetRightPlayerIndex(1);
            /*mensageiro.SetLeftPlayerIndex(eventController0.GetComponent<testeSelecao>().GetPlayerSelected());
            mensageiro.SetRightPlayerIndex(eventController1.GetComponent<testeSelecao>().GetPlayerSelected());*/
            StartCoroutine(DoWaitTest());
        }
    }
    
    IEnumerator DoWaitTest()
    {
       
        yield return (new WaitForSeconds(1f));
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
