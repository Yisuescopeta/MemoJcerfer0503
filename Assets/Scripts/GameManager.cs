using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Player { None, Player1, Player2 }


public class GameManager : MonoBehaviour

{
    
    
    // ajustes de las cartas y del tiempo
    [SerializeField] private GameObject cellsContainer;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField, Min(2)] private int numeroCartas = 6;
    [SerializeField, Min(1)] private float duracionContrarreloj = 60f;
    public bool Contrarreloj { get; set; }
    public float TiempoRestante { get; private set; }
    // guarda qué cartas están descubiertas
    public bool[,] Board { get; private set; } = new bool[2, 3];
    
    public int PuntuacionJugador1 { get; private set; } = 0;
    
    public int PuntuacionJugador2 { get; private set; } = 0;
    public static int ultimoValor { get; private set; } = 0;
    
    public Player CurrentTurn { get; private set; } = Player.Player1;
    private bool esperandoPrimeraCarta = true;
    private bool comprobandoPareja;
    private BoardCell[] cells;
    
    public bool IsGameOver { get; private set; }
    public Player Winner { get; private set; } = Player.None;
    
    // posiciones de las dos últimas cartas elegidas
    public int row1, col1, row2, col2;
    
    
    
    // permite usar este controlador desde otros scripts
    
    public static GameManager Instance { get; private set;  }

    public void Awake(){
        
        // guarda este controlador si todavía no hay uno
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // borra este objeto si ya hay otro controlador
            Destroy(gameObject);
        }
    }

    // baja el tiempo y acaba la partida al llegar a cero
    private void Update()
    {
        if (!Contrarreloj || IsGameOver) return;

        TiempoRestante = Mathf.Max(0f, TiempoRestante - Time.deltaTime);
        if (TiempoRestante > 0f) return;

        IsGameOver = true;
        StopAllCoroutines();
        comprobandoPareja = false;
    }

    // prepara las cartas para la partida
    private void ResetImages()
    {
        if (!cellsContainer || !cellPrefab)
        {
            return;
        }

        if (cells == null || cells.Length != numeroCartas)
        {
            CrearCartas();
        }

        RepartirCartas();
        AjustarTablero();
    }

    // borra las cartas anteriores y crea las nuevas
    private void CrearCartas()
    {
        if (cells != null)
        {
            foreach (BoardCell carta in cells)
            {
                carta.gameObject.SetActive(false);
                Destroy(carta.gameObject);
            }
        }

        cells = new BoardCell[numeroCartas];
        int indice = 0;

        // coloca cada carta en su fila y columna
        for (int fila = 0; fila < Board.GetLength(0); fila++)
        {
            for (int columna = 0; columna < Board.GetLength(1); columna++)
            {
                GameObject objetoCarta = Instantiate(cellPrefab, cellsContainer.transform);
                BoardCell carta = objetoCarta.GetComponent<BoardCell>();
                carta.row = fila;
                carta.col = columna;
                cells[indice] = carta;
                indice++;
            }
        }
    }

    // prepara las parejas y reparte sus valores al azar
    private void RepartirCartas()
    {
        List<int> valores = new List<int>();
        int numeroParejas = numeroCartas / 2;

        // añade dos cartas con el mismo valor
        for (int pareja = 0; pareja < numeroParejas; pareja++)
        {
            valores.Add(pareja);
            valores.Add(pareja);
        }

        // elige un valor al azar y lo quita de la lista
        foreach (BoardCell carta in cells)
        {
            int posicion = Random.Range(0, valores.Count);
            carta.valor = valores[posicion];
            valores.RemoveAt(posicion);
            carta.Refresh();
        }
    }

    // ajusta el tamaño de las cartas al espacio del tablero
    private void AjustarTablero()
    {
        GridLayoutGroup grid = cellsContainer.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            return;
        }

        // actualiza el tablero antes de medirlo
        Canvas.ForceUpdateCanvases();
        Rect area = ((RectTransform)cellsContainer.transform).rect;
        int filas = Board.GetLength(0);
        int columnas = Board.GetLength(1);

        // resta los bordes y los huecos entre las cartas
        float anchoDisponible = area.width - grid.padding.horizontal;
        float altoDisponible = area.height - grid.padding.vertical;
        anchoDisponible -= grid.spacing.x * (columnas - 1);
        altoDisponible -= grid.spacing.y * (filas - 1);

        float anchoCarta = anchoDisponible / columnas;
        float altoCarta = altoDisponible / filas;
        // usa el lado más pequeño para que las cartas sean cuadradas
        float lado = Mathf.Min(anchoCarta, altoCarta);
        if (lado < 1f)
        {
            lado = 1f;
        }

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columnas;
        grid.cellSize = new Vector2(lado, lado);
    }

    // revisa el número de cartas al cambiarlo en el editor
    private void OnValidate()
    {
        ValidarNumeroCartas();
    }

    // deja un mínimo de dos cartas y una cantidad par
    private void ValidarNumeroCartas()
    {
        if (numeroCartas < 2)
        {
            numeroCartas = 2;
        }

        if (numeroCartas % 2 != 0)
        {
            numeroCartas++;
        }
    }

    // calcula las filas y columnas del tablero
    private void CrearTablero()
    {
        int filas = 1;

        // busca el divisor que deja el tablero más cuadrado
        for (int divisor = 2; divisor * divisor <= numeroCartas; divisor++)
        {
            if (numeroCartas % divisor == 0)
            {
                filas = divisor;
            }
        }

        int columnas = numeroCartas / filas;
        Board = new bool[filas, columnas];
    }

    // comprueba el clic y guarda la carta elegida
    public bool PlayMove(int row, int col, int valor)
    {
        
        // bloquea los clics si la partida ha acabado o hay dos cartas esperando
        if (IsGameOver || comprobandoPareja)
        {
            return false;
        }

        // comprueba que la posición está dentro del tablero
        if (row < 0 || row >= Board.GetLength(0))
        {
            return false;
        }

        if (col < 0 || col >= Board.GetLength(1))
        {
            return false;
        }

        // no deja elegir una carta que ya está descubierta
        if (Board[row, col])
        {
            return false;
        }

        // descubre la carta y guarda su posición
        Board[row, col] = true;
        
        row2 = row1;
        col2 = col1;
        row1 = row;
        col1 = col;
        
        
        // guarda el valor de la primera carta para compararlo con la siguiente
        if (esperandoPrimeraCarta)
        {
            ultimoValor = valor;
            esperandoPrimeraCarta = false;
            return true;
        }

        // si son distintas espera antes de taparlas
        if (ultimoValor != valor)
        {
            comprobandoPareja = true;
            StartCoroutine(OcultarPareja(valor));
            return true;
        }

        // suma el punto si las cartas coinciden
        CheckPoint(ultimoValor, valor);
        esperandoPrimeraCarta = true;

        // termina la partida y elige al ganador si ya están todas descubiertas
        if (IsBoardFull())
        {
            IsGameOver = true;
            if (PuntuacionJugador1 > PuntuacionJugador2)
            {
                Winner = Player.Player1;
            }
            else if (PuntuacionJugador2 > PuntuacionJugador1)
            {
                Winner = Player.Player2;
            }
            else
            {
                Winner = Player.None;
            }
        }

        return true;
    }

    // reinicia el tiempo, los puntos y las cartas
    public void StartNewGame()
    {
        StopAllCoroutines();
        ValidarNumeroCartas();
        TiempoRestante = Mathf.Max(1f, duracionContrarreloj);
        CrearTablero();
        CurrentTurn = Player.Player1;
        IsGameOver = false;
        Winner = Player.None;
        PuntuacionJugador1 = 0;
        PuntuacionJugador2 = 0;
        ultimoValor = 0;
        esperandoPrimeraCarta = true;
        comprobandoPareja = false;
        row1 = col1 = row2 = col2 = 0;
        ResetImages();

    }

    private IEnumerator OcultarPareja(int valor)
    {
        // deja ver las dos cartas durante un segundo
        yield return new WaitForSeconds(1f);
        CheckPoint(ultimoValor, valor);
        // cambia de jugador solo en el modo de dos jugadores
        if (!Contrarreloj)
        {
            if (CurrentTurn == Player.Player1)
            {
                CurrentTurn = Player.Player2;
            }
            else
            {
                CurrentTurn = Player.Player1;
            }
        }
        esperandoPrimeraCarta = true;
        comprobandoPareja = false;
    }

    private bool IsBoardFull()
    {
        // comprueba si quedan cartas por descubrir
        foreach (bool cell in Board)
        {
            if (!cell)
                return false;
        }
        return true;
    }

        /*
        // comprueba la fila
        if (Board[row, 0] == CurrentTurn && Board[row, 1] == CurrentTurn && Board[row, 2] == CurrentTurn)
            return true;
        
        // comprueba la columna
        if (Board[0, col] == CurrentTurn && Board[1, col] == CurrentTurn && Board[2, col] == CurrentTurn)
            return true;
        
        // comprueba la diagonal principal
        if (Board[0, 0] == CurrentTurn && Board[1, 1] == CurrentTurn && Board[2, 2] == CurrentTurn)
            return true;
        
        // comprueba la diagonal secundaria
        if (Board[0, 2] == CurrentTurn && Board[1, 1] == CurrentTurn && Board[2, 0] == CurrentTurn)
            return true;
        
        return false;
        */

    public bool CheckPoint(int valor1, int valor2)
    {
        // suma un punto al jugador si las cartas son iguales

        if (valor1 == valor2)
        {
            if (CurrentTurn == Player.Player1)
            {
                PuntuacionJugador1++;
            }
            if (CurrentTurn == Player.Player2)
            {
                PuntuacionJugador2++;
            }

            
            
            return true;
        }
        
        // tapa las dos cartas si son distintas
        Board[row1, col1] = false;
        Board[row2, col2] = false;
        

        
        
        return false;
    }
}
