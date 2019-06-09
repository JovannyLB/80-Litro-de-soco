using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mensageiro : MonoBehaviour
{
    public static int leftPlayerIndex;
    public static int rightPlayerIndex;

    private void Start()
    {
        Debug.Log("Left Player: " + leftPlayerIndex);
        Debug.Log("Right Player: " + rightPlayerIndex);
    }

    public int GetLeftPlayerIndex()
    {
        return leftPlayerIndex;
    }
    public int GetRightPlayerIndex()
    {
        return rightPlayerIndex;
    }

    public void SetLeftPlayerIndex(int left)
    {
        leftPlayerIndex = left;
    }
    
    public void SetRightPlayerIndex(int right)
    {
        rightPlayerIndex = right;
    }
 
 }
