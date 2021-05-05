using System.Collections.Generic;
using UnityEngine;

public class PetriDish : MonoBehaviour
{
    public Cell cellPrefab;

    public int generation = 0;

    public float secTillNextUpdate = 2;

    public int cellSize;
    public int width;
    public int height;

    public List<Cell> aliveCells = new List<Cell>();

    public Dictionary<Vector2, Cell> cellByIndex = new Dictionary<Vector2, Cell>();

    // Start is called before the first frame update
    void Start()
    {
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector3(column * cellSize, row * cellSize, 1), Quaternion.identity, transform);

                cellByIndex.Add(new Vector2(column, row), cell);

                cell.transform.parent = transform;
            }
        }

        StartCoroutine(UpdateCells());
    }

    IEnumerator<WaitForSeconds> UpdateCells()
    {
        while (true)
        {
            yield return new WaitForSeconds(secTillNextUpdate);
            if (!InputController.isPaused)
            {
                aliveCells = new List<Cell>(GetUpdatedAliveCells());
                generation++;
            }
        }
    }

    List<Cell> GetUpdatedAliveCells()
    {
        List<Cell> updatedAliveCells = new List<Cell>();
        List<Cell> evaluatedCells = new List<Cell>();

        foreach (Cell cell in aliveCells)
        {
            EvaluateCellState(cell, evaluatedCells, updatedAliveCells);
        }

        return updatedAliveCells;
    }

    void EvaluateCellState(Cell cell, List<Cell> evaluatedCells, List<Cell> updatedAliveCells, int currDepth = 0)
    {
        List<Cell> neighbors = new List<Cell>(GetNeighbors(cell));

        int depth = 1;
        if (currDepth < depth)
        {
            foreach(Cell neighbor in neighbors)
            {
                EvaluateCellState(neighbor, evaluatedCells, updatedAliveCells, currDepth + 1);
            }
        }

        if (!evaluatedCells.Contains(cell))
        {
            int aliveNeighborsCount = GetAliveNeighorsCount(neighbors);
            if (aliveCells.Contains(cell) && (aliveNeighborsCount < 2 || aliveNeighborsCount > 3))
            {
                cell.SetState(false);

                print($"Generation {generation} cell of {cell.index} has died, population: {aliveNeighborsCount}");
            }
            else if (aliveNeighborsCount == 3 || (aliveNeighborsCount == 2 && aliveCells.Contains(cell)))
            {
                cell.SetState(true);
                updatedAliveCells.Add(cell);

                if (aliveNeighborsCount == 3)
                {
                    print($"Generation {generation} cell of {cell.index} was born");
                }
            }
        }

        evaluatedCells.Add(cell);
    }

    List<Cell> GetNeighbors(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();

        for (int yOffset = -1; yOffset < 2; yOffset++)
        {
            for (int xOffset = -1; xOffset < 2; xOffset++)
            {
                if (!(xOffset == 0 && yOffset == 0))
                {
                    if (cellByIndex.TryGetValue(new Vector2(cell.index.x + xOffset, cell.index.y + yOffset), out Cell neighbor))
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }
        }

        return neighbors;
    }

    int GetAliveNeighorsCount(List<Cell> neighbors)
    {
        int aliveNeighbors = 0;

        foreach (Cell neighbor in neighbors)
        {
            if (aliveCells.Contains(neighbor))
            {
                aliveNeighbors++;
            }
        }

        return aliveNeighbors;
    }
}
