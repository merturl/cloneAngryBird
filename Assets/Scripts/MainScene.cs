using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    private Button[] buttons; 
    // Start is called before the first frame update
    void Start()
    {
        buttons = gridLayoutGroup.GetComponentsInChildren<Button>();
        for (var i = 0; i < buttons.Length; i++)
        {
            var index = i;
            buttons[i].onClick.AddListener(() =>LoadScene(index+1));
        }
    }

    private void LoadScene(int index)
    {
        SceneManager.LoadScene("Stage"+index);
    }
}
