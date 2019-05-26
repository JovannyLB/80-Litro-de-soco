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
    private bool doneCameraEffect;

    void Start(){
        // Arrumas os player
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players){
            if (player.transform.root.GetChild(0).GetComponent<playerPlataformerController>().isPlayer1){
                leftPlayer = player;
            }
            else{
                rightPlayer = player;
            }
        }

        // Acha a camera
        camera = GameObject.FindWithTag("MainCamera");
        
        // Pega os scripts dos players
        leftPlayerScript = leftPlayer.transform.root.GetChild(0).GetComponent<playerPlataformerController>();
        rightPlayerScript = rightPlayer.transform.root.GetChild(0).GetComponent<playerPlataformerController>();

        // Vira os personagens
        rightPlayerScript.isFlippedSide = true;
        leftPlayerScript.isLeft = true;

        // Coloca os nomes
        uiTexts[0].text = leftPlayerScript.characterName;
        uiTexts[1].text = rightPlayerScript.characterName;

        // Desativa pause e vitória
        uiTexts[4].enabled = false;
        uiTexts[5].enabled = false;

        // Colocam a cor
        uiImages[0].color = leftPlayerScript.mainColor;
        uiImages[3].color = rightPlayerScript.mainColor;
        
        // Coloca o sangue nos prefabs
        AttachBlood();
    }

    // Update is called once per frame
    void Update(){
        // Camera
        CameraControl();
        
        // Pausa o jogo
        if (Input.GetKeyDown(KeyCode.JoystickButton9) && !isPaused){
            isPaused = true;
        } else if (Input.GetKeyDown(KeyCode.JoystickButton9) && isPaused){
            isPaused = false;
        }

        // Pausa o jogo
        if (isPaused){
            Time.timeScale = 0;
            leftPlayer.GetComponent<Animator>().speed = 0;
            rightPlayer.GetComponent<Animator>().speed = 0;
            leftPlayerScript.enableControls = false;
            rightPlayerScript.enableControls = false;
            uiTexts[5].enabled = true;
        }
        else{
            leftPlayerScript.enableControls = true;
            rightPlayerScript.enableControls = true;
            uiTexts[5].enabled = false;
        }

        if (!hitStopCurrently && !isPaused){
            Time.timeScale = 1;
            leftPlayer.GetComponent<Animator>().speed = 1;
            rightPlayer.GetComponent<Animator>().speed = 1;
        }

        
        // Checa a troca de lados
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
    
        // Cuida da vida
        PlayerHealth();
        
        // Cuida do contador de combo
        ComboCounter();
    }

    IEnumerator EndCondition(){
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }

    void StopPlayer(){
        rightPlayerScript.StopAllAttack();
        rightPlayerScript.targetVelocity = Vector2.zero;
        rightPlayerScript.enableControls = false;
        leftPlayerScript.StopAllAttack();
        leftPlayerScript.targetVelocity = Vector2.zero;
        leftPlayerScript.enableControls = false;
    }

    void AttachBlood(){
        AttackCheck[] bloodyAttacks = FindObjectsOfType<AttackCheck>();
        foreach (AttackCheck attack in bloodyAttacks){
            attack.blood = particles[0];
        }
    }

    public void CallHitStop(float frames, float time){
        StartCoroutine(HitStopStart(frames, time));
    }
    
    IEnumerator HitStopStart(float frames, float time){
        // Para o tempo
        hitStopCurrently = true;
        Time.timeScale = time;
        
        // Espera por frames
        yield return new WaitForSecondsRealtime(frames / 60f);

        // Volta o tempo
        Time.timeScale = 1;
        hitStopCurrently = false;
        
        // Volta as animações
        leftPlayer.GetComponent<Animator>().speed = 1;
        rightPlayer.GetComponent<Animator>().speed = 1;
    }

    void ComboCounter(){
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
            PlayerVictory(leftPlayerScript, rightPlayerScript);
        } else if (rightPlayerScript.health <= 0){
            PlayerVictory(rightPlayerScript, leftPlayerScript);
        }
    }

    void PlayerVictory(playerPlataformerController losingPlayerScript, playerPlataformerController winningPlayerScript){
        // Congela a nimação
        leftPlayer.GetComponent<Animator>().speed = 0;
        rightPlayer.GetComponent<Animator>().speed = 0;
            
        // Faz o efeito final
        if (!GetComponent<PostProcessing>().doneEffect){
            GetComponent<PostProcessing>().DeathVignette(true);
        }
        else{
            GetComponent<PostProcessing>().DeathVignette(false);
        }

        // Faz o efeito final da camera
        if (!doneCameraEffect){
            GameObject.FindWithTag("MainCamera").transform.DOMoveY(((leftPlayer.transform.position.y + rightPlayer.transform.position.y) / 2) + 2, 0.1f).OnComplete(() => { doneCameraEffect = true; });
            if (losingPlayerScript.isLeft){
                GameObject.FindWithTag("MainCamera").transform.DORotate(new Vector3(0, 0, 5), 0.1f);
            }
            else{
                GameObject.FindWithTag("MainCamera").transform.DORotate(new Vector3(0, 0, -5), 0.1f);
            }
        }
        else{
            GameObject.FindWithTag("MainCamera").transform.DOMoveY(0, 0.1f);
            GameObject.FindWithTag("MainCamera").transform.DORotate(new Vector3(0, 0, 0), 0.1f).OnComplete(() => {
                winningPlayerScript.won = true;
                losingPlayerScript.lost = true;
                StopPlayer();
            });
        }

        // Da o hitstop longo
        StartCoroutine(HitStopStart(120f, 0.05f));
        uiTexts[4].enabled = true;
        uiTexts[4].text = winningPlayerScript.characterName + " wins";
            
        // Termina o jogo
        StartCoroutine(EndCondition());
    }

    void CameraControl(){
        // Faz a camera ficar entre os dois personages
        camera.transform.position = new Vector3((leftPlayer.transform.position.x + rightPlayer.transform.position.x) / 2, camera.transform.position.y, camera.transform.position.z);
        
        // Pega a difereça dos dois players
        float diffPos = Math.Abs(leftPlayer.transform.position.x - rightPlayer.transform.position.x);
        
        // Se a difereça for maior que 30 a camera muda de tamanho
        if (diffPos >= 30){
            camera.GetComponent<Camera>().orthographicSize = diffPos / 3f;
        }
        else{
            camera.GetComponent<Camera>().orthographicSize = 10;
        }

        // Se a diferença for maior ou igual a 40 o personagem não pode ir mais para trás
        if (diffPos >= 40){
            leftPlayerScript.canMoveBack = false;
            rightPlayerScript.canMoveBack = false;
        }
        else{
            leftPlayerScript.canMoveBack = true;
            rightPlayerScript.canMoveBack = true;
        }

    }
}
