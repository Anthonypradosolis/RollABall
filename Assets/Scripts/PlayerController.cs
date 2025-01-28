using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Controlador del jugador que maneja el movimiento basado en entradas del usuario.
public class PlayerController : MonoBehaviour
{
    // Componente Rigidbody del jugador, utilizado para aplicar física.
    private Rigidbody rb; 

    // Variables para almacenar el movimiento en los ejes X e Y.
    private float movementX;
    private float movementY;

    // Velocidad a la que el jugador se moverá.
    public float speed = 0; 

    public GameObject Pared_Invisible_0;

    // Cuenta de los pickup
    private int count;

    // Este método se llama antes de que comience la primera actualización del frame.
    void Start()
    {
        // Obtiene y almacena el componente Rigidbody adjunto al jugador.
        rb = GetComponent<Rigidbody>();
    }
 
    // Método que se llama cuando se detecta una entrada de movimiento.
    void OnMove(InputValue movementValue)
    {
        // Convierte el valor de entrada en un Vector2 para el movimiento.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Almacena los componentes X e Y del movimiento.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    // FixedUpdate se llama en intervalos constantes, ideal para cálculos de física.
    private void FixedUpdate() 
    {
        // Crea un vector de movimiento en 3D usando las entradas de X e Y.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Aplica una fuerza al Rigidbody para mover al jugador.
        rb.AddForce(movement * speed); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            count+=1;
            if(count>=2){
                Pared_Invisible_0.SetActive(false);
            }
        }
    }
}
