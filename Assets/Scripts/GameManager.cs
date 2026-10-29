using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum CellState { Empty, X, O }


public class GameManager : MonoBehaviour

{
    
    
    [SerializeField] private GameObject cellsContainer;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField, Min(2)] private int numeroCartas = 6;
    public CellState[,] Board { get; private set; } = new CellState[2, 3];
    
    public int PuntuacionJugador1 { get; private set; } = 0;
    
    public int PuntuacionJugador2 { get; private set; } = 0;
    public static int ultimoValor { get; private set; } = 0;
    
    public CellState CurrentTurn { get; private set; } = CellState.X;
    private bool CambioTurno { get; set; } = true;
    private bool comprobandoPareja;
    private BoardCell[] cells;
    
    public bool IsGameOver { get; private set; }
    public CellState Winner { get; private set; } = CellState.Empty;
    
    public int row1, col1, row2, col2;
    
    
    
    // Singleton
    
    public static GameManager Instance { get; private set;  }

    public void Awake(){
        
        // verificar si la instancia existe :)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // y si existe una istancia previa, eliminar este objeto
            Destroy(gameObject);
        }
    }

    private void ResetImages()
    {
        if (cellsContainer && cellPrefab)
        {
            if (cells == null || cells.Length != numeroCartas)
            {
                if (cells != null)
                {
                    foreach (BoardCell previousCell in cells)
                    {
                        previousCell.gameObject.SetActive(false);
                        Destroy(previousCell.gameObject);
                    }
                }
                cells = new BoardCell[numeroCartas];
                for (int i = 0; i < cells.Length; i++)
                {
                    GameObject cell = Instantiate(cellPrefab, cellsContainer.transform);
                    cells[i] = cell.GetComponent<BoardCell>();
                    cells[i].row = i / Board.GetLength(1);
                    cells[i].col = i % Board.GetLength(1);
                }
            }

            int[] valores = new int[numeroCartas];
            for (int i = 0; i < valores.Length; i++)
                valores[i] = i / 2;
            for (int i = valores.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                int temporal = valores[i];
                valores[i] = valores[j];
                valores[j] = temporal;
            }
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].valor = valores[i];
                cells[i].Refresh();
            }
            AjustarTablero();
        }
    }

    private void AjustarTablero()
    {
        GridLayoutGroup grid = cellsContainer.GetComponent<GridLayoutGroup>();
        if (grid == null) return;

        Canvas.ForceUpdateCanvases();
        Rect area = ((RectTransform)cellsContainer.transform).rect;
        int filas = Board.GetLength(0);
        int columnas = Board.GetLength(1);
        float ancho = (area.width - grid.padding.horizontal - grid.spacing.x * (columnas - 1)) / columnas;
        float alto = (area.height - grid.padding.vertical - grid.spacing.y * (filas - 1)) / filas;
        float lado = Mathf.Max(1f, Mathf.Min(ancho, alto));
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columnas;
        grid.cellSize = new Vector2(lado, lado);
    }

    private void OnValidate()
    {
        numeroCartas = Mathf.Max(2, numeroCartas);
        if (numeroCartas % 2 != 0) numeroCartas++;
    }

    // Intenta realizar una jugada en la posición (row, col)
    public bool PlayMove(int row, int col, int valor)
    {
        
        // comprobar si el juego terminó o la casilla ya está ocupada
        if (IsGameOver || comprobandoPareja ||
            row < 0 || row >= Board.GetLength(0) ||
            col < 0 || col >= Board.GetLength(1) ||
            Board[row, col] != CellState.Empty)
            return false;

        // Registrar la jugada
        Board[row, col] = CurrentTurn;
        
        row2 = row1;
        col2 = col1;
        row1 = row;
        col1 = col;
        
        
        if (CambioTurno)
        {
            ultimoValor = valor;
            CambioTurno = false;
        }
        else if (ultimoValor != valor)
        {
            comprobandoPareja = true;
            StartCoroutine(OcultarPareja(valor));
        }
        else
        {
            CheckPoint(ultimoValor, valor);
            CambioTurno = true;
            if (CheckWin(row, col))
            {
                IsGameOver = true;
                Winner = PuntuacionJugador1 == PuntuacionJugador2 ? CellState.Empty :
                    (PuntuacionJugador1 > PuntuacionJugador2 ? CellState.X : CellState.O);
            }
        }

        return true;
    }

    public void StartNewGame()
    {
        StopAllCoroutines();
        OnValidate();
        int filas = Mathf.FloorToInt(Mathf.Sqrt(numeroCartas));
        while (numeroCartas % filas != 0) filas--;
        Board = new CellState[filas, numeroCartas / filas];
        CurrentTurn = CellState.X;
        IsGameOver = false;
        Winner = CellState.Empty;
        PuntuacionJugador1 = 0;
        PuntuacionJugador2 = 0;
        ultimoValor = 0;
        CambioTurno = true;
        comprobandoPareja = false;
        row1 = col1 = row2 = col2 = 0;
        ResetImages();

    }

    private IEnumerator OcultarPareja(int valor)
    {
        // Dejar ver las dos cartas antes de ocultarlas y cambiar el turno.
        yield return new WaitForSeconds(1f);
        CheckPoint(ultimoValor, valor);
        CurrentTurn = CurrentTurn == CellState.X ? CellState.O : CellState.X;
        CambioTurno = true;
        comprobandoPareja = false;
    }

    private bool IsBoardFull()
    {
        // Recorrer el tablero en busca de una casilla vacía (CellState.Empty)
        foreach (CellState cell in Board)
        {
            if (cell == CellState.Empty)
                return false;
        }
        return true;
    }

    public bool CheckWin(int row, int col)
    {
        /*
        // Comprobar la fila en la que estoy
        if (Board[row, 0] == CurrentTurn && Board[row, 1] == CurrentTurn && Board[row, 2] == CurrentTurn)
            return true;
        
        // Comprobar la columna en la que estoy
        if (Board[0, col] == CurrentTurn && Board[1, col] == CurrentTurn && Board[2, col] == CurrentTurn)
            return true;
        
        // Comprobar la diagonal principal
        if (Board[0, 0] == CurrentTurn && Board[1, 1] == CurrentTurn && Board[2, 2] == CurrentTurn)
            return true;
        
        // Comprobar la diagonal secundaria
        if (Board[0, 2] == CurrentTurn && Board[1, 1] == CurrentTurn && Board[2, 0] == CurrentTurn)
            return true;
        
        return false;
        */
        if (IsBoardFull())
        {
            return true;
        }
        return false;
    }

    public bool CheckPoint(int valor1, int valor2)
    {
        //la idea es pasarle los dos movimientos que ha hecho el jugador para verificar si ha ganado un punto

        if (valor1 == valor2)
        {
            if (CurrentTurn == CellState.X)
            {
                PuntuacionJugador1++;
            }
            if (CurrentTurn == CellState.O)
            {
                PuntuacionJugador2++;
            }

            
            
            return true;
        }
        
        Board[row1, col1] = CellState.Empty;
        Board[row2, col2] = CellState.Empty;
        

        
        
        return false;
    }
}
