using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPlataformerController : PhysicsObject{
    public float jumpTakeOffSpeed = 7;
    public float speed = 7;
    public float dashFrameTotal;
    public float dashSpeed;

    protected float backTimer = 100;
    protected float frontTimer = 100;
    protected float downTimer = 100;
    protected bool ableToMove;

    private bool backTimerBool;
    private bool frontTimerBool;
    private bool downTimerBool;
    private bool canKeyCheck;
    private int dashForward;
    private int dashBackward;
    private float dashFrames;
    private bool currentlyDashing;

    protected override void ComputeVelocity(){
        // Pega as inputs do controle
        float moveHRaw = Input.GetAxisRaw("Horizontal");
        float moveVRaw = Input.GetAxisRaw("Vertical");
        float moveH = 0;
        float moveV = 0;

        // Transforma analogico em binário (0 ou 1)
        if (moveHRaw > 0){
            moveH = 1;
        }
        else if (moveHRaw < 0){
            moveH = -1;
        }

        if (moveVRaw > 0){
            moveV = 1;
        }
        else if (moveVRaw < 0){
            moveV = -1;
        }

        // Reseta o movimento
        Vector2 move = Vector2.zero;

        // Move.x vira 0 ou 1
        move.x = moveH;

        // Checagem de teclas para especiais
        KeyCheck(moveHRaw, moveVRaw);
        if (backTimerBool){
            backTimer++;
        }

        if (frontTimerBool){
            frontTimer++;
        }

        if (downTimerBool){
            downTimer++;
        }

        // Dashes
        if (dashBackward >= 1 && grounded && !currentlyDashing){
            dashFrames = dashFrameTotal;
            targetVelocity = new Vector2(1, 0) * -dashSpeed;
            dashBackward = 0;
            print("Dash back");
        }

        if (dashForward >= 1 && grounded && !currentlyDashing){
            dashFrames = dashFrameTotal;
            targetVelocity = new Vector2(1, 0) * dashSpeed;
            dashForward = 0;
            print("Dash forward");
        }

        // Checa os pulos
        if (moveV > 0.4f && moveH > 0.4f && grounded){
            velocity.y = jumpTakeOffSpeed;
        }
        else if (moveV > 0.4f && moveH < -0.4f && grounded){
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetAxis("Vertical") > 0.5f && grounded){
            velocity.y = jumpTakeOffSpeed;
        }

        // Ações que previnem a movimentação livre
        if (dashFrames > 0){
            dashFrames--;
            ableToMove = false;
            currentlyDashing = true;
        }
        else{
            ableToMove = true;
            currentlyDashing = false;
        }

        // Faz o personagem andar
        if (grounded && ableToMove){
            targetVelocity = move * speed;
        }
    }

    void KeyCheck(float moveH, float moveV){
        // Checa se o jogador deixou o analogico no neutro antes de repetir o movimento (aplica-se apenas para dash)
        if (moveH > -0.5f && moveH < 0.5f && moveV > -0.5f && moveV < 0.5f){
            canKeyCheck = true;
        }

        // Dash window
        float dashWindow = 15;

        // Checa o analogico para trás
        if (moveH == -1 && canKeyCheck){
            if (backTimer < dashWindow && grounded){
                dashBackward += 1;
            }
            else{
                dashBackward = 0;
            }

            backTimer = 0;
            backTimerBool = true;
            canKeyCheck = false;
        }

        // Checa o analogico para frente
        if (moveH == 1 && canKeyCheck){
            if (frontTimer < dashWindow && grounded){
                dashForward += 1;
            }
            else{
                dashForward = 0;
            }

            frontTimer = 0;
            frontTimerBool = true;
            canKeyCheck = false;
        }

        // Checa o analogico para baixo
        if (moveV == -1 && canKeyCheck){
            downTimer = 0;
            downTimerBool = true;
            canKeyCheck = false;
        }
    }
}