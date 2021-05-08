using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public PetriDish petriDish;

    public int index;

    public bool isAlive;

    public float size;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        petriDish = transform.parent.GetComponent<PetriDish>();

        size = petriDish.cellSize;

        transform.localScale = new Vector2(1, 1) * size;

        isAlive = false;
        sprite.color = Color.black;
    }

    void OnMouseDown()
    {
        SetState(!isAlive);
        if (isAlive)
        {
            petriDish.population.Add(this);
        }
        else
        {
            petriDish.population.Remove(this);
        }
    }

    public void SetState(bool state)
    {
        isAlive = state;
        sprite.color = isAlive ? Color.white : Color.black;
    }
}
