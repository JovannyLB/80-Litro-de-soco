using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPlataformerController : PhysicsObject{

    public float jumpTakeOffSpeed = 7;
    public float speed = 7;

    private bool isJumpingForward;

    protected override void ComputeVelocity(){
        float moveH = Input.GetAxisRaw("Horizontal");
        float moveV = Input.GetAxisRaw("Vertical");
        
        Vector2 move = Vector2.zero;

        move.x = moveH;

        if (moveV > 0.4f && moveH > 0.4f && grounded){
            isJumpingForward = true;
            velocity.y = jumpTakeOffSpeed;
            targetVelocity = move * speed;
            print("Pulo para frente");
        } else if (moveV > 0.4f && moveH < -0.4f && grounded){
            velocity.y = jumpTakeOffSpeed;
            print("Pulo para trás");
        } else if (Input.GetAxis("Vertical") > 0.5f && grounded){
            velocity.y = jumpTakeOffSpeed;
            print("Pulo neutro");
        }

        if (isJumpingForward){
            move.x = 1;
        }
        
        if (grounded){
            isJumpingForward = false;
            targetVelocity = move * speed;
        }

    }
}
