using UnityEngine;
using UnityEngine.InputSystem; // Permite utilizar el nuevo sistema de entrada.

// Esta clase controla el movimiento de Deiva.
public class PlayerController : MonoBehaviour
{
    // Referencia al componente CharacterController de Deiva.
    private CharacterController controller;

    // Referencia al componente Animator de Deiva.
    private Animator anim;

    // Velocidad automática de avance de Deiva.
    public float forwardSpeed = 5f;

    // Velocidad de movimiento hacia los lados.
    public float lateralSpeed = 6f;

    // Altura del salto.
    public float jumpHeight = 2f;

    // Fuerza de gravedad.
    public float gravity = -20f;

    // Velocidad vertical actual de Deiva.
    private Vector3 velocity;

    // Se ejecuta una vez al iniciar el objeto.
    void Start()
    {
        // Obtiene el componente CharacterController del jugador.
        controller = GetComponent<CharacterController>();

        // Obtiene el componente Animator del jugador.
        anim = GetComponent<Animator>();
    }

    // Se ejecuta una vez por cada fotograma.
    void Update()
    {
        // Comprueba si existe un teclado conectado.
        if (Keyboard.current == null)
        {
            return;
        }

        // Comprueba si se presiona A o la flecha izquierda.
        bool moveLeft = Keyboard.current.aKey.isPressed ||
                        Keyboard.current.leftArrowKey.isPressed;

        // Comprueba si se presiona D o la flecha derecha.
        bool moveRight = Keyboard.current.dKey.isPressed ||
                         Keyboard.current.rightArrowKey.isPressed;

        // Variable que almacena el movimiento horizontal.
        float horizontal = 0f;

        // Si se presiona izquierda, el valor será -1.
        if (moveLeft)
        {
            horizontal = -1f;
        }

        // Si se presiona derecha, el valor será 1.
        if (moveRight)
        {
            horizontal = 1f;
        }

        // Crea el movimiento automático hacia adelante
        // y el movimiento lateral controlado por el jugador.
        Vector3 move = transform.forward * forwardSpeed
                     + transform.right * horizontal * lateralSpeed;

        // Comprueba si Deiva está tocando el suelo.
        if (controller.isGrounded)
        {
            // Mantiene una pequeña velocidad hacia abajo
            // para que el CharacterController permanezca en el suelo.
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Comprueba si se acaba de presionar la barra espaciadora.
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                // Calcula la velocidad necesaria para alcanzar la altura del salto.
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                // Activa la animación de salto.
                anim.SetBool("IsJumping", true);
            }
        }

        // Aplica la gravedad a la velocidad vertical.
        velocity.y += gravity * Time.deltaTime;

        // Agrega el movimiento vertical al movimiento general.
        move += velocity;

        // Mueve físicamente a Deiva utilizando el CharacterController.
        controller.Move(move * Time.deltaTime);
    }
}