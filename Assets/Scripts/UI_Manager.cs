using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanelPrefab = null;
    
    
    private GameObject gameOverPanel = null;

    private Text timeText = null;
    private GameObject startLabel = null;
    
    // Start is called before the first frame update
    void Start()
    {
        timeText = FindObjectOfType<Canvas>().transform.Find("TimeText").GetComponent<Text>();
        startLabel = FindObjectOfType<Canvas>().transform.Find("StartLabel").gameObject;
        //timeText = GameObject.FindGameObjectWithTag("TimeText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayGameOverPanel(float yourScore, float topScore)
    {
        gameOverPanel = Instantiate(gameOverPanelPrefab, Vector3.zero, Quaternion.identity);
        gameOverPanel.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

        var yourScoreText = gameOverPanel.transform.Find("YourScoreText");
        var topScoreText = gameOverPanel.transform.Find("TopScoreText");

        yourScoreText.GetComponent<Text>().text = yourScore.ToString("F2");
        topScoreText.GetComponent<Text>().text = "Top Score: " + topScore.ToString("F2");

        //timeText.text = "00";
    }

    public void HideGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            Destroy(gameOverPanel);
        }    
    }

    public void UpdateTime(float currentTIme)
    {
        timeText.text = "  Time: " + currentTIme.ToString("F2");
    }

    public void HideStartLabel()
    {
        if (startLabel != null)
        {
            Destroy(startLabel);
        }
    }
}
