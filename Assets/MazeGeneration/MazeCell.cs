using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _unvisitedBlock;

    public Transform _wayPoint;

    public bool IsVisited { get; private set; }
    public bool IsLeftCleared { get; set; }
    public bool IsRightCleared { get; set; }
    public bool IsFrontCleared { get; set; }
    public bool IsBackCleared { get; set; }

    public void Visit()
    {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall()
    {
        IsLeftCleared = true;
        _leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        IsRightCleared = true;
        _rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        IsFrontCleared = true;
        _frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        IsBackCleared = true;
        _backWall.SetActive(false);
    }
}