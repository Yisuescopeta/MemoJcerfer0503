using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
// código del rival del tres en raya que está desactivado
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
      // busca una jugada para ganar
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
      // busca una jugada para bloquear al rival
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
      // intenta jugar en el centro
      if (GameManager.Instance.Board[1, 1] == CellState.Empty)
      {
         GameManager.Instance.PlayMove(1, 1);
         return;
      }
      // intenta jugar en una esquina
      (int r, int c)[] rc = new (int, int)[] { (0, 0), (0, 2), (2, 0), (2, 2) };
      List<(int, int)> celdas = new List<(int, int)>(rc);
      for (int j = 0; j < celdas.Count; j++)
      {
         // elige una casilla al azar
         int r = Random.Range(0, celdas.Count);
         if (GameManager.Instance.Board[celdas[r].Item1, celdas[r].Item2] == CellState.Empty)
         {
            GameManager.Instance.PlayMove(celdas[r].Item1, celdas[r].Item2);
            return;
         }
         // quita la casilla ocupada de la lista
         celdas.RemoveAt(r);
      }
      // prueba las casillas que quedan
      rc = new (int, int)[] { (0, 1), (1, 0), (1, 2), (2, 1) };
      celdas = new List<(int, int)>(rc);
      for (int j = 0; j < celdas.Count; j++)
      {
         // elige una casilla al azar
         int r = Random.Range(0, celdas.Count);
         if (GameManager.Instance.Board[celdas[r].Item1, celdas[r].Item2] == CellState.Empty)
         {
            GameManager.Instance.PlayMove(celdas[r].Item1, celdas[r].Item2);
            return;
         }
         // quita la casilla ocupada de la lista
         celdas.RemoveAt(r);
      }
   }

   private bool TryMove(CellState[,] board, int r, int c, CellState piece)
   {
      // prueba la jugada si la casilla está libre
      if (board[r, c] == CellState.Empty)
      {
         board[r, c] = piece;
         // comprueba si gana con esa jugada
         bool resultado = CheckWin(board, r, c, piece);
         // deja la casilla como estaba
         board[r, c] = CellState.Empty;
         return resultado;
      }

      return false;
   }
   
   private bool CheckWin(CellState[,] board, int row, int col, CellState piece)
   {
      // comprueba la fila
      if (board[row, 0] == piece && board[row, 1] == piece && board[row, 2] == piece)
         return true;
        
      // comprueba la columna
      if (board[0, col] == piece && board[1, col] == piece && board[2, col] == piece)
         return true;
        
      // comprueba la diagonal principal
      if (board[0, 0] == piece && board[1, 1] == piece && board[2, 2] == piece)
         return true;
        
      // comprueba la diagonal secundaria
      if (board[0, 2] == piece && board[1, 1] == piece && board[2, 0] == piece)
         return true;
        
      return false;
   }
}*/
