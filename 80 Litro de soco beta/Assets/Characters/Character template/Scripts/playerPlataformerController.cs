using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPlataformerController : PhysicsObject{
    
    // Informações gerais
        // Informações sobre movimetação
    public float jumpTakeOffSpeed = 7;
    public float speed = 7;
        // Informações sobre os personagens
    public string CharName;
    public float health;

    // Atributos de dash
    protected float backTimer = 100;
    protected float frontTimer = 100;
    protected float downTimer = 100;
    protected bool ableToMove;
    
    public float dashFrameTotal;
    public float dashSpeed;
    public float dashCooldownTotal;

    private bool backTimerBool;
    private bool frontTimerBool;
    private bool downTimerBool;
    private bool canKeyCheck;
    private int dashForward;
    private int dashBackward;
    private float dashFrames;
    private bool currentlyDashing;
    private float dashCooldown;
    private bool canDash;
    
    // Cotroles
    protected bool xButton;
    protected bool square;
    protected bool circle;
    protected bool triangle;
    protected float moveHRaw;
    protected float moveVRaw;
    public bool player1;
    
    // Animações e botões
        // Animator
    protected Animator animator;
        // Booleans
    protected bool crouching;
    protected bool currentlyAttacking;
    protected bool standLightPunchCurrently;
    protected bool standLightKickCurrently;
    protected bool crouchLightPunchCurrently;
    protected bool crouchLightKickCurrently;

    protected override void Controls(){
        // Analógico
        if (player1){
            moveHRaw = Input.GetAxisRaw("Horizontal");
            moveVRaw = Input.GetAxisRaw("Vertical");
        }
        else{
            moveHRaw = Input.GetAxisRaw("Horizontal2");
            moveVRaw = Input.GetAxisRaw("Vertical2");
        }

        // Botões
        if (player1){
            xButton = Input.GetKeyDown(KeyCode.JoystickButton1);
            square = Input.GetKeyDown(KeyCode.JoystickButton0);
            circle = Input.GetKeyDown(KeyCode.JoystickButton2);
            triangle = Input.GetKeyDown(KeyCode.JoystickButton3);
        }
        else{
            xButton = Input.GetKeyDown(KeyCode.Joystick1Button1);
            square = Input.GetKeyDown(KeyCode.Joystick1Button0);
            circle = Input.GetKeyDown(KeyCode.Joystick1Button2);
            triangle = Input.GetKeyDown(KeyCode.Joystick1Button3);
        }
    }

    protected override void ComputeVelocity(){
        // Pega as inputs do controle
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

        // Cooldown
        if (dashCooldown > 0){
            canDash = false;
            dashCooldown--;
        }
        else{
            canDash = true;
        }
        
        // Dashes
        if (dashBackward >= 1 && grounded && !currentlyDashing && !crouching && !currentlyAttacking && canDash){
            dashFrames = dashFrameTotal;
            targetVelocity = new Vector2(1, 0) * -dashSpeed;
            dashBackward = 0;
            dashCooldown = dashCooldownTotal;
        }

        if (dashForward >= 1 && grounded && !currentlyDashing && !crouching && !currentlyAttacking && canDash){
            dashFrames = dashFrameTotal;
            targetVelocity = new Vector2(1, 0) * dashSpeed;
            dashForward = 0;
            dashCooldown = dashCooldownTotal;
        }

        // Checa os pulos
        if (moveV > 0.4f && moveH > 0.4f && grounded && ableToMove){
            velocity.y = jumpTakeOffSpeed;
        }
        else if (moveV > 0.4f && moveH < -0.4f && grounded && ableToMove){
            velocity.y = jumpTakeOffSpeed;
        }
        else if (moveV > 0.5f && grounded && ableToMove){
            velocity.y = jumpTakeOffSpeed;
        }

        // Ações que previnem a movimentação livre
        if (dashFrames > 0){
            dashFrames--;
            ableToMove = false;
            currentlyDashing = true;
        }
        else{
            currentlyDashing = false;
        }

        // Agachar
        if (moveVRaw < -0.6f && grounded && !currentlyDashing){
            crouching = true;
            ableToMove = false;
            targetVelocity = Vector2.zero;
        }
        else{
            crouching = false;
        }
        
        // Faz o personagem andar
        if (grounded && ableToMove){
            targetVelocity = move * speed;
        }

        if (grounded && !crouching && !currentlyDashing && !currentlyAttacking){
            ableToMove = true;
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

    // Animação
    protected override void animationStart(){
        animator = GetComponent<Animator>();
    }

    protected override void animationUpdate(){
        // Moimento
        if (grounded){
            animator.SetFloat("movementSpeed", Math.Abs(targetVelocity.x));
        }
        else{
            animator.SetFloat("movementSpeed", 0);
        }

        animator.SetBool("crouching", crouching);
        
        // Stand light punch
        animator.SetBool("standLightPunch", standLightPunchCurrently);
        // Stand light kick
        animator.SetBool("standLightKick", standLightKickCurrently);
        // Crouch light punch
        animator.SetBool("crouchLightPunch", crouchLightPunchCurrently);
        // Crouch light kick
        animator.SetBool("crouchLightKick", crouchLightKickCurrently);
    }
    
    // Core gameplay
    protected override void CoreGameplayUpdate(){
        // Light punches
        if (square && grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            StandLightPunch();
        }
        else if (square && grounded && crouching && !currentlyAttacking && !currentlyDashing){
            CrouchLightPunch();
        }
        
        // Light kick's
        if (xButton && grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            StandLightKick();
        } 
        else if (xButton && grounded && crouching && !currentlyAttacking && !currentlyDashing){
            CrouchLightKick();
        }
    }

    protected void CurrentlyAttacking(bool attacking){
        targetVelocity = Vector2.zero;
        currentlyAttacking = attacking;
        ableToMove = !attacking;
    }

    // Stand light punch
    protected void StandLightPunch(){
        CurrentlyAttacking(true);
        standLightPunchCurrently = true;
    }

    public void StandLightPunchStop(){
        CurrentlyAttacking(false);
        standLightPunchCurrently = false;
    }
    
    // Stand light kick
    protected void StandLightKick(){
        CurrentlyAttacking(true);
        standLightKickCurrently = true;
    }

    public void StandLightKickStop(){
        CurrentlyAttacking(false);
        standLightKickCurrently = false;
    }
    
    // Crouch light punch
    protected void CrouchLightPunch(){
        CurrentlyAttacking(true);
        crouchLightPunchCurrently = true;
    }

    public void CrouchLightPunchStop(){
        CurrentlyAttacking(false);
        crouchLightPunchCurrently = false;
    }
    
    // Crouch light kick
    protected void CrouchLightKick(){
        CurrentlyAttacking(true);
        crouchLightKickCurrently = true;
    }

    public void CrouchLightKickStop(){
        CurrentlyAttacking(false);
        crouchLightKickCurrently = false;
    }
}