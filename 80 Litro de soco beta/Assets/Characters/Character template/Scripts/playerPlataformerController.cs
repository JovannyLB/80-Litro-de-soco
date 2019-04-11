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
    public bool jumping;

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
    protected bool standHardPunchCurrently;
    protected bool standLightKickCurrently;
    protected bool standHardKickCurrently;
    protected bool crouchLightPunchCurrently;
    protected bool crouchHardPunchCurrently;
    protected bool crouchLightKickCurrently;
    protected bool crouchHardKickCurrently;
    protected bool jumpingPunchCurrently;
    protected bool jumpingKickCurrently;
    public int lastHitStun;
    protected bool hitStunFreezeAnim;
    public bool beenHitTorso;
    public bool beenHitHead;
    public bool beenHitLeg;
    
    protected bool isHitting;
    protected bool hasHit;
    public Collider2D enemy;

    // Hurtboxes
    public Collider2D[] hurtbox;
    public GameObject hurtBoxes;
    public GameObject hitBoxes;

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
            xButton = Input.GetKeyDown(KeyCode.Joystick1Button1);
            square = Input.GetKeyDown(KeyCode.Joystick1Button0);
            circle = Input.GetKeyDown(KeyCode.Joystick1Button2);
            triangle = Input.GetKeyDown(KeyCode.Joystick1Button3);
        }
        else{
            xButton = Input.GetKeyDown(KeyCode.Joystick2Button1);
            square = Input.GetKeyDown(KeyCode.Joystick2Button0);
            circle = Input.GetKeyDown(KeyCode.Joystick2Button2);
            triangle = Input.GetKeyDown(KeyCode.Joystick2Button3);
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

        if (!grounded) jumping = true;
        else jumping = false;

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

        if (grounded && !crouching && !currentlyDashing && !currentlyAttacking & !beenHitTorso && !beenHitLeg && !beenHitHead){
            ableToMove = true;
        }

        hitBoxes.transform.position = transform.position;
        hurtBoxes.transform.position = transform.position;
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
        
        animator.SetBool("jumping", jumping);
        
        // Hit stun animation
        animator.SetInteger("lastHitStun", lastHitStun);
        animator.SetBool("beenHitTorso", beenHitTorso);
        animator.SetBool("beenHitHead", beenHitHead);
        animator.SetBool("beenHitLeg", beenHitLeg);

        // Stand light punch
        animator.SetBool("standLightPunch", standLightPunchCurrently);
        // Stand hard punch
        animator.SetBool("standHardPunch", standHardPunchCurrently);
        // Stand light kick
        animator.SetBool("standLightKick", standLightKickCurrently);
        // Stand hard kick
        animator.SetBool("standHardKick", standHardKickCurrently);
        // Crouch light punch
        animator.SetBool("crouchLightPunch", crouchLightPunchCurrently);
        // Crouch hard punch
        animator.SetBool("crouchHardPunch", crouchHardPunchCurrently);
        // Crouch light kick
        animator.SetBool("crouchLightKick", crouchLightKickCurrently);
        // Crouch hard kick
        animator.SetBool("crouchHardKick", crouchHardKickCurrently);
        // Jump punch
        animator.SetBool("jumpingPunch", jumpingPunchCurrently);
        // Jump kick
        animator.SetBool("jumpingKick", jumpingKickCurrently);
    }

    // Core gameplay
    protected override void CoreGameplayStart(){
    }

    protected override void CoreGameplayUpdate(){
        if (lastHitStun != 0 && hitStunFreezeAnim){
            lastHitStun--;
        }

        if (hasHit && enemy.CompareTag("Head")){
            print("hurt head lmao");
            hasHit = false;
        }
        else if (hasHit && enemy.CompareTag("Torso")){
            print("hurt torso lol");
            hasHit = false;
        }
        else if (hasHit && enemy.CompareTag("Legs")){
            print("hurt leggy :(");
            hasHit = false;
        }

        // Light punches
        if (square && grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            StandLightPunch();
        }
        else if (square && grounded && crouching && !currentlyAttacking && !currentlyDashing){
            CrouchLightPunch();
        }
        
        // Hard punches
        if (triangle && grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            StandHardPunch();
        }
        else if (triangle && grounded && crouching && !currentlyAttacking && !currentlyDashing){
            CrouchHardPunch();
        }

        // Light kick's
        if (xButton && grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            StandLightKick();
        }
        else if (xButton && grounded && crouching && !currentlyAttacking && !currentlyDashing){
            CrouchLightKick();
        }
        
        // Hard kick's
        if (circle && grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            StandHardKick();
        }
        else if (circle && grounded && crouching && !currentlyAttacking && !currentlyDashing){
            CrouchHardKick();
        }
        
        // Jump punch
        if (square && !grounded && !crouching && !currentlyAttacking && !currentlyDashing || triangle && !grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            JumpPunch();
        }
        
        // Jump kick
        if (xButton && !grounded && !crouching && !currentlyAttacking && !currentlyDashing || circle && !grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            JumpKick();
        }
    }

    protected void CurrentlyAttacking(bool attacking){
        targetVelocity = Vector2.zero;
        currentlyAttacking = attacking;
        ableToMove = !attacking;
    }

    protected void CurrentlyJumpAttacking(bool attacking){
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
    
    // Stand hard punch
    protected void StandHardPunch(){
        CurrentlyAttacking(true);
        standHardPunchCurrently = true;
    }

    public void StandHardPunchStop(){
        CurrentlyAttacking(false);
        standHardPunchCurrently = false;
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
    
    // Stand hard kick
    protected void StandHardKick(){
        CurrentlyAttacking(true);
        standHardKickCurrently = true;
    }

    public void StandHardKickStop(){
        CurrentlyAttacking(false);
        standHardKickCurrently = false;
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
    
    // Crouch hard punch
    protected void CrouchHardPunch(){
        CurrentlyAttacking(true);
        crouchHardPunchCurrently = true;
    }

    public void CrouchHardPunchStop(){
        CurrentlyAttacking(false);
        crouchHardPunchCurrently = false;
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
    
    // Crouch hard kick
    protected void CrouchHardKick(){
        CurrentlyAttacking(true);
        crouchHardKickCurrently = true;
    }

    public void CrouchHardKickStop(){
        CurrentlyAttacking(false);
        crouchHardKickCurrently = false;
    }
    
    // Jump punch
    protected void JumpPunch(){
        CurrentlyJumpAttacking(true);
        jumpingPunchCurrently = true;
    }

    public void JumpPunchStop(){
        CurrentlyJumpAttacking(false);
        jumpingPunchCurrently = false;
    }
    
    // Jump kick
    protected void JumpKick(){
        CurrentlyJumpAttacking(true);
        jumpingKickCurrently = true;
    }

    public void JumpKickStop(){
        CurrentlyJumpAttacking(false);
        jumpingKickCurrently = false;
    }

    // Hit stun animations
    public void addHitStun(int hitStun){
        lastHitStun = (hitStun - 10) + 1;
    }

    public void hitStunStart(){
        hitStunFreezeAnim = true;
    }

    public void hitStunStop(){
        hitStunFreezeAnim = false;
    }

    public void gotHitTorsoStart(){
        beenHitTorso = true;
    }

    public void gotHitTorsoEnd(){
        beenHitTorso = false;
    }
    
    public void gotHitHeadStart(){
        beenHitHead = true;
    }

    public void gotHitHeadEnd(){
        beenHitHead = false;
    }
    
    public void gotHitLegStart(){
        beenHitLeg = true;
    }

    public void gotHitLegEnd(){
        beenHitLeg = false;
    }

    public void StopAllAttack(){
        JumpPunchStop();
        JumpKickStop();
        StandLightPunchStop();
        StandHardPunchStop();
        StandLightKickStop();
        StandHardKickStop();
        CrouchLightPunchStop();
        CrouchHardPunchStop();
        CrouchLightKickStop();
        CrouchHardKickStop();
    }

    /*private void launchAttack(Collider2D col){
        Collider2D cols = Physics2D.OverlapBox(col.bounds.center, col.bounds.size, col.transform.rotation.z, LayerMask.GetMask("Hitbox"));
        if (cols != null){
            if (cols.transform.root.name != transform.root.name){
                enemy = cols;
                print(cols.transform.root.name);
                hasHit = true;
            }
        }
    }
    */
}