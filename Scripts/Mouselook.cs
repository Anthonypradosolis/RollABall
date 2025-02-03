using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase para manejar la rotación de la cámara en base al movimiento del mouse.
public class Mouselook : MonoBehaviour
{
    // Sensibilidad del movimiento del mouse (controla la velocidad de la rotación).
    public float Sensibilidad = 100;

    // Referencia al cuerpo del jugador para rotarlo.
    public Transform playerBody;

    // Rotación acumulada en el eje X para limitar la vista vertical.
    public float xRotacion;

    // Start se llama antes de la primera actualización del frame.
    void Start()
    {
        // Bloquea el cursor en el centro de la pantalla y lo oculta.
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update se llama una vez por cada frame.
    void Update()
    {
        // Obtiene los movimientos del mouse en los ejes X e Y y ajusta la sensibilidad.
        float mouseX = Input.GetAxis("Mouse X") * Sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Sensibilidad * Time.deltaTime;

        // Acumula la rotación en el eje X y limita la rotación vertical para evitar giros completos.
        xRotacion -= mouseY;
        xRotacion = Mathf.Clamp(xRotacion, -90, 90);

        // Aplica la rotación acumulada en el eje X a la cámara.
        transform.localRotation = Quaternion.Euler(xRotacion, 0, 0);

        // Rota el cuerpo del jugador en el eje Y para simular el giro horizontal.
        playerBody.Rotate(Vector3.up * mouseX);

        // Imprime el valor de la rotación en el eje X (útil para depuración).
        print(xRotacion);
    }
}

