using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public PetriDish petriDish;

    public Vector2 index;

    public bool isAlive = false;

    public float size;

    void Start()
    {
        GetComponent<Renderer>().enabled = false;

        petriDish = transform.parent.GetComponent<PetriDish>();

        size = petriDish.cellSize;

        transform.localScale = new Vector2(1, 1) * size;
    }

    void OnMouseDown()
    {
        SetState(!isAlive);
        if (isAlive)
        {
            petriDish.population.Add(index, this);
            Debug.Log($"Cell of {index} was added.");
        }
        else
        {
            petriDish.population.Remove(index);
            Debug.Log($"Cell of {index} was removed.");
        }
    }

    public void SetState(bool state)
    {
        isAlive = state;
        if (isAlive)
        {
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
