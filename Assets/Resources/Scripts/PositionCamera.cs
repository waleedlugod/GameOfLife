using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCamera : MonoBehaviour
{
    void Start()
    {
        float offset = Cell.size / 2;

        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));
        transform.position = new Vector3(transform.position.x - offset, transform.position.y - offset, transform.position.z) ;
    }
}
