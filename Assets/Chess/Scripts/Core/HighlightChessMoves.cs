using System;
using UnityEngine;
using Chess.Scripts.Core;

public class HighlightChessMoves : MonoBehaviour
{
    // Enum defining types of chess pieces
    public enum PieceType
    {
        King,
        Queen,
        Bishop,
        Knight,
        Rook,
        Pawn
    }

    // Type of the chess piece
    [SerializeField] private PieceType _type;

    // Tag to identify the player (e.g., "White" or "Black")
    [SerializeField] private string _tag;

    // Called when the player clicks on the chess piece
    private void OnMouseDown()
    {
        // Clear any existing highlights on the chessboard
        ChessBoardPlacementHandler.Instance.ClearHighlights();

        // Highlight possible moves for the selected piece
        HighlightPossibleMoves();
    }

    // Checks if a tile is invalid (out of bounds or occupied by an opponent)
    private bool IsInvalidTile(int row, int col)
    {
        return !IsValidTile(row, col) || ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponentAndMine(row, col, _tag);
    }

    // Checks if a tile is within the valid bounds of the chessboard
    private bool IsValidTile(int row, int col)
    {
        return row >= 0 && row <= 7 && col >= 0 && col <= 7;
    }

    // Highlights possible moves based on the type of chess piece
    private void HighlightPossibleMoves()
    {
        switch (_type)
        {
            case PieceType.King:
                HighlightKingMovement();
                break;
            case PieceType.Queen:
                HighlightQueenMovement();
                break;
            case PieceType.Bishop:
                HighlightBishopMovement();
                break;
            case PieceType.Knight:
                HighlightKnightMovement();
                break;
            case PieceType.Rook:
                HighlightRookMovement();
                break;
            case PieceType.Pawn:
                HighlightPawnMovement();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Highlights possible moves for the King piece
    private void HighlightKingMovement()
    {
        // Getting the row and column values for the king's tile using the ChessPlayerPlacementHandler script
        int row = GetComponent<ChessPlayerPlacementHandler>().row;
        int col = GetComponent<ChessPlayerPlacementHandler>().column;

        // King's possible movements
        MovementofKing(row - 1, col - 1);
        MovementofKing(row - 1, col);
        MovementofKing(row - 1, col + 1);
        MovementofKing(row, col - 1);
        MovementofKing(row, col + 1);
        MovementofKing(row + 1, col - 1);
        MovementofKing(row + 1, col);
        MovementofKing(row + 1, col + 1);
    }

    // Highlights a specific move for the King piece
    private void MovementofKing(int row, int col)
    {
        // Check if the tile is invalid or occupied by an opponent
        if (IsInvalidTile(row, col)) return;

        // Check if the tile is occupied by an opponent, and highlight if not
        if (ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponentAndMine(row, col, _tag)) return;

        // Highlight the tile
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    // Highlights possible moves for the Queen piece
    private void HighlightQueenMovement()
    {
        // Combine movements of Bishop and Rook for the Queen
        HighlightBishopMovement();
        HighlightRookMovement();
    }

    // Highlights possible moves for the Bishop piece
    private void HighlightBishopMovement()
    {
        var chessPlayerPlacementHandler = GetComponent<ChessPlayerPlacementHandler>();
        var bishopCol = chessPlayerPlacementHandler.column;
        var bishopRow = chessPlayerPlacementHandler.row;

        ChessBoardPlacementHandler.Instance.GetTile(bishopRow, bishopCol);

        var row = bishopRow;
        var col = bishopCol;

        MovementsInDirection(row, col, -1, -1);
        MovementsInDirection(row, col, -1, 1);
        MovementsInDirection(row, col, 1, -1);
        MovementsInDirection(row, col, 1, 1);
    }

    // Highlights possible moves in a specified direction for a piece (used by Bishop and Queen)
    private void MovementsInDirection(int row, int col, int rowDirection, int colDirection)
    {
        var curRow = row + rowDirection;
        var curCol = col + colDirection;
        while (IsValidTile(curRow, curCol))
        {
            if (ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponentAndMine(curRow, curCol, _tag))
            {
                return;
            }
            ChessBoardPlacementHandler.Instance.Highlight(curRow, curCol);
            curRow += rowDirection;
            curCol += colDirection;
        }
    }

    // Highlights possible moves for the Knight piece
    private void HighlightKnightMovement()
    {
        var chessPlayerPlacementHandler = GetComponent<ChessPlayerPlacementHandler>();
        var knightCol = chessPlayerPlacementHandler.column;
        var knightRow = chessPlayerPlacementHandler.row;

        ChessBoardPlacementHandler.Instance.GetTile(knightRow, knightCol);

        MovementOfKnight(knightRow + 2, knightCol + 1);
        MovementOfKnight(knightRow + 2, knightCol - 1);
        MovementOfKnight(knightRow - 2, knightCol + 1);
        MovementOfKnight(knightRow - 2, knightCol - 1);
        MovementOfKnight(knightRow + 1, knightCol + 2);
        MovementOfKnight(knightRow + 1, knightCol - 2);
        MovementOfKnight(knightRow - 1, knightCol + 2);
        MovementOfKnight(knightRow - 1, knightCol - 2);
    }

    // Highlights a specific move for the Knight piece
    private void MovementOfKnight(int row, int col)
    {
        if (IsValidTile(row, col))
        {
            if (!ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponentAndMine(row, col, _tag))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row, col);
            }
        }
    }

    // Highlights possible moves for the Rook piece
    private void HighlightRookMovement()
    {
        // Getting the row and column values for the rook's tile using the ChessPlayerPlacementHandler script
        int row = GetComponent<ChessPlayerPlacementHandler>().row;
        int col = GetComponent<ChessPlayerPlacementHandler>().column;

        // Highlight the rook's possible moves
        MovementsInDirection(row, col, -1, 0);
        MovementsInDirection(row, col, 1, 0);
        MovementsInDirection(row, col, 0, -1);
        MovementsInDirection(row, col, 0, 1);
    }

    // Highlights possible moves for the Pawn piece
    private void HighlightPawnMovement()
    {
        // Getting the row and column values for the pawn's tile using the ChessPlayerPlacementHandler script
        int row = GetComponent<ChessPlayerPlacementHandler>().row;
        int col = GetComponent<ChessPlayerPlacementHandler>().column;

        if (_tag == "White")
        {
            MovementOfWhitePawn(row - 1, col);
            MovementOfWhitePawnAttack(row - 1, col - 1);
            MovementOfWhitePawnAttack(row - 1, col + 1);
            if (row == 6) MovementOfWhitePawn(row - 2, col);
        }

        else if (_tag == "Black")
        {
            MovementOfBlackPawn(row + 1, col);
            MovementOfBlackPawnAttack(row + 1, col - 1);
            MovementOfBlackPawnAttack(row + 1, col + 1);
            if (row == 1) MovementOfBlackPawn(row + 2, col);
        }
    }

    // Highlights a move for a White Pawn
    private void MovementOfWhitePawn(int row, int col)
    {
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileEmpty(row, col)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    // Highlights a move for a Black Pawn
    private void MovementOfBlackPawn(int row, int col)
    {
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileEmpty(row, col)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    // Highlights an attack move for a White Pawn
    private void MovementOfWhitePawnAttack(int row, int col)
    {
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponentAndMine(row, col, _tag)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    // Highlights an attack move for a Black Pawn
    private void MovementOfBlackPawnAttack(int row, int col)
    {
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponentAndMine(row, col, _tag)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }
}
