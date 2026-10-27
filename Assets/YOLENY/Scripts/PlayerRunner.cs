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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // Al comenzar, Deiva espera el inicio de la partida.
        if (anim != null)
        {
            anim.SetBool("IsDead", false);
            anim.SetBool("IsJumping", false);
        }
    }

    void Update()
    {
        if (isDead)
            return;

        if (controller == null)
            return;

        // La gravedad se mantiene aunque la partida no haya empezado.
        if (!canMove)
        {
            if (controller.isGrounded)
                velocity.y = -2f;
            else
                velocity.y += gravity * Time.deltaTime;

            controller.Move(Vector3.up * velocity.y * Time.deltaTime);
            return;
        }

        // Movimiento lateral.
        float horizontal = 0f;

        if (Keyboard.current != null)
        {
            bool moveLeft = Keyboard.current.aKey.isPressed ||
                            Keyboard.current.leftArrowKey.isPressed;

            bool moveRight = Keyboard.current.dKey.isPressed ||
                             Keyboard.current.rightArrowKey.isPressed;

            if (moveLeft && !moveRight)
                horizontal = -1f;
            else if (moveRight && !moveLeft)
                horizontal = 1f;
        }

        // Avance automático del runner.
        Vector3 move = transform.forward * forwardSpeed
                     + transform.right * horizontal * lateralSpeed;


        // Salto y gravedad.
        bool groundedBeforeMove = controller.isGrounded;

        if (groundedBeforeMove)
        {
            if (velocity.y < 0f)
                velocity.y = -2f;

            if (Keyboard.current != null &&
    Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("ESPACIO detectado. canMove: " + canMove
                    + " | En el suelo: " + controller.isGrounded);

                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

                Debug.Log("Fuerza vertical del salto: " + velocity.y);

                if (anim != null)
                    anim.SetBool("IsJumping", true);
                else
                    Debug.LogWarning("No se encontró el Animator en Deiva.");
            }
        }

        velocity.y += gravity * Time.deltaTime;
        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);

        // Actualizar la animación cuando aterriza.
        if (controller.isGrounded && velocity.y <= 0f)
        {
            if (anim != null)
                anim.SetBool("IsJumping", false);
        }
        }

    // Llamar cuando se cierre el tutorial y comience la partida.
    public void StartRunning()
    {
        if (isDead)
            return;

        canMove = true;

        Debug.Log("PARTIDA INICIADA: canMove = " + canMove);
    }

    // Detiene el movimiento durante el tutorial o una pausa.
    public void StopRunning()
    {
        canMove = false;
        velocity = Vector3.zero;

        if (anim != null)
        {
            anim.SetBool("IsJumping", false);
        }
    }

    public void StopAtFinish()
    {
        if (isDead)
            return;

        // Bloquear el movimiento del runner.
        canMove = false;
        velocity = Vector3.zero;

        // Detener la animación de salto.
        if (anim != null)
        {
            anim.SetBool("IsJumping", false);
        }

        // Mostrar el resultado del nivel.
        if (LevelCompletionManager.instance != null)
        {
            LevelCompletionManager.instance.CheckLevelCompletion();
        }
        else
        {
            Debug.LogError("No se encontró LevelCompletionManager en la escena.");
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

        if (anim != null)
        {
            anim.SetBool("IsJumping", false);
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

    // Animation Event, si lo tienes configurado.
    void StopJump()
    {
        if (anim != null)
            anim.SetBool("IsJumping", false);
    }
}