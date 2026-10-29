using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/*

public class SystemWOPR : MonoBehaviour
{
   [SerializeField] private CellState playerPiece = CellState.X;
   
   
   
   private void Update()
   {
      if (GameManager.Instance.IsGameOver)
      {
         return;
      } 
      
      if (GameManager.Instance.CurrentTurn == playerPiece)
      {
         Move(playerPiece);

      } 
   }
   

   public void Move(CellState piece)
   {
      // Buscar jugadas ganadoras
      CellState[,] BoardIA = GameManager.Instance.Board.Clone() as CellState[,]; 
      int i = 0;
      foreach (CellState cellState in BoardIA)
      {
         if (TryMove(BoardIA, i / 3, i % 3, piece))
         {
            
            GameManager.Instance.PlayMove(i / 3, i % 3, valor);
            return;
         }
         i++;
      }
      // Evitar jugadas ganadoras del rival
      CellState human = (piece == CellState.O ? CellState.X : CellState.O);
      i = 0;
      foreach (CellState cellState in BoardIA)
      {
         if (TryMove(BoardIA, i / 3, i % 3, human))
         {
            GameManager.Instance.PlayMove(i / 3, i % 3);
            return;
         }
         i++;
      }
      // Casilla central
      if (GameManager.Instance.Board[1, 1] == CellState.Empty)
      {
         GameManager.Instance.PlayMove(1, 1);
         return;
      }
      // Casillas de esquina
      (int r, int c)[] rc = new (int, int)[] { (0, 0), (0, 2), (2, 0), (2, 2) };
      List<(int, int)> celdas = new List<(int, int)>(rc);
      for (int j = 0; j < celdas.Count; j++)
      {
         // aleatorio entre 0 y la longitud del array de celdas
         int r = Random.Range(0, celdas.Count);
         if (GameManager.Instance.Board[celdas[r].Item1, celdas[r].Item2] == CellState.Empty)
         {
            GameManager.Instance.PlayMove(celdas[r].Item1, celdas[r].Item2);
            return;
         }
         // elimina la celda ocupa de la lista de posibles
         celdas.RemoveAt(r);
      }
      // Resto de casillas
      rc = new (int, int)[] { (0, 1), (1, 0), (1, 2), (2, 1) };
      celdas = new List<(int, int)>(rc);
      for (int j = 0; j < celdas.Count; j++)
      {
         // aleatorio entre 0 y la longitud del array de celdas
         int r = Random.Range(0, celdas.Count);
         if (GameManager.Instance.Board[celdas[r].Item1, celdas[r].Item2] == CellState.Empty)
         {
            GameManager.Instance.PlayMove(celdas[r].Item1, celdas[r].Item2);
            return;
         }
         // elimina la celda ocupa de la lista de posibles
         celdas.RemoveAt(r);
      }
   }

   private bool TryMove(CellState[,] board, int r, int c, CellState piece)
   {
      // simulo la colocación de la pieza en r, c, si la casilla está libre
      if (board[r, c] == CellState.Empty)
      {
         board[r, c] = piece;
         // comprobar si es jugada ganadora
         bool resultado = CheckWin(board, r, c, piece);
         // vuelta atrás de la jugada
         board[r, c] = CellState.Empty;
         return resultado;
      }

      return false;
   }
   
   private bool CheckWin(CellState[,] board, int row, int col, CellState piece)
   {
      // Comprobar la fila en la que estoy
      if (board[row, 0] == piece && board[row, 1] == piece && board[row, 2] == piece)
         return true;
        
      // Comprobar la columna en la que estoy
      if (board[0, col] == piece && board[1, col] == piece && board[2, col] == piece)
         return true;
        
      // Comprobar la diagonal principal
      if (board[0, 0] == piece && board[1, 1] == piece && board[2, 2] == piece)
         return true;
        
      // Comprobar la diagonal secundaria
      if (board[0, 2] == piece && board[1, 1] == piece && board[2, 0] == piece)
         return true;
        
      return false;
   }
}*/
