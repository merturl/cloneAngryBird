using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(GoMainScene);
    }
    public void GoMainScene()
    {
        Debug.Log("ResetScene");
        SceneManager.LoadScene("Main");
    }
}
