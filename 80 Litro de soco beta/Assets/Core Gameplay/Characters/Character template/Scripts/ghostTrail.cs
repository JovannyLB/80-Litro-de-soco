using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostTrail : MonoBehaviour{

    private float ghostDelay = 0.05f;
    private float ghostDelaySeconds;
    [Header("Prefab chamado 'GhostEffect'")]
    public GameObject ghostPreFab;
    [HideInInspector]public bool makeGhost;
    
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
                currentSprite.color = GetComponent<playerPlataformerController>().mainColor;
                // Vira ele para o lado certo
                currentSprite.transform.localScale = transform.localScale;
                ghostDelaySeconds = ghostDelay;
            }
        }
    }

    void startGhost(){
        makeGhost = true;
        StartCoroutine(safePoint());
    }

    void stopGhost(){
        makeGhost = false;
        StopCoroutine(safePoint());
    }

    IEnumerator safePoint(){
        yield return new WaitForSeconds(1);
        makeGhost = false;
    }

}
