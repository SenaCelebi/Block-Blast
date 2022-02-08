using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject menuUI, gameplayUI, winUI, gameOverUI;
    
    private void Awake()
    {
        initialGameUI();
    }

    private void initialGameUI()
    {
        menuUI.SetActive(true);
        gameplayUI.SetActive(false);
        winUI.SetActive(false);
        gameOverUI.SetActive(false);
    }
}
