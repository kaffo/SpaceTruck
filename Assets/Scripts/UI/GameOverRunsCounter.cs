using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverRunsCounter : MonoBehaviour
{
    public Text gameOverText;

    private void Start()
    {
        if (gameOverText == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        SetGameOverText();
    }

    public void SetGameOverText()
    {
        string gameOverString = $"Game Over\nRuns: {GameManager.Instance.runCount.ToString()}";
        gameOverText.text = gameOverString;
    }
}
