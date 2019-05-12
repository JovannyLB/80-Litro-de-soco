using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void ExtraGame()
    {
        SceneManager.LoadScene(2);
    }
    
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    
    /*
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }*/
}
