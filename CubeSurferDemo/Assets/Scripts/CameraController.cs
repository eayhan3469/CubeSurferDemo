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

    private bool _hasZoomOut;

    private void LateUpdate()
    {
        if (!_hasZoomOut && Character.Instance.Cubes.Count % 4 == 0) //Recalculate Camera distance from Character based on cube count
        {
            camTarget.localPosition += camTarget.localPosition.normalized * 5f;
            _hasZoomOut = true;
        }
        else if (Character.Instance.Cubes.Count % 4 != 0)
        {
            _hasZoomOut = false;
        }

        Vector3 dPos = camTarget.position;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, smoothSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position);
    }
}
