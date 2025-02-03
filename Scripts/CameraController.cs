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
    private float yRotation = 0f; // Control de la rotación horizontal (eje Y)

    // Parámetros para la vista en primera persona
    public float Sensibilidad = 100f;
    public float heightOffset = 1.5f; // Altura de la cámara en primera persona
    public float shoulderOffset = 0.5f; // Desplazamiento lateral para tercera persona
    public float distanceFromPlayer = 2.0f; // Distancia de la cámara al jugador en tercera persona
    public float movementSpeed = 5f; // Velocidad de movimiento de la pelota

    private Rigidbody playerRigidbody; // Rigidbody de la pelota

    // Guardamos la posición inicial de la cámara en tercera persona
    private Vector3 thirdPersonStartPosition;
    private Quaternion thirdPersonStartRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Calculamos el offset inicial para la cámara en tercera persona
        offset = transform.position - player.transform.position;

        // Guardar la posición y rotación iniciales de tercera persona
        thirdPersonStartPosition = transform.position;
        thirdPersonStartRotation = transform.rotation;

        // Bloqueamos el cursor para el modo en primera persona
        Cursor.lockState = CursorLockMode.Locked;

        // Obtener el Rigidbody del jugador
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    // Update se usa para alternar entre modos de cámara
    void Update()
    {
        // Cambiar al modo en tercera persona
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isFirstPerson = false;
            Cursor.lockState = CursorLockMode.None; // Mostrar el cursor

            // Restablecer la posición y rotación inicial de la cámara para tercera persona
            transform.position = thirdPersonStartPosition;
            transform.rotation = thirdPersonStartRotation;
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
        // Calculamos la nueva posición de la cámara en tercera persona
        Vector3 desiredPosition = player.transform.position + offset;
        transform.position = desiredPosition;

        // La cámara siempre mira al jugador
        transform.LookAt(player.transform);
    }

    // Método para la vista en primera persona
    void FirstPersonView()
    {
        // Entrada del ratón para rotación
        float mouseX = Input.GetAxis("Mouse X") * Sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Sensibilidad * Time.deltaTime;

        // Rotación vertical (cámara)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limitar el ángulo de visión vertical

        // Aplicar rotación vertical a la cámara
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Rotación horizontal (jugador)
        yRotation += mouseX;
        playerBody.Rotate(Vector3.up * mouseX); // Rotación horizontal del jugador

        // Colocar la cámara ligeramente sobre el jugador (simula la vista en primera persona)
        transform.position = player.transform.position + new Vector3(0, heightOffset, 0);

        // Mover al jugador basado en la cámara
        HandleFirstPersonMovement();
    }

    // Método para manejar el movimiento en primera persona
    private void HandleFirstPersonMovement()
    {
        // Obtener entrada de movimiento
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calcular las direcciones hacia adelante y derecha basadas en la rotación de la cámara
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Aplanar las direcciones para ignorar la rotación vertical
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calcular la dirección del movimiento
        Vector3 movementDirection = (forward * moveVertical + right * moveHorizontal).normalized;

        // Aplicar la velocidad directamente al Rigidbody del jugador
        playerRigidbody.velocity = movementDirection * movementSpeed + new Vector3(0, playerRigidbody.velocity.y, 0);
    }
}


