using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
{
    // fila, columna y valor de la carta
    [SerializeField] public int row;
    [SerializeField] public int col;
    [SerializeField] public int valor = -1;
     
    

    

    private Sprite _spriteDefault;
        
    private Image _image;
    
    // imágenes de las parejas
    [SerializeField] private Sprite[] imagenes;
    
    

    // guarda la imagen de la carta y su reverso
    private void Awake()
    {
        
        _image = GetComponent<Image>();
        _spriteDefault = _image.sprite;
        
    }
    // intenta jugar la carta al pulsarla
    
    
    public void OnClick()
    {
        if (GameManager.Instance.IsGameOver)
        {
            
            return;
        } 
        
        // muestra la carta si el clic se acepta
        if (GameManager.Instance.PlayMove(row, col, valor))
        {
           
            Refresh();
        }
    }
        

    // muestra la imagen de la carta o su reverso
    public void Refresh()
    {
        bool descubierta = GameManager.Instance.Board[row, col];
        
        
        if (descubierta)
        {
            _image.sprite = imagenes[valor];
        }
        else
        {
            _image.sprite = _spriteDefault;
        }
        
    }
    
    // actualiza la imagen de la carta
    private void Update()
    {
        Refresh();
    }
}
