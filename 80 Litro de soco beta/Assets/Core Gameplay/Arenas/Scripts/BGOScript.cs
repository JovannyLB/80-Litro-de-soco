using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BGOScript : MonoBehaviour{

    private bool isLeft;
    
    void Start(){
        if (transform.position.x < 0){
            isLeft = true;
        }
        else{
            isLeft = false;
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void Update()
    {
        
        if (isLeft){
            GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
            if (transform.position.x > 100){
                GameObject.FindWithTag("GameController").GetComponent<GameController>().currentlyBGO = false;
                Destroy(gameObject);
            }
        }
        else{
            GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
            if (transform.position.x < -100){
                GameObject.FindWithTag("GameController").GetComponent<GameController>().currentlyBGO = false;
                Destroy(gameObject);
            }
        }

    }
}
