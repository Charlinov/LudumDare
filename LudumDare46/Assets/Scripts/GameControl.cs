using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour
{
    public int score;

    private GlobalBlackBoard gbb;

    public CanvasGroup HUD;
    public CanvasGroup EndScreen;

    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        gbb = GameObject.FindGameObjectWithTag("GBB").GetComponent<GlobalBlackBoard>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        score += (int)gbb.scoreDelta;
    }

    public void EndGame()
    {

    }
}
