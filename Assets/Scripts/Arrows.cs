using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Arrows : MonoBehaviour
{
    [FormerlySerializedAs("gamaManager")] [SerializeField] private GameManager gameManager;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private GameObject cursor;
    private Slot[] _arrowSlots;
    // Start is called before the first frame update
    void Start()
    {
        _arrowSlots = gridLayoutGroup.GetComponentsInChildren<Slot>();
        for (var i = 0; i < _arrowSlots.Length; i++)
        {
            var index = i;
            var arrowData = gameManager.GetArrowData(index);
            _arrowSlots[i].SetArrowIcon(arrowData.icon);
        }
    }

    // Update is called once per frame
    void SetCursorPosition(int arrowIndex)
    {
        cursor.transform.position = _arrowSlots[arrowIndex].transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var index = gameManager.PreviousArrow();
            SetCursorPosition(index);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            var index = gameManager.NextArrow();
            SetCursorPosition(index);
        }
    }

}
