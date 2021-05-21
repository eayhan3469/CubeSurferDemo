﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private GameObject characterObject;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpPower;

    [SerializeField]
    private Transform cubesParent;

    [SerializeField]
    private GameObject cubeObject;

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

    public List<Cube> Cubes;
    private BoxCollider _characterCollider;

    private void Start()
    {
        _characterCollider = gameObject.GetComponent<BoxCollider>();
        Cubes.AddRange(GetComponentsInChildren<Cube>());
        EnlargeCollider();
    }

    void Awake()
    {
        if (Instance != null)
            Debug.LogError("'Character' must be one!");
        else
            Instance = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CollectableCube")
        {
            AddCube();
            Destroy(collision.gameObject);
        }
    }

    public void AddCube()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower);
        EnlargeCollider();
        var cubePosition = new Vector3(0f, Cubes[Cubes.Count - 1].transform.localPosition.y - cubeObject.transform.localScale.y, 0f);
        var cube = Instantiate(cubeObject, cubesParent);
        Cubes.Add(cube.GetComponent<Cube>());
        cube.transform.localPosition = cubePosition;
    }

    public async void RemoveCube(Cube cube)
    {
        foreach (var c in Cubes)
            c.GetComponent<BoxCollider>().isTrigger = false;

        await Task.Run(() =>
        {
            Task.Delay(500);
        });

        Cubes.Remove(cube);
        ShrinkCollider();

        foreach (var c in Cubes)
            c.GetComponent<BoxCollider>().isTrigger = true;
    }

    public void EnlargeCollider()
    {
        _characterCollider.size = new Vector3(_characterCollider.size.x, _characterCollider.size.y + 1f, _characterCollider.size.z);
        _characterCollider.center = new Vector3(_characterCollider.center.x, _characterCollider.center.y - 0.5f, _characterCollider.center.z);
    }

    public void ShrinkCollider()
    {
        _characterCollider.size = new Vector3(_characterCollider.size.x, _characterCollider.size.y - 1f, _characterCollider.size.z);
        _characterCollider.center = new Vector3(_characterCollider.center.x, _characterCollider.center.y + 0.5f, _characterCollider.center.z);
    }
}
