using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriDish : MonoBehaviour
{
    public Cell cellPrefab;

    public int cellsInRow = 10;

    // Start is called before the first frame update
    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.pixelWidth / Camera.main.pixelHeight;

        Cell.size = cameraWidth / cellsInRow;

        for (int row = 0; row < cameraHeight / Cell.size; row++)
        {
            for (int column = 0; column < cellsInRow; column++)
            {
                 var cell = Instantiate(cellPrefab, new Vector3(column, row, 1), Quaternion.identity);
                cell.transform.parent = GameObject.Find("CellParent").transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
