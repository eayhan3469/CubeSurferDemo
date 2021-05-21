using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Image LevelProgressImage;

    [SerializeField]
    private GameObject tapToPlayPanel;

    [SerializeField]
    private GameObject finishPanel;

    [SerializeField]
    private Text finishText;


    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("'GameManager' must be one");
        else
            Instance = this;
    }

    public void OnPlayClicked()
    {
        GameManager.Instance.HasGameStart = true;
        tapToPlayPanel.SetActive(false);
    }

    public void OnFinishClicked()
    {
        if (GameManager.Instance.HasLevelSuccess)
        {
            var nextLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;

            if (nextLevel > 3)
            {
                PlayerPrefs.SetInt("CurrentLevel", 1);
                nextLevel = 1;
            }
            else
                PlayerPrefs.SetInt("CurrentLevel", nextLevel);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ShowFinishPanel()
    {
        if (GameManager.Instance.HasLevelSuccess)
        {
            if (GameManager.Instance.PointMultiplier > 1)
            {
                finishText.text = string.Format("Your Point is '{0}'\nTap To Next Level", GameManager.Instance.PointMultiplier * (Character.Instance.Cubes.Count + GameManager.Instance.PointMultiplier));
            }
            else
            {
                finishText.text = string.Format("Your Point is '{0}'\nTap To Next Level", GameManager.Instance.PointMultiplier * Character.Instance.Cubes.Count);
            }
        }
        else
        {
            finishText.text = "Failed, tap for play again";
        }

        finishPanel.SetActive(true);
    }
}
