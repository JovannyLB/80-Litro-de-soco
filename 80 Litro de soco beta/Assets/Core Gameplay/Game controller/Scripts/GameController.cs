using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour{

    [Header("Controls everything else than the character interactions")]
    
    protected GameObject leftPlayer;
    protected GameObject rightPlayer;
    protected playerPlataformerController leftPlayerScript;
    protected playerPlataformerController rightPlayerScript;
    private int leftPlayerCombo;
    private int leftPlayerMaxCombo;
    private int rightPlayerCombo;
    private int rightPlayerMaxCombo;
    private bool isPaused;

    private bool gameRunning;
    private int rounds = 1;

    private int leftPlayerChosen;
    private int rightPlayerChosen;
    public GameObject[] players;
    private bool flipToggle = true;
    private GameObject sceneCamera;

    public GameObject[] arenas;
    [HideInInspector]public bool currentlyBGO;
    public Text[] uiTexts;
    public Image[] uiImages;
    public GameObject[] spawnPoints;

    public ParticleSystem[] particles;

    private bool hitStopCurrently;
    private bool timeFreeze;
    
    private bool doneCameraEffect;
    private float alpha;
    public bool introDone;

    private float totalTime = 99;
    private float timeLeft;

    private Coroutine hitStopCoroutine;

    void Start(){
        // Coloca o mapa
        Instantiate(arenas[0]);
        
        arenas[0].GetComponent<ArenaScript>().SpawnInteractable(spawnPoints[2].transform.position);
        arenas[0].GetComponent<ArenaScript>().SpawnInteractable(spawnPoints[3].transform.position);

        timeLeft = totalTime;
        uiTexts[10].text = timeLeft.ToString();
        
        // Arrumas os player
        leftPlayerChosen = Mensageiro.leftPlayerIndex;
        rightPlayerChosen = Mensageiro.rightPlayerIndex;
        
        var leftPlayerPreFab = Instantiate(players[leftPlayerChosen]);
        leftPlayer = leftPlayerPreFab.transform.GetChild(0).gameObject;
        leftPlayerScript = leftPlayerPreFab.transform.GetChild(0).GetComponent<playerPlataformerController>();
        leftPlayerScript.isPlayer1 = true;
        leftPlayerScript.gameController = this;

        var rightPlayerPreFab = Instantiate(players[rightPlayerChosen]);
        rightPlayer = rightPlayerPreFab.transform.GetChild(0).gameObject;
        rightPlayerScript = rightPlayerPreFab.transform.GetChild(0).GetComponent<playerPlataformerController>();
        rightPlayerScript.isPlayer1 = false;
        rightPlayerScript.gameController = this;
        
        // Acha a camera
        sceneCamera = GameObject.FindWithTag("MainCamera");
        
        // Pega os scripts dos players
        leftPlayerScript = leftPlayer.transform.root.GetChild(0).GetComponent<playerPlataformerController>();
        rightPlayerScript = rightPlayer.transform.root.GetChild(0).GetComponent<playerPlataformerController>();
        
        leftPlayerScript.MoveTo(spawnPoints[0].transform.position);
        rightPlayerScript.MoveTo(spawnPoints[1].transform.position);

        // Vira os personagens
        rightPlayerScript.isLeft = false;
        leftPlayerScript.isLeft = true;
        
        rightPlayerScript.flipCharacterLeft();
        leftPlayerScript.flipCharacterRight();

        // Coloca os nomes
        uiTexts[0].text = leftPlayerScript.characterName;
        uiTexts[1].text = rightPlayerScript.characterName;

        // Desativa pause e vitória
        uiTexts[4].enabled = false;
        uiTexts[5].enabled = false;

        uiTexts[6].gameObject.SetActive(false);
        uiTexts[7].gameObject.SetActive(false);
        uiTexts[8].gameObject.SetActive(false);
        uiTexts[9].gameObject.SetActive(false);
        
        uiTexts[2].gameObject.SetActive(false);
        uiTexts[3].gameObject.SetActive(false);

        // Colocam a cor
        uiImages[0].color = leftPlayerScript.mainColor;
        uiTexts[0].color = leftPlayerScript.mainColor;
        uiTexts[2].color = leftPlayerScript.mainColor;
        uiImages[6].color = leftPlayerScript.mainColor;
        
        uiTexts[1].color = rightPlayerScript.mainColor;
        uiTexts[3].color = rightPlayerScript.mainColor;
        uiImages[3].color = rightPlayerScript.mainColor;
        uiImages[10].color = rightPlayerScript.mainColor;

        
        transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.black;

        // Coloca o sangue nos prefabs
        AttachParticles();

        StartCoroutine(StartCondition());

        /*// Start settings
        camera.GetComponent<Camera>().orthographicSize = 15;
        foreach (var text in uiTexts){
            text.GetComponent<CanvasRenderer>().SetAlpha(0);
        }
        foreach (var images in uiImages){
            images.GetComponent<CanvasRenderer>().SetAlpha(0);
        }
        uiImages[2].rectTransform.localScale = new Vector3(0, uiImages[2].rectTransform.localScale.y, uiImages[2].rectTransform.localScale.z);
        uiImages[1].rectTransform.localScale = new Vector3(0, uiImages[1].rectTransform.localScale.y, uiImages[1].rectTransform.localScale.z);
        
        uiImages[5].rectTransform.localScale = new Vector3(0, uiImages[5].rectTransform.localScale.y, uiImages[5].rectTransform.localScale.z);
        uiImages[4].rectTransform.localScale = new Vector3(0, uiImages[4].rectTransform.localScale.y, uiImages[4].rectTransform.localScale.z);*/
    }

    // Update is called once per frame
    void Update(){
        // Cuida da vida
        PlayerHealth();
        
        PlayerBar();
        
        // Camera
        CameraControl();
        
        // BGOs do mapa
        MapaBGO();
        
        // Pausa o jogo
        if (Input.GetKeyDown(KeyCode.JoystickButton9) && !isPaused){
            isPaused = true;
        } else if (Input.GetKeyDown(KeyCode.JoystickButton9) && isPaused){
            isPaused = false;
        }

        if (Input.GetKeyDown(KeyCode.Backspace)){
            TDEEffect(rightPlayerScript);
        }

        // Pausa o jogo
        if (isPaused){
            Time.timeScale = 0;
            leftPlayerScript.animator.speed = 0;
            rightPlayerScript.animator.speed = 0;
            leftPlayerScript.enableControls = false;
            rightPlayerScript.enableControls = false;
            uiTexts[5].enabled = true;
        }
        else{
            uiTexts[5].enabled = false;
        }
        
        if (!hitStopCurrently && !isPaused && !timeFreeze){
            Time.timeScale = 1;
            leftPlayerScript.animator.speed = 1;
            rightPlayerScript.animator.speed = 1;
        }

        
        // Checa a troca de lados
        var onTopLeft = leftPlayer.transform.root.GetChild(2).GetChild(4).GetComponent<jumpOverCheck>().onTop;
        var onTopRight = rightPlayer.transform.root.GetChild(2).GetChild(4).GetComponent<jumpOverCheck>().onTop;

        if (leftPlayerScript.inCorner || rightPlayerScript.inCorner){
            if (leftPlayerScript.posX > rightPlayerScript.posX && flipToggle && !onTopLeft && !onTopRight && !leftPlayerScript.currentlyAttacking && !rightPlayerScript.currentlyAttacking && leftPlayerScript.grounded && rightPlayerScript.grounded){
                leftPlayerScript.isLeft = false;
                rightPlayerScript.isLeft = true;
                leftPlayerScript.flipCharacterLeft();
                rightPlayerScript.flipCharacterRight();
                flipToggle = false;
            }
            else if (leftPlayerScript.posX < rightPlayerScript.posX && !flipToggle && !onTopLeft && !onTopRight && !leftPlayerScript.currentlyAttacking && !rightPlayerScript.currentlyAttacking && leftPlayerScript.grounded && rightPlayerScript.grounded){
                leftPlayerScript.isLeft = true;
                rightPlayerScript.isLeft = false;
                leftPlayerScript.flipCharacterRight();
                rightPlayerScript.flipCharacterLeft();
                flipToggle = true;
            }
        }
        else{
            if (leftPlayerScript.posX > rightPlayerScript.posX && flipToggle && !onTopLeft && !onTopRight && !leftPlayerScript.currentlyAttacking && !rightPlayerScript.currentlyAttacking){
                leftPlayerScript.isLeft = false;
                rightPlayerScript.isLeft = true;
                leftPlayerScript.flipCharacterLeft();
                rightPlayerScript.flipCharacterRight();
                flipToggle = false;
            }
            else if (leftPlayerScript.posX < rightPlayerScript.posX && !flipToggle && !onTopLeft && !onTopRight && !leftPlayerScript.currentlyAttacking && !rightPlayerScript.currentlyAttacking){
                leftPlayerScript.isLeft = true;
                rightPlayerScript.isLeft = false;
                leftPlayerScript.flipCharacterRight();
                rightPlayerScript.flipCharacterLeft();
                flipToggle = true;
            }
        }

        // Cuida do contador de combo
        ComboCounter();
        
        // Seleciona as coisas que acontecem quando o jogo roda
        if (gameRunning && !isPaused){
            // Controle
            leftPlayerScript.enableControls = true;
            rightPlayerScript.enableControls = true;
            timeLeft -= Time.deltaTime;
            if (timeLeft > 0){
                uiTexts[10].text = timeLeft.ToString("0");
            }
            else{
                uiTexts[10].text = 0.ToString("0");
            }
        }
        else{
            leftPlayerScript.enableControls = false;
            rightPlayerScript.enableControls = false;
        }

        leftPlayerScript.gameRunning = gameRunning;
        rightPlayerScript.gameRunning = gameRunning;

        if (leftPlayerScript.jumpingOver || leftPlayerScript.beingJumpedOver){
            leftPlayerScript.enableControls = false;
            leftPlayerScript.StopInput();
        }
        
        if (rightPlayerScript.jumpingOver || rightPlayerScript.beingJumpedOver){
            rightPlayerScript.enableControls = false;
            rightPlayerScript.StopInput();
        }

        /*if (!introDone){
            DOTween.To(() => GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize, x => GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize = x, 10f, 2f);
            DOTween.To(() => alpha, x => alpha = x, 1f, 2f);
            
            uiImages[2].rectTransform.DOScaleX(1, 2).SetDelay(2);
            uiImages[5].rectTransform.DOScaleX(1, 2).SetDelay(2);
            uiImages[1].rectTransform.DOScaleX(1, 2).SetDelay(2);
            uiImages[4].rectTransform.DOScaleX(1, 2).SetDelay(2);
            
            foreach (var text in uiTexts){
                text.GetComponent<CanvasRenderer>().SetAlpha(alpha);
            }
            foreach (var images in uiImages){
                images.GetComponent<CanvasRenderer>().SetAlpha(alpha);
            }
            StartCoroutine(StartCondition());
        }*/
    }

    IEnumerator EndCondition(){
        yield return new WaitForSeconds(5);
        FadeOut();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }

    IEnumerator StartCondition(){
        FadeIn();
        yield return new WaitForSeconds(1);
        uiTexts[4].enabled = true;
        uiTexts[4].text = "Round " + rounds;
        yield return new WaitForSeconds(1);
        uiTexts[4].enabled = false;
        yield return new WaitForSeconds(0.5f);
        uiTexts[4].enabled = true;
        gameRunning = true;
        uiTexts[4].text = "FIGHT!";
        yield return new WaitForSeconds(0.5f);
        uiTexts[4].enabled = false;
/*
        introDone = true;
        gameRunning = true;
*/
    }

    void StopPlayer(playerPlataformerController player){
        player.StopAllAttack();
        player.targetVelocity = Vector2.zero;
        player.StopInput();
    }

    void AttachParticles(){
        AttackCheck[] bloodyAttacks = FindObjectsOfType<AttackCheck>();
        foreach (AttackCheck attack in bloodyAttacks){
            attack.blood = particles[0];
            attack.hitSplash = particles[2];
        }

        projectileSpecialCheck[] projectileAttacks = FindObjectsOfType<projectileSpecialCheck>();
        foreach (projectileSpecialCheck attack in projectileAttacks){
            attack.blood = particles[0];
        }
        
        leftPlayerScript.block = particles[1];
        leftPlayerScript.blockSplash = particles[3];
        
        rightPlayerScript.block = particles[1];
        rightPlayerScript.blockSplash = particles[3];
    }

    public void CallHitStop(float frames, float time){
        hitStopCoroutine = StartCoroutine(HitStopStart(frames, time));
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
        leftPlayerScript.animator.speed = 1;
        rightPlayerScript.animator.speed = 1;
    }

    private bool comboFadeDoneLeft;
    private bool comboFadeDoneRight;
    
    void ComboCounter(){
        // Right player
        if (leftPlayerScript.lastHitStun <= 0){
            rightPlayerCombo = 0;
        }
        else if (leftPlayerScript.lastHitStun == leftPlayerScript.lastHitTaken - 1){
            rightPlayerCombo++;
        }

        if (leftPlayerCombo <= 1){
            if (!comboFadeDoneLeft){
                StartCoroutine(FadeCombo(uiTexts[2], leftPlayerMaxCombo));
                comboFadeDoneLeft = true;
            }
        }
        else{
            uiTexts[2].gameObject.SetActive(true);
            DOTween.To(() => uiTexts[2].color, x => uiTexts[2].color = x, leftPlayerScript.mainColor, 0.5f);
            uiTexts[2].text = leftPlayerCombo + " HITS!";
            leftPlayerMaxCombo = leftPlayerCombo;
            comboFadeDoneLeft = false;
        }
        
        // Left player
        if (rightPlayerScript.lastHitStun <= 0){
            leftPlayerCombo = 0;
        }
        else if (rightPlayerScript.lastHitStun == rightPlayerScript.lastHitTaken - 1){
            leftPlayerCombo++;
        }
        
        if (rightPlayerCombo <= 1){
            if (!comboFadeDoneRight){
                StartCoroutine(FadeCombo(uiTexts[3], rightPlayerMaxCombo));
                comboFadeDoneRight = true;
            }
        }
        else{
            uiTexts[3].gameObject.SetActive(true);
            DOTween.To(() => uiTexts[3].color, x => uiTexts[3].color = x, rightPlayerScript.mainColor, 0.5f);
            uiTexts[3].text = rightPlayerCombo + " HITS!";
            rightPlayerMaxCombo = rightPlayerCombo;
            comboFadeDoneRight = false;
        }
    }

    IEnumerator FadeCombo(Text text, int maxCombo){
        text.text = maxCombo + " HITS!";
        
        yield return new WaitForSeconds(1f);
        
        DOTween.To(() => text.color, x => text.color = x, Color.clear, 0.5f).OnComplete(() => {
            text.gameObject.SetActive(false);
        });
    }

    private bool winningChance = true;
    private bool winningChanceGame = true;
    
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
            
            // Rounds condition
            if ((leftPlayerScript.health <= 0 && rightPlayerScript.roundsWon == 0) || (timeLeft <= 0 && leftPlayerScript.health < rightPlayerScript.health && rightPlayerScript.roundsWon == 0)){
                PlayerVictoryRound(leftPlayerScript, rightPlayerScript);
            }
            else if ((rightPlayerScript.health <= 0 && leftPlayerScript.roundsWon == 0) || (timeLeft <= 0 && rightPlayerScript.health < leftPlayerScript.health && leftPlayerScript.roundsWon == 0)){
                PlayerVictoryRound(rightPlayerScript, leftPlayerScript);
            }
            else if (timeLeft <= 0 && rightPlayerScript.health == leftPlayerScript.health && rightPlayerScript.roundsWon == 0 && leftPlayerScript.roundsWon == 0){
                var chance = Random.Range(0, 2);
                if (chance == 0 && winningChance){
                    PlayerVictoryRound(leftPlayerScript, rightPlayerScript);
                    winningChance = false;
                }
                else if (chance == 1 && winningChance){
                    PlayerVictoryRound(rightPlayerScript, leftPlayerScript);
                    winningChance = false;
                }
            }
            
            // Win condition
            else if (leftPlayerScript.health <= 0 && rightPlayerScript.roundsWon >= 1 || (timeLeft <= 0 && leftPlayerScript.health < rightPlayerScript.health && rightPlayerScript.roundsWon >= 1)){
                PlayerVictory(leftPlayerScript, rightPlayerScript);
            }
            else if (rightPlayerScript.health <= 0 && leftPlayerScript.roundsWon >= 1 || (timeLeft <= 0 && rightPlayerScript.health < leftPlayerScript.health && leftPlayerScript.roundsWon >= 1)){
                PlayerVictory(rightPlayerScript, leftPlayerScript);
            }
            else if (timeLeft <= 0 && rightPlayerScript.health == leftPlayerScript.health){
                if (rightPlayerScript.roundsWon > leftPlayerScript.roundsWon){
                    PlayerVictory(leftPlayerScript, rightPlayerScript);
                }
                else if (rightPlayerScript.roundsWon < leftPlayerScript.roundsWon){
                    PlayerVictory(rightPlayerScript, leftPlayerScript);
                }
                else{
                    rightPlayerScript.health = 1;
                    leftPlayerScript.health = 1;
                    uiTexts[10].color = Color.red;
                }
            }
        
    }

    private void PlayerBar(){
        // Left player
        if (leftPlayerScript.specialBar > leftPlayerScript.maxBar){
            leftPlayerScript.specialBar = 900;
        }

        if (leftPlayerScript.specialBar <= 300){
            uiImages[7].rectTransform.localScale = new Vector3(leftPlayerScript.specialBar / (leftPlayerScript.maxBar / 3), 1, 1);
            uiImages[7].color = new Color(0, 255, 0, 0.5f);
            
            uiImages[8].rectTransform.localScale = new Vector3(0, 1, 1);
            uiImages[8].color = new Color(0, 255, 0, 0.5f);
            
            uiImages[9].rectTransform.localScale = new Vector3(0, 1, 1);
            uiImages[9].color = new Color(0, 255, 0, 0.5f);
        } else if (leftPlayerScript.specialBar <= 600){
            uiImages[7].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[7].color = Color.green;
            
            uiImages[8].rectTransform.localScale = new Vector3((leftPlayerScript.specialBar - 300) / (leftPlayerScript.maxBar / 3), 1, 1);
            uiImages[8].color = new Color(0, 255, 0, 0.5f);
            
            uiImages[9].rectTransform.localScale = new Vector3(0, 1, 1);
            uiImages[9].color = new Color(0, 255, 0, 0.5f);
        } else if (leftPlayerScript.specialBar < 900){
            uiImages[7].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[7].color = Color.green;
            
            uiImages[8].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[8].color = Color.green;
            
            uiImages[9].rectTransform.localScale = new Vector3((leftPlayerScript.specialBar - 600) / (leftPlayerScript.maxBar / 3), 1, 1);
            uiImages[9].color = new Color(0, 255, 0, 0.5f);
        }
        else if(leftPlayerScript.specialBar == 900){
            uiImages[7].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[7].color = Color.green;
            
            uiImages[8].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[8].color = Color.green;
            
            uiImages[9].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[9].color = Color.green;
        }


        // Right player
        if (rightPlayerScript.specialBar > rightPlayerScript.maxBar){
            rightPlayerScript.specialBar = 900;
        }
        
        if (rightPlayerScript.specialBar <= 300){
            uiImages[13].rectTransform.localScale = new Vector3(rightPlayerScript.specialBar / (rightPlayerScript.maxBar / 3), 1, 1);
            uiImages[13].color = new Color(0, 255, 0, 0.5f);
            
            uiImages[12].rectTransform.localScale = new Vector3(0, 1, 1);
            uiImages[12].color = new Color(0, 255, 0, 0.5f);
            
            uiImages[11].rectTransform.localScale = new Vector3(0, 1, 1);
            uiImages[11].color = new Color(0, 255, 0, 0.5f);
        } else if (rightPlayerScript.specialBar <= 600){
            uiImages[13].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[13].color = Color.green;
            
            uiImages[12].rectTransform.localScale = new Vector3((rightPlayerScript.specialBar - 300) / (rightPlayerScript.maxBar / 3), 1, 1);
            uiImages[12].color = new Color(0, 255, 0, 0.5f);
            
            uiImages[11].rectTransform.localScale = new Vector3(0, 1, 1);
            uiImages[11].color = new Color(0, 255, 0, 0.5f);
        } else if (rightPlayerScript.specialBar < 900){
            uiImages[13].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[13].color = Color.green;
            
            uiImages[12].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[12].color = Color.green;
            
            uiImages[11].rectTransform.localScale = new Vector3((rightPlayerScript.specialBar - 600) / (rightPlayerScript.maxBar / 3), 1, 1);
            uiImages[11].color = new Color(0, 255, 0, 0.5f);
        }
        else if(rightPlayerScript.specialBar == 900){
            uiImages[13].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[13].color = Color.green;
            
            uiImages[12].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[12].color = Color.green;
            
            uiImages[11].rectTransform.localScale = new Vector3(1, 1, 1);
            uiImages[11].color = Color.green;
        }
    }

    private bool roundCount = true;

    private void RoundUp(playerPlataformerController winner){
        if (roundCount){
            winner.roundsWon++;
            roundCount = false;
        }
    }
    
    void PlayerVictory(playerPlataformerController losingPlayerScript, playerPlataformerController winningPlayerScript){
        // Fecha os comandos
        gameRunning = false;
        
        // Faz o efeito final e congela a animação
        if (!GetComponent<PostProcessing>().doneEffect){
            GetComponent<PostProcessing>().DeathVignette(true);
        }
        else{
            GetComponent<PostProcessing>().DeathVignette(false);
        }

        // Faz o efeito final da camera
        if (!doneCameraEffect){
            GameObject.FindWithTag("MainCamera").transform.DOMove(new Vector3(losingPlayerScript.posX , ((leftPlayerScript.posY + rightPlayerScript.posY) / 2) + 2, GameObject.FindWithTag("MainCamera").transform.position.z),  GetComponent<PostProcessing>().startSpeed).OnComplete(() => { 
                doneCameraEffect = true;
                leftPlayerScript.animator.speed = 0;
                rightPlayerScript.animator.speed = 0;
                StartCoroutine(HitStopStart(120f, 0.05f));
            });
            if (losingPlayerScript.isLeft){
                GameObject.FindWithTag("MainCamera").transform.DORotate(new Vector3(0, 0, 5),  GetComponent<PostProcessing>().startSpeed);
            }
            else{
                GameObject.FindWithTag("MainCamera").transform.DORotate(new Vector3(0, 0, -5),  GetComponent<PostProcessing>().startSpeed);
            }
        }
        else{
            GameObject.FindWithTag("MainCamera").transform.DOMove(new Vector3((leftPlayerScript.posX + rightPlayerScript.posX) / 2, 0f, sceneCamera.transform.position.z),  1f).SetDelay(0.1f);
            GameObject.FindWithTag("MainCamera").transform.DORotate(new Vector3(0, 0, 0), 1f).OnPlay(() => {
                if (winningPlayerScript.roundsWon == 1){
                    RoundUp(winningPlayerScript);
                }

                if (leftPlayerScript.roundsWon == 2){
                    uiTexts[7].gameObject.SetActive(true);
                }
                if (rightPlayerScript.roundsWon == 2){
                    uiTexts[9].gameObject.SetActive(true);
                }
                
                if (winningPlayerScript.grounded && !winningPlayerScript.currentlyAttacking){
                    StopPlayer(winningPlayerScript);
                    winningPlayerScript.won = true;
                }

                if (losingPlayerScript.grounded && !losingPlayerScript.currentlyAttacking){
                    StopPlayer(losingPlayerScript);
                    losingPlayerScript.lost = true;
                }
                
            }).SetDelay(0.1f);
        }

        // Da o hitstop longo
        uiTexts[4].enabled = true;
        uiTexts[4].text = winningPlayerScript.characterName + " wins";
        
        // Termina o jogo
        StartCoroutine(EndCondition());
    }

    private bool coroutineRound = true;
    
    void PlayerVictoryRound(playerPlataformerController losingPlayerScript, playerPlataformerController winningPlayerScript){
        // Fecha os comandos
        gameRunning = false;

        if (winningPlayerScript.grounded && !winningPlayerScript.currentlyAttacking){
            StopPlayer(winningPlayerScript);
            winningPlayerScript.won = true;
        }

        if (losingPlayerScript.grounded && !losingPlayerScript.currentlyAttacking){
            StopPlayer(losingPlayerScript);
            losingPlayerScript.lost = true;
        }

        if (coroutineRound){
            StartCoroutine(RestartRound(winningPlayerScript));
            coroutineRound = false;
        }
    }

    IEnumerator RestartRound(playerPlataformerController winner){
        
        yield return new WaitForSeconds(1);
        
        FadeOut();
        
        yield return new WaitForSeconds(1);

        winningChance = true;
        
        timeLeft = totalTime;
        uiTexts[10].text = timeLeft.ToString("0");
        
        leftPlayerScript.won = false;
        leftPlayerScript.lost = false;

        rightPlayerScript.won = false;
        rightPlayerScript.lost = false;

        rounds++;

        leftPlayerScript.health = leftPlayerScript.maxHealth;
        rightPlayerScript.health = rightPlayerScript.maxHealth;
        
        leftPlayerScript.MoveTo(spawnPoints[0].transform.position);
        rightPlayerScript.MoveTo(spawnPoints[1].transform.position);
        
        rightPlayerScript.isLeft = false;
        leftPlayerScript.isLeft = true;

        rightPlayerScript.flipCharacterLeft();
        leftPlayerScript.flipCharacterRight();

        // Faz a camera ficar entre os dois personages
        sceneCamera.transform.position = new Vector3((leftPlayer.transform.position.x + rightPlayer.transform.position.x) / 2, sceneCamera.transform.position.y, sceneCamera.transform.position.z);

        // Pega a difereça dos dois players
        float diffPos = Math.Abs(leftPlayerScript.posX - rightPlayerScript.posX);

        // Se a difereça for maior que 30 a camera muda de tamanho
        if (diffPos >= 30){
            sceneCamera.GetComponent<Camera>().orthographicSize = diffPos / 3f;
        }
        else{
            sceneCamera.GetComponent<Camera>().orthographicSize = 10;
        }
        
        yield return new WaitForSeconds(1);
        
        RoundUp(winner);
        if (leftPlayerScript.roundsWon == 1){
            uiTexts[6].gameObject.SetActive(true);
        }
        
        if (rightPlayerScript.roundsWon == 1){
            uiTexts[8].gameObject.SetActive(true);
        }

        FadeIn();
        
        yield return new WaitForSeconds(1);
        
        uiTexts[4].enabled = true;
        uiTexts[4].text = "Round " + rounds;
        
        yield return new WaitForSeconds(1);
        
        uiTexts[4].enabled = false;
        
        yield return new WaitForSeconds(0.5f);
        
        coroutineRound = true;
        roundCount = true;
        uiTexts[4].enabled = true;
        gameRunning = true;
        uiTexts[4].text = "FIGHT!";
        
        yield return new WaitForSeconds(0.5f);
        
        uiTexts[4].enabled = false;
    }

    private void FadeIn(){
        transform.GetChild(2).GetComponent<SpriteRenderer>().DOColor(Color.clear, 1).OnComplete(() => {
            transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.clear;
        }).OnPlay(() => {
            transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.black;
        });
    }

    private void FadeOut(){
        transform.GetChild(2).GetComponent<SpriteRenderer>().DOColor(Color.black, 1).OnPlay(() => {
            transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.clear;
        }).OnComplete(() => {
            transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.black;
        });
    }

    void CameraControl(){
        if (gameRunning){
            // Faz a camera ficar entre os dois personages
            sceneCamera.transform.position = new Vector3((leftPlayerScript.posX + rightPlayerScript.posX) / 2, sceneCamera.transform.position.y, sceneCamera.transform.position.z);

            // Pega a difereça dos dois players
            float diffPos = Math.Abs(leftPlayerScript.posX - rightPlayerScript.posX);

            // Se a difereça for maior que 30 a camera muda de tamanho
            if (diffPos >= 30){
                sceneCamera.GetComponent<Camera>().orthographicSize = diffPos / 3f;
            }
            else{
                sceneCamera.GetComponent<Camera>().orthographicSize = 10;
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

    private void MapaBGO(){
        if (!currentlyBGO){
            var chanceGeral = Random.Range(0, 501);

            if (chanceGeral == 1){

                var chanceLado = Random.Range(0, 2);

                if (chanceLado == 0){
                    arenas[0].GetComponent<ArenaScript>().SpawnBGO(spawnPoints[4].transform.position);
                    currentlyBGO = true;
                }
                else{
                    arenas[0].GetComponent<ArenaScript>().SpawnBGO(spawnPoints[5].transform.position);
                    currentlyBGO = true;
                }
            }
        }
    }

    private bool velocityCheck;
    
    public void TDEEffect(playerPlataformerController player){
        // Start
        gameRunning = false;
        timeFreeze = true;
        
        leftPlayerScript.animator.speed = 0;
        rightPlayerScript.animator.speed = 0;

        var particle1 = Instantiate(particles[4], new Vector3(player.transform.position.x, player.transform.position.y + 2.5f), Quaternion.identity);
        var particle2 = Instantiate(particles[5], new Vector3(player.transform.position.x, player.transform.position.y + 2.5f), Quaternion.identity);

        ParticleSystem.MainModule color1 = particle1.main;
        ParticleSystem.MainModule color2 = particle2.main;

        color1.startColor = player.mainColor;
        color2.startColor = player.mainColor;
        
        Vector2 leftTargetV = Vector2.zero;
        Vector2 leftV = Vector2.zero;
        
        Vector2 rightTargetV = Vector2.zero;
        Vector2 rightV = Vector2.zero;

        if (leftPlayerScript.lastHitStun > 0){
            leftPlayerScript.hitStunPause = true;
            leftPlayerScript.targetVelocity = Vector2.zero;
            leftPlayerScript.velocity = Vector2.zero;
        }

        if (rightPlayerScript.lastHitStun > 0){
            rightPlayerScript.hitStunPause = true;
            rightPlayerScript.targetVelocity = Vector2.zero;
            rightPlayerScript.velocity = Vector2.zero;
        }

        if (leftPlayerScript.jumping && rightPlayerScript.jumping){
            if (!velocityCheck){
                leftTargetV = leftPlayerScript.targetVelocity;
                leftV = leftPlayerScript.velocity;

                rightTargetV = rightPlayerScript.targetVelocity;
                rightV = rightPlayerScript.velocity;

                leftPlayerScript.targetVelocity = Vector2.zero;
                leftPlayerScript.velocity = Vector2.zero;
                rightPlayerScript.targetVelocity = Vector2.zero;
                rightPlayerScript.velocity = Vector2.zero;

                velocityCheck = true;
            }

            leftPlayerScript.graityModified = 0f;
            rightPlayerScript.graityModified = 0f;
        } else if (leftPlayerScript.jumping){
            if (!velocityCheck){
                leftTargetV = leftPlayerScript.targetVelocity;
                leftV = leftPlayerScript.velocity;

                leftPlayerScript.targetVelocity = Vector2.zero;
                leftPlayerScript.velocity = Vector2.zero;

                velocityCheck = true;
            }

            leftPlayerScript.graityModified = 0f;
        } else if (rightPlayerScript.jumping){
            if (!velocityCheck){
                rightTargetV = rightPlayerScript.targetVelocity;
                rightV = rightPlayerScript.velocity;

                rightPlayerScript.targetVelocity = Vector2.zero;
                rightPlayerScript.velocity = Vector2.zero;

                velocityCheck = true;
            }

            rightPlayerScript.graityModified = 0f;
        }

        sceneCamera.transform.DOMove(new Vector3(player.transform.position.x, player.transform.position.y + 5, sceneCamera.transform.position.z), 0.5f);
        
        transform.GetChild(3).GetComponent<SpriteRenderer>().DOColor(Color.black, 0.25f).OnPlay(() => {
            transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.clear;
        }).OnComplete(() => {
            transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.black;
        });
        
        
        // Finish
        sceneCamera.transform.DOMove(new Vector3((leftPlayerScript.posX + rightPlayerScript.posX) / 2, sceneCamera.transform.position.y, sceneCamera.transform.position.z), 0.5f).SetDelay(1f).OnComplete(() => {
            gameRunning = true;
            timeFreeze = false;
            
            leftPlayerScript.animator.speed = 1;
            rightPlayerScript.animator.speed = 1;

            if (leftPlayerScript.lastHitStun > 0){
                leftPlayerScript.hitStunPause = false;
            }

            if (rightPlayerScript.lastHitStun > 0){
                rightPlayerScript.hitStunPause = false;
            }
            
            if (leftPlayerScript.jumping && rightPlayerScript.jumping){
                leftPlayerScript.targetVelocity = leftTargetV;
                leftPlayerScript.velocity = leftV;

                rightPlayerScript.targetVelocity = rightTargetV;
                rightPlayerScript.velocity = rightV;

                leftPlayerScript.graityModified = 10f;
                rightPlayerScript.graityModified = 10f;
            } else if (leftPlayerScript.jumping){
                leftPlayerScript.targetVelocity = leftTargetV;
                leftPlayerScript.velocity = leftV;

                leftPlayerScript.graityModified = 10f;
            } else if (rightPlayerScript.jumping){
                rightPlayerScript.targetVelocity = rightTargetV;
                rightPlayerScript.velocity = rightV;

                rightPlayerScript.graityModified = 10f;
            }

            velocityCheck = false;
        });
        
        transform.GetChild(3).GetComponent<SpriteRenderer>().DOColor(Color.clear, 0.25f).OnComplete(() => {
            transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.clear;
        }).OnPlay(() => {
            transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.black;
        }).SetDelay(1f);

    }

}
