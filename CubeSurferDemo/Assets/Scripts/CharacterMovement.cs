using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float _rotationProgress = -1f;
    private Quaternion _startRotation;
    private Quaternion _endRotation;
    private Camera _mainCam;
    private Rigidbody _rigidBody;
    private float _speed;

    private void Start()
    {
        _mainCam = Camera.main;
        _rigidBody = GetComponent<Rigidbody>();
        _speed = Character.Instance.Speed;
    }

    void Update()
    {
        if (_rotationProgress < 1 && _rotationProgress >= 0)
        {
            _rotationProgress += Time.deltaTime;

            transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, _rotationProgress);
            _mainCam.transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, _rotationProgress);

            if (Quaternion.Angle(_startRotation, _endRotation) > 1f)
                Character.Instance.IsRotating = true;
            else
                Character.Instance.IsRotating = false;
        }

        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightCurve")
        {
            Debug.Log("RightCurve");
            other.GetComponent<BoxCollider>().enabled = false;
            Character.Instance.Direction += 1;
            StartRotating(90);
        }
        else if (other.tag == "LeftCurve")
        {
            Debug.Log("LeftCurve");
            other.GetComponent<BoxCollider>().enabled = false;
            Character.Instance.Direction -= 1;
            StartRotating(-90);
        }
    }

    private void StartRotating(float rotateAmount)
    {
        _startRotation = transform.rotation;
        _endRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + rotateAmount, transform.rotation.eulerAngles.z);
        _rotationProgress = 0;
    }
}
