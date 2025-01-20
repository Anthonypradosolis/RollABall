using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // La pelota
    public float mouseSensitivity = 600f; // Sensibilidad del rat�n
    public Transform playerBody; // El transform del jugador para rotar el cuerpo
    
    private Vector3 offset; // Offset para la c�mara en tercera persona
    private bool isFirstPerson = false; // Modo actual de la c�mara
    private float xRotation = 0f; // Control de la rotaci�n vertical (eje X)

    public float heightOffset = 1.5f;
    public float shoulderOffset = 0.5f;
    public float distanceFromPlayer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Calculamos el offset inicial para la c�mara en tercera persona
        offset = transform.position - player.transform.position;

        // Bloqueamos el cursor para el modo en primera persona
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update se usa para alternar entre modos de c�mara
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

        // L�gica del modo de c�mara
        if (isFirstPerson)
        {
            FirstPersonView();
        }
        else
        {
            ThirdPersonView();
        }
    }

    // M�todo para la vista en tercera persona
    void ThirdPersonView()
    {
        transform.position = player.transform.position + offset;
    }

    // M�todo para la vista en primera persona
    void FirstPersonView()
    {
        // Entrada del rat�n
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Debug: Verificar los valores del rat�n
        Debug.Log($"MouseX: {mouseX}, MouseY: {mouseY}");

        // Rotaci�n vertical (c�mara)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limitar el �ngulo de visi�n vertical

        // Aplicar rotaci�n vertical a la c�mara
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotaci�n horizontal (jugador)
        playerBody.Rotate(Vector3.up * mouseX);

        // Debug: Confirmar que la rotaci�n se est� aplicando
        Debug.Log($"Player Rotation Y: {playerBody.rotation.eulerAngles.y}");

        // Ajustar la posici�n lateral de la c�mara con base en el movimiento del rat�n
        // Desplazamos la c�mara lateralmente seg�n el movimiento de mouseX
        shoulderOffset += mouseX * 0.5f; // Puedes ajustar este factor para m�s o menos sensibilidad

        // Ajustar la posici�n de la c�mara para seguir el movimiento del jugador, con el desplazamiento lateral
        Vector3 shoulderPosition = playerBody.position + new Vector3(shoulderOffset, heightOffset, -distanceFromPlayer);
        transform.position = Vector3.Lerp(transform.position, shoulderPosition, Time.deltaTime * 5f);  // Suavizado
    }

}

