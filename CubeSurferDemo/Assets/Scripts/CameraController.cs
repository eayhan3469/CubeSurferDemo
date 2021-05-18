using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform camTarget;

    [SerializeField]
    private Transform lookTarget;

    [SerializeField]
    private float smoothSpeed = 10f;

    [SerializeField]
    private Vector3 distance;

    private void LateUpdate()
    {
        Vector3 dPos = camTarget.position + distance;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, smoothSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position);
    }
}
