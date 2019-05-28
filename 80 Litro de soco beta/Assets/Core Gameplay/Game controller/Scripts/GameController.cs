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
    private int rightPlayerCombo;
    private bool isPaused;

    private bool gameRunning;
    private int rounds = 1;

    protected GameObject[] players;
    private bool flipToggle = true;
    private GameObject sceneCamera;

    public GameObject[] arenas;
    public bool currentlyBGO;
    public Text[] uiTexts;
    public Image[] uiImages;
    public GameObject[] spawnPoints;

    public ParticleSystem[] particles;

    private bool hitStopCurrently;
    
    private bool doneCameraEffect;
    private float alpha;
    public bool introDone;

    private float totalTime = 60;
    private float timeLeft;

    void Start(){
        // Coloca o mapa
        Instantiate(arenas[0]);
        
        arenas[0].GetComponent<ArenaScript>().SpawnInteractable(spawnPoints[2].transform.position);
        arenas[0].GetComponent<ArenaScript>().SpawnInteractable(spawnPoints[3].transform.position);

        timeLeft = totalTime;
        uiTexts[10].text = timeLeft.ToString();
        
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
        
        uiTexts[1].color = rightPlayerScript.mainColor;
        uiTexts[3].color = rightPlayerScript.mainColor;
        uiImages[3].color = rightPlayerScript.mainColor;
        
        transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.black;

        // Coloca o sangue nos prefabs
        AttachBlood();

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
        
        if (!hitStopCurrently && !isPaused){
            Time.timeScale = 1;
            leftPlayerScript.animator.speed = 1;
            rightPlayerScript.animator.speed = 1;
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
        leftPlayerScript.animator.speed = 1;
        rightPlayerScript.animator.speed = 1;
    }

    void ComboCounter(){
        // Right player
        if (leftPlayerScript.lastHitStun <= 0){
            rightPlayerCombo = 0;
        }
        else if (leftPlayerScript.lastHitStun == leftPlayerScript.lastHitTaken - 1){
            rightPlayerCombo++;
        }

        if (leftPlayerCombo <= 1){
            uiTexts[2].gameObject.SetActive(false);
        }
        else{
            uiTexts[2].gameObject.SetActive(true);
            uiTexts[2].text = leftPlayerCombo + " HITS!";
        }
        
        // Left player
        if (rightPlayerScript.lastHitStun <= 0){
            leftPlayerCombo = 0;
        }
        else if (rightPlayerScript.lastHitStun == rightPlayerScript.lastHitTaken - 1){
            leftPlayerCombo++;
        }
        
        if (rightPlayerCombo <= 1){
            uiTexts[3].gameObject.SetActive(false);
        }
        else{
            uiTexts[3].gameObject.SetActive(true);
            uiTexts[3].text = rightPlayerCombo + " HITS!";
        }
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
        sceneCamera.transform.position = new Vector3(0, sceneCamera.transform.position.y, sceneCamera.transform.position.z);

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

}
