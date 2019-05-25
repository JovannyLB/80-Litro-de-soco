using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BGOScript : MonoBehaviour
{
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
    }
}
