using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ObstacleCube" || other.tag == "Altitude")
        {
            transform.parent = other.transform;
            Character.Instance.RemoveCube(this);

            if (other.tag == "Altitude")
            {
                GameManager.Instance.PointMultiplier++;
            }
        }
    }
}
