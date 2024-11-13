using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<TextMeshProUGUI> listScoreText = new List<TextMeshProUGUI>();

    int score = 0;

    void Start()
    {
        for(int i = 0; i < listScoreText.Count; i++)
        {
            listScoreText[i].text = score.ToString();
        }
    }
    public void AddScore()
    {
        for (int i = 0; i < listScoreText.Count; i++)
        {
            score += 1;
            listScoreText[i].text = score.ToString();
        }
    }
}
