using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreatIndicatorUIUpdate : MonoBehaviour
{
    [SerializeField] private Sprite emptyIndicator;
    [SerializeField] private Sprite lowIndicator;
    [SerializeField] private Sprite mediumIndicator;
    [SerializeField] private Sprite highIndicator;

    [SerializeField] private Image topIndicator;
    [SerializeField] private Image rightIndicator;
    [SerializeField] private Image bottomIndicator;
    [SerializeField] private Image leftIndicator;
      
    private GameManager.THREAT_LEVEL topThreat = GameManager.THREAT_LEVEL.NONE;
    private GameManager.THREAT_LEVEL rightThreat = GameManager.THREAT_LEVEL.NONE;
    private GameManager.THREAT_LEVEL bottomThreat = GameManager.THREAT_LEVEL.NONE;
    private GameManager.THREAT_LEVEL leftThreat = GameManager.THREAT_LEVEL.NONE;

    public GameManager.THREAT_LEVEL TopThreat
    {
        set
        {
            topThreat = value;
            UpdateThreatImage(topThreat, topIndicator);
        }
        get { return topThreat; }
    }

    public GameManager.THREAT_LEVEL RightThreat
    {
        set
        {
            rightThreat = value;
            UpdateThreatImage(rightThreat, rightIndicator);
        }
        get { return topThreat; }
    }

    public GameManager.THREAT_LEVEL BottomThreat
    {
        set
        {
            bottomThreat = value;
            UpdateThreatImage(bottomThreat, bottomIndicator);
        }
        get { return topThreat; }
    }

    public GameManager.THREAT_LEVEL LeftThreat
    {
        set
        {
            leftThreat = value;
            UpdateThreatImage(leftThreat, leftIndicator);
        }
        get { return topThreat; }
    }

    // 0 - 100
    private void UpdateThreatImage(GameManager.THREAT_LEVEL newThreat, Image threatImage)
    {
        switch (newThreat)
        {
            case GameManager.THREAT_LEVEL.NONE:
                threatImage.sprite = emptyIndicator;
                break;
            case GameManager.THREAT_LEVEL.LOW:
                threatImage.sprite = lowIndicator;
                break;
            case GameManager.THREAT_LEVEL.MEDIUM:
                threatImage.sprite = mediumIndicator;
                break;
            case GameManager.THREAT_LEVEL.HIGH:
                threatImage.sprite = highIndicator;
                break;
            default:
                threatImage.sprite = emptyIndicator;
                break;
        }
    }

    private void Start()
    {
        if (topIndicator == null || rightIndicator == null || bottomIndicator == null || leftIndicator == null ||
            emptyIndicator == null || lowIndicator == null || mediumIndicator == null || highIndicator == null )
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnThreatChange += OnThreatChange;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnThreatChange -= OnThreatChange;
    }

    private void OnThreatChange(GameManager.THREAT_LEVEL top, GameManager.THREAT_LEVEL right, GameManager.THREAT_LEVEL bottom, GameManager.THREAT_LEVEL left)
    {
        TopThreat = top;
        RightThreat = right;
        BottomThreat = bottom;
        LeftThreat = left;
    }
}
