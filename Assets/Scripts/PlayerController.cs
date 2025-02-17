using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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

    public GameObject winTextObject;

    public TextMeshProUGUI countText;

    public GameObject Pared_Invisible_0;

    // Cuenta de los pickup
    private int count;

    private float jumpForce = 4f;

    private bool canJump = false;

    private bool isGrounded = true;

    private Vector3 initialPosition;


    private List<GameObject> allPickups = new List<GameObject>();

    private List<GameObject> allEnemies = new List<GameObject>();

    private Dictionary<GameObject, Vector3> enemyStartPositions = new Dictionary<GameObject, Vector3>();

    private Animator animation;

    private enum PlayerState{Inactivo, Caminando, Cayendo, Muerto,BuffSalto}

    private PlayerState estadoActual;

    private bool isMoving = false;

    // Este método se llama antes de que comience la primera actualización del frame.
    void Start()
    {

        // Obtiene y almacena el componente Rigidbody adjunto al jugador.
        rb = GetComponent<Rigidbody>();

        count = 0;

        SetCountText();

        winTextObject.SetActive(false);

        initialPosition = transform.position;

        // Guardar todos los pickups al inicio
        GameObject[] pickupsArray = GameObject.FindGameObjectsWithTag("PickUp");
        allPickups.AddRange(pickupsArray);

        animation = GetComponent<Animator>();
        estadoActual = PlayerState.Inactivo;
        UpdateAnimator();

        /**
        GameObject[] enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemiesArray)
        {
            allEnemies.Add(enemy);
            enemyStartPositions[enemy] = enemy.transform.position;
        }
        **/



    }


    // Método que se llama cuando se detecta una entrada de movimiento.
    void OnMove(InputValue movementValue)
    {
        // Convierte el valor de entrada en un Vector2 para el movimiento.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Almacena los componentes X e Y del movimiento.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
        isMoving = movementX !=0 || movementY !=0;

        if(isGrounded && isMoving){
            estadoActual = PlayerState.Caminando;
        }
        else if(!isGrounded){
            estadoActual = PlayerState.Cayendo;
        }
        UpdateAnimator();
    }

    void SetCountText(){
        countText.text = "Count: "+ count.ToString();
        if(count >=12){
            winTextObject.SetActive(true);
        }
    }

    // FixedUpdate se llama en intervalos constantes, ideal para cálculos de física.
     void FixedUpdate() 
    {
        // Crea un vector de movimiento en 3D usando las entradas de X e Y.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Aplica una fuerza al Rigidbody para mover al jugador.
        rb.AddForce(movement * speed); 
    }

     void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Destroy(gameObject);

            Respawn();

            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You lose!";
        }

        if(collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
        //    animation.SetBool("BuffSalto",false);
            
            estadoActual = isMoving ? PlayerState.Caminando : PlayerState.Inactivo;
            UpdateAnimator();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            estadoActual = PlayerState.Cayendo;
            UpdateAnimator();

        }
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
        SetCountText();

        if(other.gameObject.CompareTag("Jump"))
        {
            canJump = true;
            other.gameObject.SetActive(false);
            animation.SetBool("BuffSalto",true);
        //    estadoActual = PlayerState.Saltando;
        //    UpdateAnimator();
        }

    

    }


    void Jump(){
        rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.y);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    //    animation.SetBool("BuffSalto",true);
        estadoActual = PlayerState.BuffSalto;
        UpdateAnimator();
        
    }

    void Update(){
        Debug.Log("isGrounded" + isGrounded);
        if(canJump && isGrounded && Input.GetKeyDown(KeyCode.Space)){
            Jump();

            //animation.SetBool("Caminando",true);
            //estadoActual = PlayerState.Caminando;
            //UpdateAnimator();
        } else if(isGrounded && isMoving){
                estadoActual = PlayerState.Caminando;
        } else if(!isGrounded){
                estadoActual = PlayerState.Cayendo;
        } else if(!isMoving){
                estadoActual = PlayerState.Inactivo;
        }
        UpdateAnimator();


        /**
        else if(!isGrounded){
            animation.SetBool("BuffSalto",false);
            animation.SetBool("Caminando",true);
            UpdateAnimator();
        }
        **/

    }

    void Respawn()
    {
        Invoke("ResetPlayer", 1f);
    }

    void ResetPlayer()
    {
        transform.position = initialPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

        foreach (GameObject pickup in allPickups)
        {
            pickup.SetActive(true);
        }
        /**
        foreach (GameObject enemy in allEnemies)
        {
            enemy.SetActive(false); // Desactiva el enemigo antes de moverlo
            enemy.transform.position = enemyStartPositions[enemy]; // Restablece su posición
            enemy.SetActive(true); // Vuelve a activarlo para reiniciar su física

            // Reiniciar Rigidbody para evitar atravesar objetos
            Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                enemyRb.velocity = Vector3.zero;
                enemyRb.angularVelocity = Vector3.zero;
            }
        }
        **/





    }

    void UpdateAnimator(){
        animation.SetBool("Caminando", estadoActual == PlayerState.Caminando);
        animation.SetBool("BuffSalto",estadoActual == PlayerState.BuffSalto);
     //   animation.SetBool("BuffSalto", canJump);
        animation.SetBool("Cayendo",estadoActual == PlayerState.Cayendo);
        animation.SetBool("Muerto",estadoActual == PlayerState.Muerto);
    }

}

