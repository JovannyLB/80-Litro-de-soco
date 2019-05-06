using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpOverCheck : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other){
        transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 10;
    }
}
