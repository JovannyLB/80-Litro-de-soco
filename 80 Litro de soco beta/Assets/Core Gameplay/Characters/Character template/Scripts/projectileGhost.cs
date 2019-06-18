using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileGhost : MonoBehaviour{

    private float ghostDelay = 0.05f;
    private float ghostDelaySeconds;
    [Header("Prefab chamado 'GhostEffect'")]
    public GameObject ghostPreFab;
    [HideInInspector]public bool makeGhost;
    private Coroutine countDown;
    
    void Start(){
        ghostDelaySeconds = ghostDelay;
    }

    
    void Update()
    {
        if (makeGhost){
            if (ghostDelaySeconds > 0){
                ghostDelaySeconds -= Time.deltaTime;
            }
            else{
                // Instancia o fantasma
                GameObject currentGhost = Instantiate(ghostPreFab, transform.position, Quaternion.identity);
                // Pega o sprite renderer do mesmo
                SpriteRenderer currentSprite = currentGhost.GetComponent<SpriteRenderer>();
                // Iguala a sprite dele para a sprite do personagem
                currentSprite.sprite = GetComponent<SpriteRenderer>().sprite;
                // Iguala a cor dele para a cor principal do personagem
                currentSprite.color = GetComponent<projectileLifeCheck>().ownPlayerScript.mainColor;
                // Vira ele para o lado certo
                currentSprite.transform.localScale = transform.localScale;
                ghostDelaySeconds = ghostDelay;
            }
        }
    }

    public void StartGhost(){
        makeGhost = true;
    }

    public void stopGhost(){
        makeGhost = false;
    }

}