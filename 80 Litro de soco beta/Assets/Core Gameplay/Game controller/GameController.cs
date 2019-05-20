using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour{

    [Header("Controls everything else than the character interactions")]
    
    protected GameObject leftPlayer;
    protected GameObject rightPlayer;
    protected playerPlataformerController leftPlayerScript;
    protected playerPlataformerController rightPlayerScript;
    private bool isPaused;

    protected GameObject[] players;
    private bool flipToggle = true;

    public Text[] uiTexts;
    
    void Start(){
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players){
            if (player.transform.root.GetChild(0).GetComponent<playerPlataformerController>().isPlayer1){
                leftPlayer = player;
            }
            else{
                rightPlayer = player;
            }
        }
        leftPlayerScript = leftPlayer.transform.root.GetChild(0).GetComponent<playerPlataformerController>();
        rightPlayerScript = rightPlayer.transform.root.GetChild(0).GetComponent<playerPlataformerController>();

        rightPlayerScript.isFlippedSide = true;
        leftPlayerScript.isLeft = true;

        uiTexts[0].text = leftPlayerScript.characterName;
        uiTexts[1].text = rightPlayerScript.characterName;

        uiTexts[4].enabled = false;
        uiTexts[5].enabled = false;
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.JoystickButton9) && !isPaused){
            isPaused = true;
        } else if (Input.GetKeyDown(KeyCode.JoystickButton9) && isPaused){
            isPaused = false;
        }

        if (isPaused){
            Time.timeScale = 0;
            uiTexts[5].enabled = true;
        }
        else{
            Time.timeScale = 1;
            uiTexts[5].enabled = false;
        }

        uiTexts[2].text = leftPlayerScript.health.ToString();
        uiTexts[3].text = rightPlayerScript.health.ToString();

        if (leftPlayerScript.health <= 0){
            stopPlayer();
            uiTexts[4].enabled = true;
            uiTexts[4].text = rightPlayerScript.characterName + " wins";
            rightPlayerScript.won = true;
            leftPlayerScript.lost = true;
            StartCoroutine(endCondition());
        } else if (rightPlayerScript.health <= 0){
            stopPlayer();
            uiTexts[4].enabled = true;
            uiTexts[4].text = leftPlayerScript.characterName + " wins";
            leftPlayerScript.won = true;
            rightPlayerScript.lost = true;
            StartCoroutine(endCondition());
        }

        var onTopLeft = leftPlayer.transform.root.GetChild(2).GetChild(4).GetComponent<jumpOverCheck>().onTop;
        var onTopRight = rightPlayer.transform.root.GetChild(2).GetChild(4).GetComponent<jumpOverCheck>().onTop;
        
        if (leftPlayerScript.posX > rightPlayerScript.posX && flipToggle && !onTopLeft && !onTopRight){
            leftPlayerScript.isLeft = false;
            rightPlayerScript.isLeft = true;
            leftPlayerScript.flipCharacterLeft();
            rightPlayerScript.flipCharacterRight();
            flipToggle = false;
        }
        else if (leftPlayerScript.posX < rightPlayerScript.posX && !flipToggle && !onTopLeft && !onTopRight){
            leftPlayerScript.isLeft = true;
            rightPlayerScript.isLeft = false;
            leftPlayerScript.flipCharacterRight();
            rightPlayerScript.flipCharacterLeft();
            flipToggle = true;
        }

    }

    IEnumerator endCondition(){
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }

    void stopPlayer(){
        rightPlayerScript.StopAllAttack();
        rightPlayerScript.enableControls = false;
        leftPlayerScript.StopAllAttack();
        leftPlayerScript.enableControls = false;
    }
}
