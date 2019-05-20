using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;

public class ghostFadeOut : MonoBehaviour
{
    
    private float fadeOut = 0.9f;
    public float fadeOutSpeed;
    
    void Update(){
        // Faz o fadeout do ghost
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadeOut);
        fadeOut -= fadeOutSpeed;

        // Destroi o mesmo se a opacidade for invisivel
        if (spriteRenderer.color.a <= 0){
            Destroy(gameObject);
        }
        
    }
}
