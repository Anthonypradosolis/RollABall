using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // La pelota
    public float mouseSensitivity = 600f; // Sensibilidad del ratón
    public Transform playerBody; // El transform del jugador para rotar el cuerpo
    
    private Vector3 offset; // Offset para la cámara en tercera persona
    private bool isFirstPerson = false; // Modo actual de la cámara
    private float xRotation = 0f; // Control de la rotación vertical (eje X)

    public float heightOffset = 1.5f;
    public float shoulderOffset = 0.5f;
    public float distanceFromPlayer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Calculamos el offset inicial para la cámara en tercera persona
        offset = transform.position - player.transform.position;

        // Bloqueamos el cursor para el modo en primera persona
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update se usa para alternar entre modos de cámara
    void Update()
    {
        // Cambiar al modo en tercera persona
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isFirstPerson = false;
            Cursor.lockState = CursorLockMode.None; // Mostrar el cursor
        }

        // Cambiar al modo en primera persona
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isFirstPerson = true;
            Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor
        }

        // Lógica del modo de cámara
        if (isFirstPerson)
        {
            FirstPersonView();
        }
        else
        {
            ThirdPersonView();
        }
    }

    // Método para la vista en tercera persona
    void ThirdPersonView()
    {
        transform.position = player.transform.position + offset;
    }

    // Método para la vista en primera persona
    void FirstPersonView()
    {
        // Entrada del ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Debug: Verificar los valores del ratón
        Debug.Log($"MouseX: {mouseX}, MouseY: {mouseY}");

        // Rotación vertical (cámara)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limitar el ángulo de visión vertical

        // Aplicar rotación vertical a la cámara
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación horizontal (jugador)
        playerBody.Rotate(Vector3.up * mouseX);

        // Debug: Confirmar que la rotación se está aplicando
        Debug.Log($"Player Rotation Y: {playerBody.rotation.eulerAngles.y}");

        // Ajustar la posición lateral de la cámara con base en el movimiento del ratón
        // Desplazamos la cámara lateralmente según el movimiento de mouseX
        shoulderOffset += mouseX * 0.5f; // Puedes ajustar este factor para más o menos sensibilidad

        // Ajustar la posición de la cámara para seguir el movimiento del jugador, con el desplazamiento lateral
        Vector3 shoulderPosition = playerBody.position + new Vector3(shoulderOffset, heightOffset, -distanceFromPlayer);
        transform.position = Vector3.Lerp(transform.position, shoulderPosition, Time.deltaTime * 5f);  // Suavizado
    }

}

