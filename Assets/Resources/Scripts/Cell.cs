using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static float size = 40;

    private bool alive = false;

    private SpriteRenderer sprite; 

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        transform.localScale = new Vector2(size, size);
        sprite.color = Color.black;
    }

    void OnMouseDown()
    {
        ChangeState();
    }

    void ChangeState()
    {
        alive = !alive;
        sprite.color = alive ? Color.white : Color.black;
    }
}
