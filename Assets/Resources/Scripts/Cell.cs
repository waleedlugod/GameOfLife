using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public PetriDish petriDish;

    public Vector2 index;

    private bool isAlive;

    private static float size;

    private SpriteRenderer sprite;

    public Cell(Vector2 index)
    {
        this.index = index;
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        petriDish = transform.parent.GetComponent<PetriDish>();

        size = petriDish.cellSize;
        index = new Vector2(transform.position.x / size, transform.position.y / size);
        transform.localScale = new Vector2(size, size);

        isAlive = false;
        sprite.color = Color.black;
    }

    void OnMouseDown()
    {
        SetState(!isAlive);
        if (isAlive)
        {
            petriDish.aliveCells.Add(this);
        }
        else
        {
            petriDish.aliveCells.Remove(this);
        }
    }

    public void SetState(bool state)
    {
        isAlive = state;
        sprite.color = isAlive ? Color.white : Color.black;
    }
}
