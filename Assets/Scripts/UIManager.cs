using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    // textos de la pantalla

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI puntuacion1Text;
    [SerializeField] private TextMeshProUGUI puntuacion2Text;
    
    
    // empieza una partida al abrir el juego
    void Start()
    {
        
        Reset();
        
    }
    
    // reinicia la partida al pulsar el botón
    public void OnClickReset()
    {
       
        Reset();
        
    }

    
    
    // activa el modo contrarreloj y empieza de nuevo
    public void OnClickContrarreloj()
    {
        GameManager.Instance.Contrarreloj = true;
        Reset();
    }

    // activa el modo de dos jugadores y empieza de nuevo
    public void OnClickDosJugadores()
    {
        GameManager.Instance.Contrarreloj = false;
        Reset();
    }

    private void Reset()
    {
        // empieza una partida nueva
        GameManager.Instance.StartNewGame();
        
        // actualiza los textos
        UpdateUI();
    }

    // muestra los puntos y el estado de la partida
    public void UpdateUI()
    {
        // muestra el tiempo que queda o el resultado final
        if (GameManager.Instance.Contrarreloj)
        {
            puntuacion1Text.text = "Puntuación: " + GameManager.Instance.PuntuacionJugador1;
            puntuacion2Text.text = "Puntuación: " + GameManager.Instance.PuntuacionJugador2;
            statusText.text = GameManager.Instance.IsGameOver
                ? (GameManager.Instance.Winner == Player.Player1 ? "¡Has ganado!" : "¡Tiempo agotado!")
                : $"Tiempo: {Mathf.CeilToInt(GameManager.Instance.TiempoRestante)} s";
            return;
        }
        
        
        // muestra el resultado si la partida ha acabado
        if (GameManager.Instance.IsGameOver)
        {
            puntuacion1Text.text = "Puntuación: " +  GameManager.Instance.PuntuacionJugador1;
            puntuacion2Text.text = "Puntuación: " +  GameManager.Instance.PuntuacionJugador2;
            // compara los puntos para mostrar el ganador o el empate
            if (GameManager.Instance.PuntuacionJugador1 == GameManager.Instance.PuntuacionJugador2)
            {
                statusText.text = "¡ Empate !";
            }
            else if (GameManager.Instance.PuntuacionJugador1 > GameManager.Instance.PuntuacionJugador2)
            {
                statusText.text = $"¡ Ganador jugador 1 !";
            }
            else
            {
                 statusText.text = $"¡ Ganador jugador 2 !";
            }
        }
        else
        {
            // muestra los puntos de cada jugador
            puntuacion1Text.text = "Puntuación: " +  GameManager.Instance.PuntuacionJugador1;
            puntuacion2Text.text = "Puntuación: " +  GameManager.Instance.PuntuacionJugador2;
            // muestra a quién le toca jugar
            statusText.text = $"Turno actual: jugador {(int)GameManager.Instance.CurrentTurn}";
        }
    }

    // actualiza los textos durante la partida
    private void Update()
    {
        UpdateUI();
    }
}
