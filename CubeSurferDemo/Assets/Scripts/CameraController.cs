using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform Target;

    public float SmoothFactor;

    private Vector3 cameraOffset;

    private void Start()
    {
        cameraOffset = transform.position - Target.position;
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            if (4 > 1) //TODO: karakter yüksekliği ayarlanacak
            {
                Vector3 newPosition = (Target.position + cameraOffset) - new Vector3(0f, 0f, 0f);// *;/*+ (Target.localScale * 0.2f)*/; //TODO: topuk sayısına göre uzaklık değişecek
                transform.position = Vector3.Slerp(transform.position, newPosition, SmoothFactor);
            }
            else
            {
                Vector3 newPosition = (Target.position + cameraOffset);// *;/*+ (Target.localScale * 0.2f)*/; //TODO: topuk sayısına göre uzaklık değişecek
                transform.position = Vector3.Slerp(transform.position, newPosition, SmoothFactor);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Target.rotation, SmoothFactor * Time.deltaTime);
        }
    }
}
