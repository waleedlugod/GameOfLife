using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriDish : MonoBehaviour
{
    public Cell cellPrefab;

    public bool isPaused;

    public int generation = 0;

    public float secsTillNextGeneration;

    public int cellSize = 0;

    public int width, height;

    public int totalCellCount;

    public List<Cell> population = new List<Cell>();

    private double nextActionTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = true;

        DetermineHeightAndWidth();

        PositionPetriDish();

        GenerateCells();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
            Debug.Log($"isPaused changed to {isPaused}");
        }

        if (Time.time > nextActionTime && !isPaused)
        {
            nextActionTime = Time.time + secsTillNextGeneration;
            UpdateCells();
        }
    }

    void DetermineHeightAndWidth()
    {
        height = Mathf.CeilToInt((Camera.main.orthographicSize * 2 / cellSize));
        width = Mathf.CeilToInt((Camera.main.orthographicSize * Camera.main.aspect * 2 / cellSize));
    }

    void PositionPetriDish()
    {
        // Position to bottom left of camera
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        // Offset to completelyshow bottom left cell
        transform.position = new Vector2(transform.position.x + (cellSize / 2), transform.position.y + (cellSize / 2));
    }

    void GenerateCells()
    {
        int index = 0;
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                Cell cell = Instantiate(cellPrefab, transform, false);
                cell.transform.localPosition = new Vector2(column, row) * cellSize;
                cell.index = index;
                index++;
            }
        }
    }

    void UpdateCells()
    {
        if (!isPaused)
        {
            population = new List<Cell>(GetNextPopulation(population));
            generation++;
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    List<Cell> GetNextPopulation(List<Cell> currPopulation)
    {
        List<Cell> nextPopulation = new List<Cell>();
        List<Cell> evaluatedCells = new List<Cell>();

        foreach (Cell cell in currPopulation)
        {
            GetNextPopulation(cell, currPopulation, nextPopulation, evaluatedCells, 0);
        }

        return nextPopulation;
    }

    void GetNextPopulation(Cell cell, List<Cell> currPopulation, List<Cell> nextPopulation, List<Cell> evaluatedCells, int currDepth)
    {
        List<Cell> neighbors = new List<Cell>(GetNeighbors(cell));

        // Evaluate states of neighbors
        int depth = 1;
        if (currDepth < depth)
        {
            foreach(Cell neighbor in neighbors)
            {
                GetNextPopulation(neighbor, currPopulation, nextPopulation, evaluatedCells, currDepth + 1);
            }
        }

        if (!evaluatedCells.Contains(cell))
        {
            // Count alive neighbors
            int aliveNeighborsCount = 0;
            foreach(Cell neighbor in neighbors)
            {
                if (currPopulation.Contains(neighbor))
                {
                    aliveNeighborsCount++;
                }
            }

            // If Cell is alive in previous generation.
            if (currPopulation.Contains(cell) &&
                // If underpopulated or overpopulated.
               (aliveNeighborsCount < 2 || aliveNeighborsCount > 3))
            {
                // Cell is dead.
                cell.SetState(false);

                Debug.Log($"Generation {generation} cell of {cell.index} has died, population: {aliveNeighborsCount}");
            }
            // If cell will be born.
            else if (aliveNeighborsCount == 3 ||
                    // If living cell in previous population will survive.
                    (aliveNeighborsCount == 2 && currPopulation.Contains(cell)))
            {
                // Cell is alive.
                cell.SetState(true);
                nextPopulation.Add(cell);

                if (aliveNeighborsCount == 3)
                {
                    Debug.Log($"Generation {generation} cell of {cell.index} was born");
                }
            }
        }

        evaluatedCells.Add(cell);
    }

    List<Cell> GetNeighbors(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();

        // NOTE: Decided no not use Dictionary to increase performance
        for (int yOffset = -1; yOffset < 2; yOffset++)
        {
            for (int xOffset = -1; xOffset < 2; xOffset++)
            {
                if (!(yOffset == 0 && xOffset == 0))
                {
                    try
                    {
                        Cell neighbor = transform.GetChild((cell.index + width * yOffset) + xOffset).GetComponent<Cell>();
                        neighbors.Add(neighbor);
                    }
                    catch
                    {
                        // neighbor is out of bounds.
                    }
                }
            }
        }

        return neighbors;
    }
}
