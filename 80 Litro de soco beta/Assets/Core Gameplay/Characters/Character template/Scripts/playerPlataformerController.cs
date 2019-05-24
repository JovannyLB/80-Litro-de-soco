using System;
using UnityEngine;

public class playerPlataformerController : PhysicsObject{
    // Informações gerais

    [Header("Basic character information")]

    // Informações sobre os personagens
    public string characterName;

    public float health;
    [HideInInspector]public float maxHealth;
    [HideInInspector]public bool won;
    [HideInInspector]public bool lost;
    public Color mainColor;

    // Informações sobre movimetação
    private float jumpTakeOffSpeed = 45;
    private float jumpTakeOffHorizontal = 13;
    [HideInInspector]public float walkingSpeed = 7;
    private float speedTotal;

    [HideInInspector] public bool enableControls = true;

    // Atributos de dash
    protected float backDashTimer = 100;
    protected float frontDashTimer = 100;
    private bool ableToMove;
    [HideInInspector]public float posX;

    private float dashFrameTotal = 10;
    private float dashSpeed = 20;
    private float dashCooldownTotal = 30;

    private bool backTimerDashBool;
    private bool frontTimerDashBool;
    private bool canKeyCheck;
    private int dashForward;
    private int dashBackward;
    private float dashFrames;
    private bool currentlyDashing;
    private float dashCooldown;
    private bool canDash;
    [HideInInspector] public bool jumping;

    [HideInInspector] public bool isBlockingHigh;
    [HideInInspector] public bool isBlockingLow;
    private bool currentlyBlockingGeneral;

    // Checagem de especiais
    [HideInInspector]public int upTimerSpecial;
    [HideInInspector]public int downTimerSpecial;
    [HideInInspector]public int rightTimerSpecial;
    [HideInInspector]public int leftTimerSpecial;
    [HideInInspector]public int xButtonTimerSpecial;
    [HideInInspector]public int squareTimerSpecial;
    [HideInInspector]public int triangleTimerSpecial;
    [HideInInspector]public int circleTimerSpecial;
    private ghostTrail ghost;

    // Cotroles
    [HideInInspector]public bool xButton;
    [HideInInspector]public bool square;
    [HideInInspector]public bool circle;
    [HideInInspector]public bool triangle;
    protected bool r2;
    protected float moveHRaw;
    protected float moveVRaw;

    [Header("Flips between JoyStick 1 and 2")]
    public bool isPlayer1;

    // Animator
    protected Animator animator;

    // Booleans
    [HideInInspector]public bool isFlippedSide;

    [HideInInspector] public bool isLeft;
    protected bool hasFlippedSide;
    [HideInInspector] public bool inCorner;
    [HideInInspector] public bool beingJumpedOver;
    [HideInInspector] public bool jumpingOver;
    protected bool crouching;
    protected bool currentlyAttacking;
    [HideInInspector]public bool canLightPunch;
    protected bool standLightPunchCurrently;
    [HideInInspector]public bool canHardPunch;
    protected bool standHardPunchCurrently;
    [HideInInspector]public bool canLightKick;
    protected bool standLightKickCurrently;
    [HideInInspector]public bool canHardKick;
    protected bool standHardKickCurrently;
    protected bool crouchLightPunchCurrently;
    protected bool crouchHardPunchCurrently;
    protected bool crouchLightKickCurrently;
    protected bool crouchHardKickCurrently;
    protected bool jumpingPunchCurrently;
    protected bool jumpingKickCurrently;
    [HideInInspector]public bool lightSpecial1Currently;
    [HideInInspector]public bool lightSpecial2Currently;
    [HideInInspector]public bool lightSpecial3Currently;
    [HideInInspector]public bool hardSpecial1Currently;
    [HideInInspector]public bool hardSpecial2Currently;
    [HideInInspector]public bool hardSpecial3Currently;
    protected int lastHitStun;
    protected int lastHitStunBlock;
    private bool hitStunFreezeAnim;
    [HideInInspector] public bool beenHitTorso;
    [HideInInspector] public bool beenHitHead;
    [HideInInspector] public bool beenHitLeg;
    [HideInInspector] public bool blockedHigh;
    [HideInInspector] public bool blockedLow;

    [Header("Hitboxes and hurtboxes")]

    // Hurtboxes
    public Collider2D[] hurtbox;

    public GameObject hurtBoxes;
    public GameObject hitBoxes;

    protected override void Controls(){
        if (enableControls){
            // Analógico
            if (isPlayer1){
                moveHRaw = Input.GetAxisRaw("Horizontal");
                moveVRaw = Input.GetAxisRaw("Vertical");
            }
            else{
                moveHRaw = Input.GetAxisRaw("Horizontal2");
                moveVRaw = Input.GetAxisRaw("Vertical2");
            }

            // Botões
            if (isPlayer1){
                xButton = Input.GetKeyDown(KeyCode.Joystick1Button1);
                square = Input.GetKeyDown(KeyCode.Joystick1Button0);
                circle = Input.GetKeyDown(KeyCode.Joystick1Button2);
                triangle = Input.GetKeyDown(KeyCode.Joystick1Button3);
                r2 = Input.GetKey(KeyCode.Joystick1Button7);
            }
            else{
                /*xButton = Input.GetKeyDown(KeyCode.Joystick2Button1);
                square = Input.GetKeyDown(KeyCode.Joystick2Button0);
                circle = Input.GetKeyDown(KeyCode.Joystick2Button2);
                triangle = Input.GetKeyDown(KeyCode.Joystick2Button3);
                r2 = Input.GetKey(KeyCode.Joystick2Button7);*/
                
                xButton = Input.GetKeyDown(KeyCode.Z);
                square = Input.GetKeyDown(KeyCode.X);
                circle = Input.GetKeyDown(KeyCode.C);
                triangle = Input.GetKeyDown(KeyCode.V);
                r2 = Input.GetKey(KeyCode.Space);
            }
        }
    }

    protected override void ComputeVelocity(){
        // Vira o personagem
        if (isFlippedSide && !hasFlippedSide){
            transform.position = new Vector3(transform.position.x + 20, transform.position.y, transform.position.z);
            flipCharacterLeft();
            hasFlippedSide = true;
        }

        // Pega as inputs do controle
        float moveH = 0;
        float moveV = 0;

        posX = transform.position.x;

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
        KeyCheckDash(moveHRaw, moveVRaw);
        if (backTimerDashBool){
            backDashTimer++;
        }

        if (frontTimerDashBool){
            frontDashTimer++;
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
        if (dashBackward >= 1 && grounded && !currentlyDashing && !crouching && !currentlyAttacking && canDash && !jumpingOver){
            dashFrames = dashFrameTotal;
            targetVelocity = new Vector2(1, 0) * -dashSpeed;
            dashBackward = 0;
            dashCooldown = dashCooldownTotal;
        }

        if (dashForward >= 1 && grounded && !currentlyDashing && !crouching && !currentlyAttacking && canDash && !jumpingOver){
            dashFrames = dashFrameTotal;
            targetVelocity = new Vector2(1, 0) * dashSpeed;
            dashForward = 0;
            dashCooldown = dashCooldownTotal;
        }

        // Checa os pulos
        if (moveVRaw > 0.4f && moveHRaw > 0.6f && grounded && ableToMove && !jumpingOver){
            dashFrames = 0;
            walkingSpeed = jumpTakeOffHorizontal;
            velocity.y = jumpTakeOffSpeed;
        }
        else if (moveVRaw > 0.4f && moveHRaw < -0.6f && grounded && ableToMove && !jumpingOver){
            dashFrames = 0;
            walkingSpeed = jumpTakeOffHorizontal;
            velocity.y = jumpTakeOffSpeed;
        }
        else if (moveVRaw > 0.6f && grounded && ableToMove && !jumpingOver){
            walkingSpeed = 0;
            velocity.y = jumpTakeOffSpeed;
        }
        else{
            walkingSpeed = speedTotal;
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
        if (moveVRaw < -0.6f && grounded && !currentlyDashing && !beenHitTorso && !beenHitLeg && !beenHitHead && !jumpingOver){
            crouching = true;
            ableToMove = false;
            if (!beingJumpedOver && !currentlyBlockingGeneral && !currentlyAttacking){
                targetVelocity = Vector2.zero;
            }
        }
        else{
            crouching = false;
        }

        // Faz o personagem andar
        if (grounded && !crouching && !currentlyDashing && !currentlyAttacking && !beenHitTorso && !beenHitLeg && !beenHitHead && !currentlyBlockingGeneral && !blockedHigh && !blockedLow && !beingJumpedOver &&
            !jumpingOver){
            ableToMove = true;
        }
        else{
            ableToMove = false;
        }

        if (grounded && ableToMove){
            targetVelocity = move * walkingSpeed;
        }

        // Faz com que as hitboxes sigam o personagem
        hitBoxes.transform.position = transform.position;
        hurtBoxes.transform.position = transform.position;
        hitBoxes.transform.localScale = new Vector3(transform.localScale.x, hitBoxes.transform.localScale.y, hitBoxes.transform.localScale.z);
        hurtBoxes.transform.localScale = new Vector3(transform.localScale.x, hurtBoxes.transform.localScale.y, hurtBoxes.transform.localScale.z);
    }

    // Checagem de botões para os dashes
    void KeyCheckDash(float moveH, float moveV){
        // Checa se o jogador deixou o analogico no neutro antes de repetir o movimento (aplica-se apenas para dash)
        if (moveH > -0.5f && moveH < 0.5f && moveV > -0.5f && moveV < 0.5f){
            canKeyCheck = true;
        }

        // Dash window
        float dashWindow = 15;

        // Checa o analogico para trás
        if (moveH == -1 && canKeyCheck){
            if (backDashTimer < dashWindow && grounded){
                dashBackward += 1;
            }
            else{
                dashBackward = 0;
            }

            backDashTimer = 0;
            backTimerDashBool = true;
            canKeyCheck = false;
        }

        // Checa o analogico para frente
        if (moveH == 1 && canKeyCheck){
            if (frontDashTimer < dashWindow && grounded){
                dashForward += 1;
            }
            else{
                dashForward = 0;
            }

            frontDashTimer = 0;
            frontTimerDashBool = true;
            canKeyCheck = false;
        }
    }

    // Checagem de botões para os especiais
    public void KeyCheckSpecial(float moveH, float moveV){
        // Direcionais
        if (downTimerSpecial < 100){
            downTimerSpecial++;
        }

        if (upTimerSpecial < 100){
            upTimerSpecial++;
        }

        if (rightTimerSpecial < 100){
            rightTimerSpecial++;
        }

        if (leftTimerSpecial < 100){
            leftTimerSpecial++;
        }

        // Teste baixo
        if (moveV < -0.6 && moveH < 0.3 && moveH > -0.3){
            downTimerSpecial = 0;
        }

        // Teste cima
        if (moveV > 0.6 && moveH < 0.3 && moveH > -0.3){
            upTimerSpecial = 0;
        }

        // Teste direita/esquerda
        if (moveH > 0.6 && moveV < 0.3 && moveV > -0.3 && isLeft){
            rightTimerSpecial = 0;
        }
        else if (moveH > 0.6 && moveV < 0.3 && moveV > -0.3 && !isLeft){
            leftTimerSpecial = 0;
        }

        // Teste esquerda/direita
        if (moveH < -0.6 && moveV < 0.3 && moveV > -0.3 && isLeft){
            leftTimerSpecial = 0;
        }
        else if (moveH < -0.6 && moveV < 0.3 && moveV > -0.3 && !isLeft){
            rightTimerSpecial = 0;
        }

        if (xButtonTimerSpecial < 100){
            xButtonTimerSpecial++;
        }

        if (squareTimerSpecial < 100){
            squareTimerSpecial++;
        }

        if (circleTimerSpecial < 100){
            circleTimerSpecial++;
        }

        if (triangleTimerSpecial < 100){
            triangleTimerSpecial++;
        }

        // Botões
        if (xButton){
            xButtonTimerSpecial = 0;
        }

        if (square){
            squareTimerSpecial = 0;
        }

        if (circle){
            circleTimerSpecial = 0;
        }

        if (triangle){
            triangleTimerSpecial = 0;
        }
    }

    // Animação
    protected override void animationStart(){
        animator = GetComponent<Animator>();
        ghost = GetComponent<ghostTrail>();
    }

    protected override void animationUpdate(){
        // Gameplay
        if (grounded){
            animator.SetBool("youWin", won);
            animator.SetBool("youLose", lost);
        }

        // Movimento
        if (grounded){
            animator.SetFloat("movementSpeed", Math.Abs(targetVelocity.x));
        }
        else{
            animator.SetFloat("movementSpeed", 0);
        }

        animator.SetBool("crouching", crouching);

        animator.SetBool("jumping", jumping);

        // Blocking
        animator.SetBool("blockingHigh", isBlockingHigh);
        animator.SetBool("blockingLow", isBlockingLow);

        animator.SetBool("beenHitBlockHigh", blockedHigh);
        animator.SetBool("beenHitBlockLow", blockedLow);

        animator.SetInteger("blockHitStun", lastHitStunBlock);

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
        
        // Specials
        // Light special 1
        animator.SetBool("lightSpecial1", lightSpecial1Currently);
        // Light special 2
        animator.SetBool("lightSpecial2", lightSpecial2Currently);
        // Light special 3
        animator.SetBool("lightSpecial3", lightSpecial3Currently);
        // Hard special 1
        animator.SetBool("hardSpecial1", hardSpecial1Currently);
        // Hard special 2
        animator.SetBool("hardSpecial2", hardSpecial2Currently);
        // Hard special 3
        animator.SetBool("hardSpecial3", hardSpecial3Currently);
    }

    // Core gameplay
    protected override void CoreGameplayStart(){
        speedTotal = walkingSpeed;
        maxHealth = health;

        upTimerSpecial = 100;
        downTimerSpecial = 100;
        rightTimerSpecial = 100;
        leftTimerSpecial = 100;

        xButtonTimerSpecial = 100;
        squareTimerSpecial = 100;
        triangleTimerSpecial = 100;
        circleTimerSpecial = 100;
    }

    protected override void CoreGameplayUpdate(){
        // Cria o sistema de hitstun modular
        if (lastHitStun > 0){
            lastHitStun--;
        }

        if (lastHitStunBlock > 0){
            lastHitStunBlock--;
        }

        if (lastHitStunBlock == 0){
            gotHitBlockHighEnd();
            gotHitBlockLowEnd();
        }

        // Blocking
        if (r2 && !crouching && grounded && !currentlyAttacking && !currentlyDashing && !jumpingOver){
            isBlockingHigh = true;
        }
        else{
            isBlockingHigh = false;
        }

        if (r2 && crouching && grounded && !currentlyAttacking && !currentlyDashing && !jumpingOver){
            isBlockingLow = true;
        }
        else{
            isBlockingLow = false;
        }

        if (isBlockingHigh || isBlockingLow){
            currentlyBlockingGeneral = true;
            ableToMove = false;
            targetVelocity = Vector2.zero;
        }
        else{
            currentlyBlockingGeneral = false;
        }
        
        KeyCheckSpecial(moveHRaw, moveVRaw);

        // Light punches
        if (square && grounded && !crouching && !currentlyAttacking && !currentlyDashing && !jumpingOver && canLightPunch){
            StandLightPunch();
        }
        else if (square && grounded && crouching && !currentlyAttacking && !currentlyDashing && !jumpingOver && canLightPunch){
            CrouchLightPunch();
        }

        // Hard punches
        if (triangle && grounded && !crouching && !currentlyAttacking && !currentlyDashing && !jumpingOver && canHardPunch){
            StandHardPunch();
        }
        else if (triangle && grounded && crouching && !currentlyAttacking && !currentlyDashing && !jumpingOver && canHardPunch){
            CrouchHardPunch();
        }

        // Light kick's
        if (xButton && grounded && !crouching && !currentlyAttacking && !currentlyDashing && !jumpingOver && canLightKick){
            StandLightKick();
        }
        else if (xButton && grounded && crouching && !currentlyAttacking && !currentlyDashing && !jumpingOver && canLightKick){
            CrouchLightKick();
        }

        // Hard kick's
        if (circle && grounded && !crouching && !currentlyAttacking && !currentlyDashing && !jumpingOver && canHardKick){
            StandHardKick();
        }
        else if (circle && grounded && crouching && !currentlyAttacking && !currentlyDashing && !jumpingOver && canHardKick){
            CrouchHardKick();
        }

        // Jump punch
        if (square && !grounded && !crouching && !currentlyAttacking && !currentlyDashing ||
            triangle && !grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            JumpPunch();
        }

        // Jump kick
        if (xButton && !grounded && !crouching && !currentlyAttacking && !currentlyDashing ||
            circle && !grounded && !crouching && !currentlyAttacking && !currentlyDashing){
            JumpKick();
        }
    }

    protected void CurrentlyAttacking(bool attacking){
        // Se o personagem atacar, ele ira ficar parado
        targetVelocity = Vector2.zero;
        velocity.y = 0;
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
    
    // Specials
    // Light special 1
    public void LightSpecial1(){
        CurrentlyAttacking(true);
        lightSpecial1Currently = true;
    }

    public void LightSpecial1Stop(){
        CurrentlyAttacking(false);
        lightSpecial1Currently = false;
    }
    
    // Light special 2
    public void LightSpecial2(){
        CurrentlyAttacking(true);
        lightSpecial2Currently = true;
    }

    public void LightSpecial2Stop(){
        CurrentlyAttacking(false);
        lightSpecial2Currently = false;
    }
    
    // Light special 3
    public void LightSpecial3(){
        CurrentlyAttacking(true);
        lightSpecial3Currently = true;
    }

    public void LightSpecial3Stop(){
        CurrentlyAttacking(false);
        lightSpecial3Currently = false;
    }
    
    // Hard special 1
    public void HardSpecial1(){
        CurrentlyAttacking(true);
        hardSpecial1Currently = true;
    }

    public void HardSpecial1Stop(){
        CurrentlyAttacking(false);
        hardSpecial1Currently = false;
    }
    
    // Hard special 2
    public void HardSpecial2(){
        CurrentlyAttacking(true);
        hardSpecial2Currently = true;
    }

    public void HardSpecial2Stop(){
        CurrentlyAttacking(false);
        hardSpecial2Currently = false;
    }
    
    // Hard special 3
    public void HardSpecial3(){
        CurrentlyAttacking(true);
        hardSpecial3Currently = true;
    }

    public void HardSpecial3Stop(){
        CurrentlyAttacking(false);
        hardSpecial3Currently = false;
    }

    // Hit stun animations
    // Hitstun baseado nas frames totais
    public void addHitStun(int hitStun){
        lastHitStun = hitStun;
    }

    public void addHitStunBlock(int hitStun){
        lastHitStunBlock = hitStun;
    }

    public void gotHitTorsoStart(){
        beenHitHead = false;
        beenHitTorso = true;
        beenHitLeg = false;
        ableToMove = false;
        targetVelocity = Vector2.zero;
    }

    public void gotHitTorsoEnd(){
        beenHitTorso = false;
    }

    public void gotHitHeadStart(){
        beenHitHead = true;
        beenHitTorso = false;
        beenHitLeg = false;
        ableToMove = false;
        targetVelocity = Vector2.zero;
    }

    public void gotHitHeadEnd(){
        beenHitHead = false;
    }

    public void gotHitLegStart(){
        beenHitHead = false;
        beenHitTorso = false;
        beenHitLeg = true;
        ableToMove = false;
        targetVelocity = Vector2.zero;
    }

    public void gotHitLegEnd(){
        beenHitLeg = false;
    }

    public void gotHitBlockHighStart(){
        blockedHigh = true;
    }

    public void gotHitBlockHighEnd(){
        blockedHigh = false;
    }

    public void gotHitBlockLowStart(){
        blockedLow = true;
    }

    public void gotHitBlockLowEnd(){
        blockedLow = false;
    }

    // Para todos ataques atuais
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
        LightSpecial1Stop();
        LightSpecial2Stop();
        LightSpecial3Stop();
        HardSpecial1Stop();
        HardSpecial2Stop();
        HardSpecial3Stop();
    }

    // Controla a vida do personagem
    public void changeHealth(int healthChanged){
        health += healthChanged;
    }

    // Faz com que o contador de frames comece ou pare
    public void ActiveFrameStart(int attack){
        hurtbox[attack].GetComponent<AttackCheck>().IsHittingTrue();
    }

    public void ActiveFrameStop(int attack){
        hurtbox[attack].GetComponent<AttackCheck>().IsHittingFalse();
    }

    public void flipCharacterLeft(){
        transform.localScale = new Vector3(transform.root.localScale.x * -1, transform.root.localScale.y, transform.root.localScale.z);
    }

    public void flipCharacterRight(){
        transform.localScale = new Vector3(transform.root.localScale.x * 1, transform.root.localScale.y, transform.root.localScale.z);
    }
    
    public bool testeDeSpecial(){
        if (grounded && !currentlyAttacking && !currentlyDashing && !jumpingOver){
            return true;
        }
        return false;
    }
    
}