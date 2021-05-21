using System.Collections;
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

    [SerializeField]
    private Transform trailObject;

    [SerializeField]
    private Material trailMaterial;

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
    private float _lavaTime = 0f;
    private float _lavaTimeInterval = 0.025f;
    private Collider _groundCollider;

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

    private void Update()
    {
        Vector3 closestOnGround = _groundCollider.ClosestPointOnBounds(transform.position);
        LeaveTrail(closestOnGround, cubeObject.transform.localScale.x, trailMaterial);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CollectableCube")
        {
            AddCube();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Road" || collision.gameObject.tag == "Bridge")
        {
            _groundCollider = collision.gameObject.GetComponent<Collider>();
        }
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private void LeaveTrail(Vector3 point, float scale, Material material)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Quad);
        sphere.transform.localScale = Vector3.one * scale;
        sphere.transform.position = point + new Vector3(0f, 0.01f, 0f);
        sphere.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
        sphere.transform.parent = trailObject;
        sphere.GetComponent<Collider>().enabled = false;
        sphere.GetComponent<Renderer>().material = material;
        Destroy(sphere, 3f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Lava")
        {
            _lavaTime += Time.deltaTime;

            if (_lavaTime >= _lavaTimeInterval)
            {
                _lavaTime = 0f;
                Destroy(Cubes[Cubes.Count - 1].gameObject);
                Cubes.Remove(Cubes[Cubes.Count - 1]);
                ShrinkCollider();
            }
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

    public void RemoveCube(Cube cube)
    {
        Cubes.Remove(cube);
        ShrinkCollider();
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
