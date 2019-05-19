using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public RectTransform lines;
    
    void FixedUpdate()
    {
        lines.DOAnchorPosY(((lines.transform.root.position.y) - 1080f), 0.5f, false);
        lines.DOAnchorPosY(((lines.transform.root.position.y) - 1080f), 0.5f, false).SetDelay(0.5f);
        lines.DOAnchorPosY(((lines.transform.root.position.y) - 1080f), 0.5f, false).SetDelay(0.5f);
        lines.DOAnchorPosY(((lines.transform.root.position.y) - 1080f), 0.5f, false).SetDelay(0.5f);
    }

}
