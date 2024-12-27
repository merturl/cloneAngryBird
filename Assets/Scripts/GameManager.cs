using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameStatus
{
    Playing,
    Clear,
    GameOver,
} 
public class GameManager : MonoBehaviour
{
    
    [SerializeField]private List<ArrowData> arrowsData = new List<ArrowData>();
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;
    
    private int currentArrowIndex = 0;
    private GameStatus gameStatus = GameStatus.Playing;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameStatus GetGameStatus()
    {
        return gameStatus;
    }

    public ArrowData GetArrowData(int index)
    {
        return arrowsData[index];
    }

    public ArrowData GetCurrentArrowData()
    {
        return arrowsData[currentArrowIndex];
    }

    public List<ArrowData> GetArrowsData()
    {
        return arrowsData;
    }

    public int NextArrow()
    {
        var length = arrowsData.Count;
        currentArrowIndex = (currentArrowIndex + 1)%length;
        return currentArrowIndex;
    }

    public int PreviousArrow()
    {
        var length = arrowsData.Count;
        currentArrowIndex = (currentArrowIndex - 1 + length)%length;
        return currentArrowIndex;
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        gameStatus = GameStatus.GameOver;
        gameOverPanel.SetActive(true);
    }

    public void GameClear()
    {
        Debug.Log("GameClear");
        gameStatus = GameStatus.Clear;
        gameClearPanel.SetActive(true);
    }
}
