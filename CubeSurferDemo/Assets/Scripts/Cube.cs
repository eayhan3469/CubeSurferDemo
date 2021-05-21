using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ObstacleCube")
        {
            transform.parent = other.transform;
            Character.Instance.RemoveCube(this);
        }
    }
}
