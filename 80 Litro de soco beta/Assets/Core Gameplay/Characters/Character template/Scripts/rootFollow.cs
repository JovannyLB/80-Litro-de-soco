using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rootFollow : MonoBehaviour{
    public GameObject root;

    void Update(){
        transform.position = root.transform.position;
    }
}