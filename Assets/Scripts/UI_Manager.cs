using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPanelPrefab = null;
    [SerializeField] private Text TimeText = null;
    [SerializeField] private GameObject StartLabel = null;
    [SerializeField] private Transform Canvas = null;
    [SerializeField] private Text ScoreText = null;
    private GameObject GameOverPanel = null;

    public void ShowGameOverPanel(float yourScore, float topScore)
    {
        if (yourScore > topScore)
	    {
            topScore = yourScore;
            ScoreText.text = $"Top Score: {topScore.ToString("F2")}";
	    }
#if !UNITY_WEBGL
        PlayerPrefs.SetFloat("topscore", topScore);
#endif
        GameOverPanel = Instantiate(GameOverPanelPrefab, Canvas);
        GameOverPanel.GetComponent<GameOver>().SetData(yourScore, topScore);
    }

    private void Start()
    {
#if !UNITY_WEBGL
        var topScore = PlayerPrefs.GetFloat("topscore", 0);
        GameManager.Instance.SetTopScore(topScore);
        ScoreText.text = $"Top Score: {topScore.ToString("F2")}";
#endif
    }

    public void HideGameOverPanel()
    {
        if (GameOverPanel != null)
        {
            Destroy(GameOverPanel);
        }    
    }

    public void UpdateTime(float currentTime)
    {
        TimeText.text = "  Time: " + currentTime.ToString("F2");
    }

    public void HideStartLabel()
    {
        StartLabel.gameObject.SetActive(false);
    }

    public void ShowStartLabel()
    {
        StartLabel.gameObject.SetActive(true);
    }
}
