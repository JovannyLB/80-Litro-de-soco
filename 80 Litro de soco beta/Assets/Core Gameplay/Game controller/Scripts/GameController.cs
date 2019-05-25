using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour{

    [Header("Controls everything else than the character interactions")]
    
    protected GameObject leftPlayer;
    protected GameObject rightPlayer;
    protected playerPlataformerController leftPlayerScript;
    protected playerPlataformerController rightPlayerScript;
    private int leftPlayerCombo;
    private int rightPlayerCombo;
    private bool isPaused;

    protected GameObject[] players;
    private bool flipToggle = true;
    private GameObject camera;

    public GameObject[] arenas;
    public Text[] uiTexts;
    public Image[] uiImages;

    public ParticleSystem[] particles;

    private bool hitStopCurrently;

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

        camera = GameObject.FindWithTag("MainCamera");
        
        leftPlayerScript = leftPlayer.transform.root.GetChild(0).GetComponent<playerPlataformerController>();
        rightPlayerScript = rightPlayer.transform.root.GetChild(0).GetComponent<playerPlataformerController>();

        rightPlayerScript.isFlippedSide = true;
        leftPlayerScript.isLeft = true;

        uiTexts[0].text = leftPlayerScript.characterName;
        uiTexts[1].text = rightPlayerScript.characterName;

        uiTexts[4].enabled = false;
        uiTexts[5].enabled = false;

        uiImages[0].color = leftPlayerScript.mainColor;
        uiImages[3].color = rightPlayerScript.mainColor;
        
        attachBlood();
    }

    // Update is called once per frame
    void Update(){
        CameraControl();
        
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
            uiTexts[5].enabled = false;
        }

        if (!hitStopCurrently && !isPaused){
            Time.timeScale = 1;
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

        PlayerHealth();
        comboCounter();

    }

    IEnumerator endCondition(){
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }

    void stopPlayer(){
        rightPlayerScript.StopAllAttack();
        rightPlayerScript.targetVelocity = Vector2.zero;
        rightPlayerScript.enableControls = false;
        leftPlayerScript.StopAllAttack();
        leftPlayerScript.targetVelocity = Vector2.zero;
        leftPlayerScript.enableControls = false;
    }

    void attachBlood(){
        AttackCheck[] bloodyAttacks = FindObjectsOfType<AttackCheck>();
        foreach (AttackCheck attack in bloodyAttacks){
            attack.blood = particles[0];
        }
    }

    public void CallHitStop(float frames, float time){
        StartCoroutine(hitStopStart(frames, time));
    }
    
    IEnumerator hitStopStart(float frames, float time){
        hitStopCurrently = true;
        Time.timeScale = time;
        
        yield return new WaitForSecondsRealtime(frames / 60f);

        Time.timeScale = 1;
        hitStopCurrently = false;
        
        leftPlayer.GetComponent<Animator>().speed = 1;
        rightPlayer.GetComponent<Animator>().speed = 1;
    }

    void comboCounter(){
        // Right player
        if (leftPlayerScript.lastHitStun <= 0){
            rightPlayerCombo = 0;
        }
        else if (leftPlayerScript.lastHitStun == leftPlayerScript.lastHitTaken){
            rightPlayerCombo++;
        }

        if (leftPlayerCombo <= 0){
            uiTexts[2].enabled = false;
        }
        else{
            uiTexts[2].enabled = true;
            uiTexts[2].text = leftPlayerCombo.ToString();
        }
        
        // Left player
        if (rightPlayerScript.lastHitStun <= 0){
            leftPlayerCombo = 0;
        }
        else if (rightPlayerScript.lastHitStun == rightPlayerScript.lastHitTaken){
            leftPlayerCombo++;
        }
        
        if (rightPlayerCombo <= 0){
            uiTexts[3].enabled = false;
        }
        else{
            uiTexts[3].enabled = true;
            uiTexts[3].text = rightPlayerCombo.ToString();
        }
    }

    void PlayerHealth(){
        // Left player
        if (leftPlayerScript.health > 0){
            uiImages[1].rectTransform.localScale = new Vector3(leftPlayerScript.health / leftPlayerScript.maxHealth, 1, 1);
            uiImages[2].rectTransform.DOScaleX(uiImages[1].rectTransform.localScale.x, 1);
        }
        else{
            uiImages[1].rectTransform.localScale = new Vector3(0, 1, 1);
            uiImages[2].rectTransform.DOScaleX(0, 1);
        }

        // Right player
        if (rightPlayerScript.health > 0){
            uiImages[4].rectTransform.localScale = new Vector3(rightPlayerScript.health / rightPlayerScript.maxHealth, 1, 1);
            uiImages[5].rectTransform.DOScaleX(uiImages[4].rectTransform.localScale.x, 1);
        }
        else{
            uiImages[4].rectTransform.localScale = new Vector3(0, 1, 1);
            uiImages[5].rectTransform.DOScaleX(0, 1);
        }

        // Win condition
        if (leftPlayerScript.health <= 0){
            leftPlayer.GetComponent<Animator>().speed = 0;
            rightPlayer.GetComponent<Animator>().speed = 0;
            StartCoroutine(hitStopStart(60f, 0.05f));
            stopPlayer();
            uiTexts[4].enabled = true;
            uiTexts[4].text = rightPlayerScript.characterName + " wins";
            rightPlayerScript.won = true;
            leftPlayerScript.lost = true;
            
            StartCoroutine(endCondition());
        } else if (rightPlayerScript.health <= 0){
            leftPlayer.GetComponent<Animator>().speed = 0;
            rightPlayer.GetComponent<Animator>().speed = 0;
            StartCoroutine(hitStopStart(60f, 0.05f));
            stopPlayer();
            uiTexts[4].enabled = true;
            uiTexts[4].text = leftPlayerScript.characterName + " wins";
            leftPlayerScript.won = true;
            rightPlayerScript.lost = true;
            
            StartCoroutine(endCondition());
        }
    }

    void CameraControl(){
        camera.transform.position = new Vector3((leftPlayer.transform.position.x + rightPlayer.transform.position.x) / 2, camera.transform.position.y, camera.transform.position.z);
        
        float diffPos = Math.Abs(leftPlayer.transform.position.x - rightPlayer.transform.position.x);
        
        if (diffPos >= 30){
            camera.GetComponent<Camera>().orthographicSize = diffPos / 3f;
        }
        else{
            camera.GetComponent<Camera>().orthographicSize = 10;
        }

        if (diffPos >= 50){
            leftPlayerScript.canMoveBack = false;
            rightPlayerScript.canMoveBack = false;
        }
        else{
            leftPlayerScript.canMoveBack = true;
            rightPlayerScript.canMoveBack = true;
        }

    }
}
