using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public GameObject startingText;
    public static bool isGamestarted;

    public static int numberOfCoins;
    public Text CoinText;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        gameOver = false;
        isGamestarted = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;

        }
        CoinText.text = "Coins: " + numberOfCoins;
        if (SwipeManager.tap||Input.anyKeyDown)
        {
            isGamestarted = true;
            Destroy(startingText);
        }
    }
}
