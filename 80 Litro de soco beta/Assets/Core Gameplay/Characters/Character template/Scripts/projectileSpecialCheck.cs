using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileSpecialCheck : MonoBehaviour{

    [Header("Projectile frame data")] 
    public int speed;
    public int hitStunFrames;
    public int blockHitStunFrames;
    public int damage;
    public int specialGainOnHit;
    public int specialGainOnWhiff;
    public bool isHard;
    
    public GameObject projectile;

    [HideInInspector] public ParticleSystem blood;
    
    public void instantiateProjectile(){
        var newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        newProjectile.GetComponent<projectileLifeCheck>().ownPlayerScript = transform.root.GetChild(0).GetComponent<playerPlataformerController>();
        newProjectile.GetComponent<projectileLifeCheck>().speed = speed;
        newProjectile.GetComponent<projectileLifeCheck>().hitStunFrames = hitStunFrames;
        newProjectile.GetComponent<projectileLifeCheck>().blockHitStunFrames = blockHitStunFrames;
        newProjectile.GetComponent<projectileLifeCheck>().damage = damage;
        newProjectile.GetComponent<projectileLifeCheck>().blood = blood;
        newProjectile.GetComponent<projectileLifeCheck>().specialGainOnHit = specialGainOnHit;
        newProjectile.GetComponent<projectileLifeCheck>().specialGainOnWhiff = specialGainOnWhiff;
        newProjectile.GetComponent<projectileLifeCheck>().isHard = isHard;
    }
    
}
