using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using Chess.Scripts.Core;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public sealed class ChessBoardPlacementHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _rowsArray;
    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] private GameObject _highlightEnemyPrefab;
    private GameObject[,] _chessBoard;

    internal static ChessBoardPlacementHandler Instance;

    private void Awake()
    {
        Instance = this;
        GenerateArray();
    }

    private void GenerateArray()
    {
        _chessBoard = new GameObject[8, 8];
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                _chessBoard[i, j] = _rowsArray[i].transform.GetChild(j).gameObject;
            }
        }
    }

    internal GameObject GetTile(int row, int col)
    {
        try
        {
            return _chessBoard[row, col];
        }
        catch (Exception)
        {
            Debug.LogError("Invalid row or column.");
            return null;
        }
    }

    internal void Highlight(int row, int col)
    {
        var tile = GetTile(row, col).transform;
        if (tile == null)
        {
            Debug.LogError("Invalid row or column.");
            return;
        }

        Instantiate(_highlightPrefab, tile.transform.position, Quaternion.identity, tile.transform);
    }

    internal void ClearHighlights()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var tile = GetTile(i, j);
                if (tile.transform.childCount <= 0) continue;
                foreach (Transform childTransform in tile.transform)
                {
                    Destroy(childTransform.gameObject);
                }
            }
        }
    }

    internal bool IsTileEmpty(int row, int col)
    {
        var tile = GetTile(row, col);
        if (tile == null)
        {
            Debug.LogError("Invalid row or column.");
            return false;
        }

        return tile.transform.childCount <= 0;
    }

    internal bool IsTileOccupiedByOpponentAndMine(int row, int col, string myTag)
    {
        var tile = GetTile(row, col);
        if (tile == null)
        {
            Debug.LogError("Invalid row or column.");
            return false;
        }

        // Get the children of the Player position object and getting all the childs
        Transform childObjectParent = GameObject.Find("Player Positions").transform;
        foreach (Transform childTransform in childObjectParent.GetComponentsInChildren<Transform>())
        {

            var placementH = childTransform.GetComponent<ChessPlayerPlacementHandler>();
            
            if(myTag == "Black")
            {
                if (childTransform.CompareTag("Black") && placementH.row == row && placementH.column == col)
                {
                    return true;
                }
                else if (childTransform.CompareTag("White") && placementH.row == row && placementH.column == col)
                {
                    Instantiate(_highlightEnemyPrefab, tile.transform.position, Quaternion.identity, tile.transform);
                    return true;
                }
            }

            else
            {
                if (childTransform.CompareTag("White") && placementH.row == row && placementH.column == col)
                {
                    return true;
                }
                else if (childTransform.CompareTag("Black") && placementH.row == row && placementH.column == col)
                {
                    Instantiate(_highlightEnemyPrefab, tile.transform.position, Quaternion.identity, tile.transform);
                    return true;
                }
            }
           
        }

        return false; 
    }

}

