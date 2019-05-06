using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour{

    protected GameObject leftPlayer;
    protected GameObject rightPlayer;
    protected playerPlataformerController leftPlayerScript;
    protected playerPlataformerController rightPlayerScript;

    protected GameObject[] players;

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

        uiTexts[0].text = leftPlayerScript.characterName;
        uiTexts[1].text = rightPlayerScript.characterName;

        uiTexts[4].enabled = false;
    }

    // Update is called once per frame
    void Update(){
        uiTexts[2].text = leftPlayerScript.health.ToString();
        uiTexts[3].text = rightPlayerScript.health.ToString();

        if (leftPlayerScript.health <= 0){
            stopPlayer();
            uiTexts[4].enabled = true;
            uiTexts[4].text = rightPlayerScript.characterName + " wins";
            StartCoroutine(endCondition());
        } else if (rightPlayerScript.health <= 0){
            stopPlayer();
            uiTexts[4].enabled = true;
            uiTexts[4].text = leftPlayerScript.characterName + " wins";
            StartCoroutine(endCondition());
        }

        if (leftPlayer.transform.position.x == rightPlayer.transform.position.x){
            leftPlayerScript.flipCharacter();
            rightPlayerScript.flipCharacter();
        }
    }

    IEnumerator endCondition(){
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Arena");
    }

    void stopPlayer(){
        rightPlayerScript.StopAllAttack();
        rightPlayerScript.enableControls = false;
        leftPlayerScript.StopAllAttack();
        leftPlayerScript.enableControls = false;
    }
}
