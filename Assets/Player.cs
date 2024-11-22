using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 10f;  // Velocidad hacia adelante
    public float laneDistance = 5f;   // Distancia entre carriles
    public float transitionSpeed = 15f; // Velocidad de transici�n entre carriles

    public float jumpHeight = 3f;     // Altura del salto
    public float jumpSpeed = 2f;      // Velocidad de salto

    private int currentLane = 1;      // �ndice de carril (0 = izquierda, 1 = centro, 2 = derecha)
    private bool isJumping = false;   // Indica si est� saltando
    private bool isDescending = false; // Indica si est� descendiendo
    private float startY;             // Posici�n inicial en el eje Y
    private Quaternion originalRotation; // Guardar rotaci�n original

    public float crouchingTime = 2f; // Tiempo en segundos durante el cual el jugador se agacha
    private bool isCrouching = false;
    private float crouchTimer = 0f;

    void Start()
    {
        // Guardar la posici�n inicial y la rotaci�n original del jugador
        startY = 1.5f;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        Vector3 moveDirection = new Vector3(1, 0, 0);
        Vector3 moveDirectionCrouching = new Vector3(1, 0, 0);
        if (!isCrouching)
        {
            transform.Translate(moveDirection * forwardSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(moveDirectionCrouching * forwardSpeed * Time.deltaTime);
        }
        // Cambiar de carril al presionar flecha derecha
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane < 2)
        {
            currentLane++;
        }

        // Cambiar de carril al presionar flecha izquierda
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane > 0)
        {
            currentLane--;
        }

        // Calcular la posici�n objetivo en el eje Z seg�n el carril actual
        float targetZ = (currentLane - 1) * laneDistance;

        // Moverse suavemente hacia la posici�n objetivo en el eje Z
        float newZ = Mathf.Lerp(transform.position.z, targetZ, transitionSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

        // Ejecutar salto si se presiona la tecla UpArrow
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
        {
            // Si est� agachado, primero regresa a la rotaci�n original antes de saltar
            if (isCrouching)
            {
                transform.Rotate(0f, 0f, -90f); // Volver a la rotaci�n original (en el eje Z)
                isCrouching = false;
                crouchTimer = 0f; // Reiniciar el temporizador de crouch
            }

            isJumping = true;
            isDescending = false;
        }

        // Manejo del salto
        if (isJumping)
        {
            // Subir hasta alcanzar la altura m�xima
            if (transform.position.y < startY + jumpHeight)
            {
                transform.position += Vector3.up * jumpSpeed * Time.deltaTime;
            }
            else
            {
                // Si se alcanza la altura m�xima, comienza a bajar
                isDescending = true;
                isJumping = false;
            }
        }

        // Manejo de la bajada
        if (isDescending)
        {
            // Bajar hasta la posici�n inicial
            transform.position -= Vector3.up * jumpSpeed * Time.deltaTime;

            // Asegurar que el jugador se quede en la posici�n inicial (startY) al aterrizar
            if (transform.position.y <= startY)
            {
                transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                isDescending = false;
            }
        }

        // Cambiar a agachado (crouch) cuando presionas la flecha hacia abajo
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Si est� saltando o descendiendo, primero debe aterrizar antes de agacharse
            if (isJumping || isDescending)
            {
                return;
            }

            isCrouching = true;
            crouchTimer = 0f; // Reiniciar el temporizador
            transform.Rotate(0f, 0f, 90f); // Rotar 90 grados en el eje Z para agacharse
        }

        // Si el jugador est� agachado, actualizar el temporizador
        if (isCrouching)
        {
            crouchTimer += Time.deltaTime;

            // Si el tiempo de agachado ha pasado, volver a la posici�n original
            if (crouchTimer >= crouchingTime)
            {
                isCrouching = false;
                transform.Rotate(0f, 0f, -90f); // Volver a la rotaci�n original (en el eje Z)
            }
        }
    }
}
