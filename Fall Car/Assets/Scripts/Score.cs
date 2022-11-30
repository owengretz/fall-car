using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score instance;

    public Transform player;


    private Vector3 playerPos;
    private float distance;

    [HideInInspector] public int score;
    public TMP_Text scoreText;

    private int highscore;
    public TMP_Text highscoreText;

    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");
            highscoreText.text = "highscore: " + highscore.ToString();
        }

        score = 0;
        distance = 0f;
    }

    private void FixedUpdate()
    {
        distance = Mathf.Sqrt(Mathf.Pow(player.position.x, 2) + Mathf.Pow(player.position.z, 2));
        score = (int)(distance / 10);

        scoreText.text = score.ToString();
    }

    public void SetHighScore()
    {
        if (score > highscore)
        {
            highscoreText.text = "highscore: " + score.ToString();
            PlayerPrefs.SetInt("highscore", score);
        }
    }
}
