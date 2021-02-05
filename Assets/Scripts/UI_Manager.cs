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
    private GameObject GameOverPanel = null;

    public void DisplayGameOverPanel(float yourScore, float topScore)
    {
        GameOverPanel = Instantiate(GameOverPanelPrefab, Canvas);
        GameOverPanel.GetComponent<GameOver>().SetData(yourScore, topScore);
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
        if (StartLabel != null)
        {
            Destroy(StartLabel);
        }
    }
}
