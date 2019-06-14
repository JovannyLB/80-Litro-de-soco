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
            eventController1.GetComponent<testeSelecao>().playerReady){
            mensageiro.SetLeftPlayerIndex(eventController0.GetComponent<testeSelecao>().GetPlayerSelected());
            mensageiro.SetRightPlayerIndex(eventController1.GetComponent<testeSelecao>().GetPlayerSelected());
            
            StartCoroutine(DoWaitTest());
        }
    }
    
    IEnumerator DoWaitTest()
    {
        print(eventController0.GetComponent<testeSelecao>().GetPlayerSelected());
        print(eventController1.GetComponent<testeSelecao>().GetPlayerSelected());
        yield return (new WaitForSeconds(1f));
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
