using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text YourScoreText = null;
    [SerializeField] private Text TopScoreText = null;

    internal void SetData(float yourScore, float topScore)
    {
        YourScoreText.text = "Your Score: " + yourScore.ToString("F2");
        TopScoreText.text = "Top Score: " + topScore.ToString("F2");
    }
}
