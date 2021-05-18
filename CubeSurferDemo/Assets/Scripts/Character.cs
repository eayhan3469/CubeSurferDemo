using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private GameObject characterObject;

    [SerializeField]
    private float speed;

    public static Character Instance { get; private set; }

    public GameObject CharacterObject
    {
        get { return characterObject; }
    }

    public bool IsRotating { get; set; }
    public int Direction { get; set; }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    void Awake()
    {
        if (Instance != null)
            Debug.LogError("'Character' must be one!");
        else
            Instance = this;
    }
}
