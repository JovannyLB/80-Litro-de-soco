﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour{
    [HideInInspector]public float minGroundNormalY = 0.65f;
    [HideInInspector]public float graityModified = 1f;
    [HideInInspector]public bool grounded;
    [HideInInspector]public Vector2 targetVelocity;

    protected Vector2 groundNormal;
    [HideInInspector]public Rigidbody2D rb2d;
    [HideInInspector]public Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    [HideInInspector]public bool onTopP;

    void OnEnable(){
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start(){
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        animationStart();
        CoreGameplayStart();
    }

    // Update is called once per frame
    void Update(){
        //targetVelocity = Vector2.zero;
        Controls();
        ComputeVelocity();
        CoreGameplayUpdate();
        animationUpdate();
    }

    protected virtual void ComputeVelocity(){
    }

    protected virtual void animationStart(){
    }

    protected virtual void animationUpdate(){
    }

    protected virtual void Controls(){
    }

    protected virtual void CoreGameplayStart(){
    }

    protected virtual void CoreGameplayUpdate(){
    }

    void FixedUpdate(){
        velocity += graityModified * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 changeInPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * changeInPosition.x;

        Movement(move, false);

        move = Vector2.up * changeInPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement){
        float distance = move.magnitude;

        if (distance > minMoveDistance){
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++){
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++){
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY && !onTopP){
                    grounded = true;
                    if (yMovement){
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0){
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }
}