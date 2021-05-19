using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    public static SwerveMovement Instance;

    private SwerveInputSystem _swerveInputSystem;

    [SerializeField]
    private float swerveSpeed = 0.5f;

    [SerializeField]
    private float maxSwerveAmount = 1f;

    [SerializeField]
    private float speed = 1f;

    private float _maxPos;
    private float _minPos;
    private Transform _road;
    private float _swerveAmount;
    private int _direction = 0;
    private bool _isOnWay = false;

    public float SwerveAmount { get { return _swerveAmount; } }

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("'SwerveMovement' must be one");
        else
            Instance = this;
    }

    private void Start()
    {
        _swerveInputSystem = GetComponent<SwerveInputSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Road")
        {
            _road = collision.gameObject.transform;
            _isOnWay = true;
            CalculateEdges(_direction);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Road")
            _isOnWay = false;
    }

    public void CalculateEdges(int direction)
    {
        if (direction == 0 || direction == 2)
        {
            _maxPos = _road.position.x + (_road.localScale.x / 2 - 0.1f);
            _minPos = _road.position.x + (0.1f - _road.localScale.x / 2);
        }
        else if (direction == 1 || direction == 3)
        {
            _maxPos = _road.position.z + (_road.localScale.x / 2 - 0.1f);
            _minPos = _road.position.z + (0.1f - _road.localScale.x / 2);
        }
    }

    private void Update()
    {
        if (!Character.Instance.IsRotating)
        {
            _swerveAmount = Time.deltaTime * swerveSpeed * _swerveInputSystem.MoveFactorX;
            _swerveAmount = Mathf.Clamp(_swerveAmount, -maxSwerveAmount, maxSwerveAmount);
            transform.Translate(_swerveAmount, 0f, 0f);
            _direction = Character.Instance.Direction;
            CalculateEdges(_direction);
        }

        float clampedPos = Mathf.Clamp(transform.position.x, _minPos, _maxPos);

        if (_isOnWay)
        {
            switch (_direction)
            {
                case 0:
                    clampedPos = Mathf.Clamp(transform.position.x, _minPos, _maxPos);
                    transform.position = new Vector3(clampedPos, transform.position.y, transform.position.z);
                    break;
                case 1:
                    clampedPos = Mathf.Clamp(transform.position.z, _minPos, _maxPos);
                    transform.position = new Vector3(transform.position.x, transform.position.y, clampedPos);
                    break;
                case 2:
                    clampedPos = Mathf.Clamp(transform.position.x, _minPos, _maxPos);
                    transform.position = new Vector3(clampedPos, transform.position.y, transform.position.z);
                    break;
                case 3:
                    clampedPos = Mathf.Clamp(transform.position.z, _minPos, _maxPos);
                    transform.position = new Vector3(transform.position.x, transform.position.y, clampedPos);
                    break;
                default:
                    break;
            }
        }
    }
}
