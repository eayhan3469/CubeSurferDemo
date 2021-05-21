using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool HasGameStart { get; internal set; }
    public bool HasGameOver { get; internal set; }
    public bool HasLevelSuccess { get; internal set; }
    public int PointMultiplier { get; set; }
    public Transform FinishLine;

    private float _initialDistance;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("'GameManager' must be one");
        else
            Instance = this;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", 1);
        }

        var levelObject = Resources.Load<GameObject>(String.Format("Levels/{0}", PlayerPrefs.GetInt("CurrentLevel").ToString()));
        Instantiate(levelObject);

        PointMultiplier = 1;
        FinishLine = GameObject.FindGameObjectWithTag("Finish").transform;
        _initialDistance = Vector3.Distance(Character.Instance.CharacterObject.transform.position, FinishLine.position);
    }

    void Update()
    {
        if (HasGameOver)
        {
            UIManager.Instance.ShowFinishPanel();
        }

        if (Character.Instance.Cubes.Count <= 0)
        {
            Character.Instance.CharacterObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            HasLevelSuccess = false;
            HasGameOver = true;
        }

        var distance = Vector3.Distance(Character.Instance.CharacterObject.transform.position, FinishLine.position);
        UIManager.Instance.LevelProgressImage.fillAmount = Mathf.Clamp((_initialDistance - distance) / _initialDistance, 0, 1);
    }
}
