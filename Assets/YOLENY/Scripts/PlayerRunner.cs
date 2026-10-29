using UnityEngine;
using UnityEngine.InputSystem; // Permite utilizar el nuevo sistema de entrada.
using UnityEngine.SceneManagement;
using System.Collections;

// Esta clase controla el movimiento de Deiva.
public class PlayerRunner : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    [Header("Movimiento")]
    public float forwardSpeed = 5f;
    public float lateralSpeed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Muerte")]
    public float deathDelay = 2f;

    private Vector3 velocity;
    private bool isDead = false;
    private bool canMove = false;

    // Indica si la partida ya comenzó.
    private bool hasStarted = false;

    // Evita que se active el salto varias veces en el aire.
    private bool isJumping = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        if (anim != null)
        {
            anim.SetBool("IsDead", false);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsRunning", false);
        }
    }

    void Update()
    {
        if (isDead || controller == null)
            return;

        // Antes de comenzar: Deiva permanece quieta,
        // pero puede saltar en el mismo sitio.
        if (!canMove)
        {
            if (controller.isGrounded)
            {
                if (velocity.y < 0f)
                    velocity.y = -2f;

                // Permitir saltar en Idle antes de iniciar.
                if (!hasStarted &&
                    !isJumping &&
                    Keyboard.current != null &&
                    Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    Saltar();
                }
            }

            AplicarGravedad();
            return;
        }

        // Movimiento lateral.
        float horizontal = 0f;

        if (Keyboard.current != null)
        {
            bool moveLeft =
                Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed;

            bool moveRight =
                Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed;

            if (moveLeft && !moveRight)
                horizontal = -1f;
            else if (moveRight && !moveLeft)
                horizontal = 1f;
        }

        // Avance automático cuando la partida ya comenzó.
        Vector3 move =
            transform.forward * forwardSpeed +
            transform.right * horizontal * lateralSpeed;

        // Detectar si está en el suelo antes de moverse.
        bool groundedBeforeMove = controller.isGrounded;

        if (groundedBeforeMove)
        {
            if (velocity.y < 0f)
                velocity.y = -2f;

            // Saltar mientras corre.
            if (!isJumping &&
                Keyboard.current != null &&
                Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Saltar();
            }
        }

        // Aplicar gravedad y movimiento.
        velocity.y += gravity * Time.deltaTime;
        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);

        // Detectar el aterrizaje.
        if (controller.isGrounded && velocity.y <= 0f)
        {
            isJumping = false;

            if (anim != null)
                anim.SetBool("IsJumping", false);
        }
    }

    void Saltar()
    {
        if (isDead || isJumping)
            return;

        isJumping = true;

        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        if (anim != null)
            anim.SetBool("IsJumping", true);

        Debug.Log("Salto activado. Partida iniciada: " + hasStarted);
    }

    void AplicarGravedad()
    {
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        // Solo movimiento vertical antes de comenzar.
        controller.Move(Vector3.up * velocity.y * Time.deltaTime);

        // Detectar el aterrizaje del salto en Idle.
        if (controller.isGrounded && velocity.y <= 0f)
        {
            if (isJumping)
            {
                isJumping = false;

                if (anim != null)
                    anim.SetBool("IsJumping", false);
            }
        }
    }

    // Llamar desde el botón que inicia la partida.
    public void StartRunning()
    {
        if (isDead)
            return;

        hasStarted = true;
        canMove = true;

        if (anim != null)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsRunning", true);
        }

        Debug.Log("PARTIDA INICIADA");
    }

    // Detener el movimiento durante el tutorial o una pausa.
    public void StopRunning()
    {
        canMove = false;
        velocity = Vector3.zero;
        isJumping = false;

        if (anim != null)
            anim.SetBool("IsJumping", false);
    }

    public void StopAtFinish()
    {
        if (isDead)
            return;

        canMove = false;
        velocity = Vector3.zero;
        isJumping = false;

        if (anim != null)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsRunning", false);
        }

        if (LevelCompletionManager.instance != null)
        {
            LevelCompletionManager.instance.CheckLevelCompletion();
        }
        else
        {
            Debug.LogError(
                "No se encontró LevelCompletionManager en la escena."
            );
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!canMove || isDead)
            return;

        if (hit.gameObject.CompareTag("Wall"))
        {
            Debug.Log("¡Pared detectada! Ejecutando muerte.");
            Morir();
        }
    }

    void Morir()
    {
        if (isDead)
            return;

        isDead = true;
        canMove = false;
        velocity = Vector3.zero;
        isJumping = false;

        if (anim != null)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsDead", true);
        }

        StartCoroutine(ReiniciarDespuesDeMorir());
    }

    IEnumerator ReiniciarDespuesDeMorir()
    {
        yield return new WaitForSeconds(deathDelay);

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // Compatible con Animation Events si los utilizas.
    void StopJump()
    {
        if (anim != null)
        {
            anim.SetBool("IsJumping", false);
        }

        isJumping = false;
    }
}