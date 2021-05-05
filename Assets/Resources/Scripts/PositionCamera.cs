using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCamera : MonoBehaviour
{
    public PetriDish petriDish;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        float offset = petriDish.cellSize / 2;

        transform.position = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        transform.position = new Vector3(transform.position.x - offset, transform.position.y - offset, transform.position.z) ;
    }
}
