using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    //[SerializeField] private BoardCell[] boardsCell;

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI puntuacion1Text;
    [SerializeField] private TextMeshProUGUI puntuacion2Text;
    
    
    void Start()
    {
        
        Reset();
        
    }
    
    // Gestión del click sobre el botón de RESET
    public void OnClickReset()
    {
       
        Reset();
        
    }

    
    
    private void Reset()
    {
        // Limpia el Tablero (modelo)
        GameManager.Instance.StartNewGame();
        
        // Actualiza la interface de usuario
        UpdateUI();
    }

    public void UpdateUI()
    {
        
        
        // Actualiza el texto de estado
        if (GameManager.Instance.IsGameOver)
        {
            puntuacion1Text.text = "Puntuación: " +  GameManager.Instance.PuntuacionJugador1;
            puntuacion2Text.text = "Puntuación: " +  GameManager.Instance.PuntuacionJugador2;
            // poner ganador o empate
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
            // informar puntuaciones
            puntuacion1Text.text = "Puntuación: " +  GameManager.Instance.PuntuacionJugador1;
            puntuacion2Text.text = "Puntuación: " +  GameManager.Instance.PuntuacionJugador2;
            // informar del turno actual
            statusText.text = $"Turno actual: {GameManager.Instance.CurrentTurn}";
        }
    }

    private void Update()
    {
        UpdateUI();
    }
}
