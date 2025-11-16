using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private Transform _mazeRoot;

    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private MazeCell _mazeExitPrefab;
    [SerializeField] private int _mazeScale;

    [SerializeField] private int _mazeWidth;
    [SerializeField] private int _mazeDepth;

    
    private int[] _centreIndex;
    private MazeCell[,] _mazeGrid;

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];
        _centreIndex = new int[] { _mazeWidth / 2, _mazeDepth / 2 };

        int totalCells = _mazeWidth * _mazeDepth;
        EnemyManager.Instance.WayPoints = new Transform[totalCells];

        int i = 0;
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {   
                if(x!=_centreIndex[0] || z != _centreIndex[1])
                    _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x*_mazeScale, 0, z*_mazeScale), Quaternion.identity, _mazeRoot);
                else
                    _mazeGrid[x, z] = Instantiate(_mazeExitPrefab, new Vector3(x*_mazeScale, 0, z*_mazeScale), Quaternion.identity, _mazeRoot);

                EnemyManager.Instance.WayPoints[i++] = _mazeGrid[x,z]._wayPoint;
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);

        _mazeGrid[_centreIndex[0] - 1,_centreIndex[1]].ClearRightWall();
        _mazeGrid[_centreIndex[0] + 1,_centreIndex[1]].ClearLeftWall();
        _mazeGrid[_centreIndex[0],_centreIndex[1] - 1].ClearFrontWall();
        _mazeGrid[_centreIndex[0],_centreIndex[1] + 1].ClearBackWall();

        Debug.Log("Maze Generated.");
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = Mathf.RoundToInt(currentCell.transform.position.x / _mazeScale);
        int z = Mathf.RoundToInt(currentCell.transform.position.z / _mazeScale);

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            
            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

}