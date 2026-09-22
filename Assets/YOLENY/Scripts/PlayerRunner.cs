using UnityEngine;
using UnityEngine.InputSystem; // Permite utilizar el nuevo sistema de entrada.
using UnityEngine.SceneManagement;
using System.Collections;

// Esta clase controla el movimiento de Deiva.
public class PlayerRunner : MonoBehaviour
{
    // Referencia al CharacterController de Deiva.
    private CharacterController controller;

    // Referencia al Animator de Deiva.
    private Animator anim;

    // Velocidad automática de avance.
    public float forwardSpeed = 5f;

    // Velocidad de movimiento hacia los lados.
    public float lateralSpeed = 6f;

    // Altura del salto.
    public float jumpHeight = 2f;

    // Fuerza de gravedad.
    public float gravity = -20f;

    // Tiempo que dura la animación de muerte antes de reiniciar.
    public float deathDelay = 2f;

    // Velocidad vertical actual.
    private Vector3 velocity;

    // Evita que la muerte se active varias veces.
    private bool isDead = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Si Deiva murió, no permitimos ningún movimiento.
        if (isDead)
        {
            return;
        }

        // Comprueba si existe un teclado.
        if (Keyboard.current == null)
        {
            return;
        }

        // Movimiento izquierda.
        bool moveLeft = Keyboard.current.aKey.isPressed ||
                        Keyboard.current.leftArrowKey.isPressed;

        // Movimiento derecha.
        bool moveRight = Keyboard.current.dKey.isPressed ||
                         Keyboard.current.rightArrowKey.isPressed;

        float horizontal = 0f;

        if (moveLeft)
        {
            horizontal = -1f;
        }

        if (moveRight)
        {
            horizontal = 1f;
        }

        // Movimiento automático hacia adelante
        // y movimiento lateral.
        Vector3 move = transform.forward * forwardSpeed
                     + transform.right * horizontal * lateralSpeed;

        // Comprobar si está en el suelo.
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Salto.
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                anim.SetBool("IsJumping", true);
            }
            else
            {
                anim.SetBool("IsJumping", false);
            }
        }

        // Gravedad.
        velocity.y += gravity * Time.deltaTime;

        // Movimiento vertical.
        move += velocity;

        // Mover a Deiva.
        controller.Move(move * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Si toca una pared, muere.
        if (hit.gameObject.CompareTag("Wall"))
        {
            Morir();
        }
    }

    void Morir()
    {
        // Evita activar la muerte varias veces.
        if (isDead)
        {
            return;
        }

        isDead = true;

        // Detiene el movimiento.
        velocity = Vector3.zero;

        // Activa la animación de muerte.
        anim.SetBool("IsDead", true);

        // Espera y reinicia.
        StartCoroutine(ReiniciarDespuesDeMorir());
    }

    IEnumerator ReiniciarDespuesDeMorir()
    {
        yield return new WaitForSeconds(deathDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Animation Event del salto.
    void StopJump()
    {
        anim.SetBool("IsJumping", false);
    }
}