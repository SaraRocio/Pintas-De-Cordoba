using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce, gravityScale;
    private float yStore;

    public CharacterController charCon;

    private CameraController cam;

    private Vector3 moveAmount;

    public float rotateSpeed = 10f;

    // Animator del personaje
    public Animator anim;

    public GameObject jumpParticle, landingParticle;

    private bool lastGrounded;

    // Controlar si el jugador está muerto
    private bool isDead = false;


    // Start is called before the first frame update
    void Start()
    {
        // Buscar automáticamente los componentes si no están asignados
        if (charCon == null)
        {
            charCon = GetComponent<CharacterController>();
        }

        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        cam = Object.FindAnyObjectByType<CameraController>();
        lastGrounded = true;

        if (charCon != null)
        {
            charCon.Move(new Vector3(
                0f,
                Physics.gravity.y * gravityScale * Time.deltaTime,
                0f
            ));
        }
        else
        {
            Debug.LogError("No se encontró un CharacterController en Deiva.");
        }
    }


    private void FixedUpdate()
    {
        // Si está muerto, no seguir calculando movimiento
        if (isDead)
            return;

        if (charCon == null)
            return;

        if (!charCon.isGrounded)
        {
            moveAmount.y = moveAmount.y +
                (Physics.gravity.y * gravityScale * Time.fixedDeltaTime);
        }
        else
        {
            moveAmount.y = Physics.gravity.y * gravityScale * Time.deltaTime;
        }
    }


    void Update()
    {
        // Si está muerto, no permitir movimiento
        if (isDead)
            return;

        if (charCon == null || cam == null)
            return;

        yStore = moveAmount.y;

        moveAmount =
            (cam.transform.forward * Input.GetAxisRaw("Vertical")) +
            (cam.transform.right * Input.GetAxisRaw("Horizontal"));

        moveAmount.y = 0f;
        moveAmount = moveAmount.normalized;


        // Rotación del personaje
        if (moveAmount.magnitude > 0.1f)
        {
            if (moveAmount != Vector3.zero)
            {
                Quaternion newRot = Quaternion.LookRotation(moveAmount);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    newRot,
                    rotateSpeed * Time.deltaTime
                );
            }
        }


        moveAmount.y = yStore;


        // Salto
        if (charCon.isGrounded)
        {
            if (!lastGrounded)
            {
                // landingParticle.SetActive(true);
            }

            if (Input.GetButtonDown("Jump"))
            {
                moveAmount.y = jumpForce;

                // jumpParticle.SetActive(true);
            }
        }

        lastGrounded = charCon.isGrounded;


        // Movimiento
        charCon.Move(
            new Vector3(
                moveAmount.x * moveSpeed,
                moveAmount.y,
                moveAmount.z * moveSpeed
            ) * Time.deltaTime
        );


        // Calcular velocidad para la animación
        float moveVel = new Vector3(
            moveAmount.x,
            0f,
            moveAmount.z
        ).magnitude * moveSpeed;


        // Cambiar entre Idle y Run
        if (anim != null)
        {
            anim.SetBool("isRunning", moveVel > 0.1f);
        }

        // Otros parámetros opcionales del Animator
        // anim.SetBool("isGrounded", charCon.isGrounded);
        // anim.SetFloat("yVel", moveAmount.y);
    }


    // =========================================================
    // DETECTAR CUANDO DEIVA CHOCA CON UNA PARED
    // =========================================================

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Comprobar si el objeto tiene el Tag "Wall"
        if (hit.gameObject.CompareTag("Wall"))
        {
            Die();
        }
    }


    // =========================================================
    // MÉTODO DE MUERTE
    // =========================================================

    private void Die()
    {
        // Evitar que se ejecute varias veces
        if (isDead)
            return;

        isDead = true;

        // Detener movimiento
        moveAmount = Vector3.zero;

        // Activar animación de muerte
        if (anim != null)
        {
            anim.SetTrigger("isDead");
        }

        Debug.Log("¡Deiva murió al tocar una pared!");
    }


    public void ResetMovement()
    {
        isDead = false;

        moveAmount = Vector3.zero;

        // Detener la animación de correr
        if (anim != null)
        {
            anim.SetBool("isRunning", false);
        }
    }
}