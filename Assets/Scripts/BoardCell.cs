using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
{
    [SerializeField] public int row;
    [SerializeField] public int col;
    [SerializeField] public int valor = -1;
     
    
    [SerializeField] private TextMeshProUGUI cellText;

    

    private Sprite _spriteDefault;
        
    private Image _image;
    
    [SerializeField] private Sprite[] imagenes;
    
    

    private void Awake()
    {
        
        _image = GetComponent<Image>();
        _spriteDefault = _image.sprite;
        
    }
    // respuesta al onClick
    
    
    public void OnClick()
    {
        if (GameManager.Instance.IsGameOver)
        {
            
            return;
        } 
        
        // Mostrar el valor solo si la jugada se ha aceptado.
        if (GameManager.Instance.PlayMove(row, col, valor))
        {
           
            Refresh();
        }
    }
        

    public void Refresh()
    {
        CellState state = GameManager.Instance.Board[row, col];
        
        
        if (state == CellState.X)
        {
            
            _image.sprite = imagenes[valor]; 
        }
        else if (state == CellState.O)
        {
            _image.sprite = imagenes[valor];
        }
        else
        {
            _image.sprite = _spriteDefault;
        }
        
        //cellText.text = (state == CellState.Empty) ? "" : state.ToString();
    }
    
    private void Update()
    {
        Refresh();
    }
}
