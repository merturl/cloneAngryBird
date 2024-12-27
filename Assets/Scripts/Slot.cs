using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image arrowIcon;
    // Start is called before the first frame update
    public void SetArrowIcon(Sprite icon)
    {
        arrowIcon.sprite = icon;
    }
}
