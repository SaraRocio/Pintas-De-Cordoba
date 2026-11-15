
using UnityEngine;
using UnityEngine.InputSystem;
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

    [Header("Mejora del salto")]
    public float jumpBufferTime = 0.15f;
    public float coyoteTime = 0.1f;

    [Header("Muerte")]
    public float deathDelay = 2f;

    private Vector3 velocity;

    private bool isDead = false;
    private bool canMove = false;
    private bool hasStarted = false;
    private bool isJumping = false;

    private float jumpBufferCounter = 0f;
    private float coyoteTimeCounter = 0f;

    // Evita saltos dobles antes de aterrizar.
    private bool jumpConsumed = false;

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

        // Detectar pulsación de salto.
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Comprobar si Deiva está en el suelo.
        bool grounded = controller.isGrounded;

        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;

            // Solo restablecer el salto al aterrizar.
            if (velocity.y <= 0f)
            {
                jumpConsumed = false;

                if (isJumping)
                {
                    isJumping = false;

                    if (anim != null)
                        anim.SetBool("IsJumping", false);
                }

                velocity.y = -2f;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Permitir salto en Idle antes de iniciar
        // y salto normal durante la carrera.
        bool puedeSaltar = !jumpConsumed &&
                            coyoteTimeCounter > 0f &&
                            jumpBufferCounter > 0f &&
                            (!hasStarted || canMove);

        if (puedeSaltar)
        {
            Saltar();
        }

        // Movimiento horizontal.
        float horizontal = 0f;

        if (canMove && Keyboard.current != null)
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

        Vector3 move = Vector3.zero;

        // Avance automático únicamente al correr.
        if (canMove)
        {
            move =
                transform.forward * forwardSpeed +
                transform.right * horizontal * lateralSpeed;
        }

        // Gravedad.
        velocity.y += gravity * Time.deltaTime;
        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);

        // Comprobar aterrizaje después del movimiento.
        if (controller.isGrounded && velocity.y <= 0f)
        {
            jumpConsumed = false;

            if (isJumping)
            {
                isJumping = false;

                if (anim != null)
                    anim.SetBool("IsJumping", false);
            }
        }
    }

    void Saltar()
    {
        if (isDead || jumpConsumed)
            return;

        jumpConsumed = true;
        isJumping = true;

        // Consumir la pulsación y el margen de salto.
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;

        velocity.y = Mathf.Sqrt(
            jumpHeight * -2f * gravity
        );

        if (anim != null)
            anim.SetBool("IsJumping", true);

        Debug.Log("Salto activado. Partida iniciada: " + hasStarted);
    }

    // Iniciar la carrera desde el botón o countdown.
    public void StartRunning()
    {
        if (isDead)
            return;

        hasStarted = true;
        canMove = true;

        if (anim != null)
        {
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsJumping", isJumping);
        }

        Debug.Log("PARTIDA INICIADA");
    }

    // Detener temporalmente la carrera.
    public void StopRunning()
    {
        canMove = false;
        velocity = Vector3.zero;

        isJumping = false;
        jumpConsumed = false;
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;

        if (anim != null)
        {
            anim.SetBool("IsJumping", false);
        }
    }

    // Llegada al final del nivel.
    public void StopAtFinish()
    {
        if (isDead)
            return;

        canMove = false;
        velocity = Vector3.zero;

        isJumping = false;
        jumpConsumed = false;
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;

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

    // Detectar colisiones con obstáculos.
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
        jumpConsumed = false;
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;

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

    // Compatible con Animation Events.
    // La animación ya no controla el estado físico del salto.
    void StopJump()
    {
        // El aterrizaje se controla desde Update().
    }
}
