using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour
{
    public int score;

    private GlobalBlackBoard gbb;

    public CanvasGroup HUD;
    public TextMeshProUGUI HUDScore;
    public TextMeshProUGUI timerText;

    public CanvasGroup EndScreen;
    public TextMeshProUGUI EndScore;

    private float gameTimer;
    public float gameLength;

    private bool gameRunning = true;
    // Start is called before the first frame update
    void Start()
    {
        gbb = GameObject.FindGameObjectWithTag("GBB").GetComponent<GlobalBlackBoard>();

        HideCanvasGroup(EndScreen);
        ShowCanvasGroup(HUD);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(gameRunning)
        {
            score += (int)gbb.scoreDelta;

            HUDScore.text = score.ToString();

            gameTimer += Time.deltaTime;


            if (gameTimer >= gameLength)
            {
                EndGame();
            }
        }
        else
        {

        }
        string timeText;
        int minutes = (int)(gameLength - gameTimer) / 60;
        int seconds = (int)(gameLength - gameTimer) % 60;

        timeText = minutes.ToString() + ": " + seconds.ToString();
        timerText.text = timeText;
    }

    public void EndGame()
    {
        gameRunning = false;

        GameObject[] FFs = GameObject.FindGameObjectsWithTag("FireFighter");
        foreach(GameObject FF in FFs)
        {
            FF.SetActive(false);
        }

        HideCanvasGroup(HUD);
        ShowCanvasGroup(EndScreen);
        EndScore.text = score.ToString();
    }

    private void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
