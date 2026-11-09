using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravityScale = 2f;
    public float rotateSpeed = 10f;

    [Header("Componentes")]
    public CharacterController charCon;
    public Animator anim;

    private CameraController cam;

    private Vector3 moveAmount;

    private bool lastGrounded;
    private bool isDead = false;


    void Start()
    {
        // Buscar automáticamente el CharacterController
        if (charCon == null)
        {
            charCon = GetComponent<CharacterController>();
        }

        // Buscar automáticamente el Animator
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        // Buscar cámara
        cam = Object.FindAnyObjectByType<CameraController>();

        lastGrounded = true;

        if (charCon == null)
        {
            Debug.LogError("No se encontró un CharacterController en el jugador.");
        }

        if (anim == null)
        {
            Debug.LogError("No se encontró un Animator en el jugador.");
        }
    }


    void Update()
    {
        if (charCon == null || cam == null)
            return;

        // Si está muerto, no permitir movimiento
        if (isDead)
            return;


        // =====================================================
        // MOVIMIENTO
        // =====================================================

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction =
            cam.transform.forward * vertical +
            cam.transform.right * horizontal;

        // Evitar que la cámara haga que el personaje se mueva hacia arriba/abajo
        direction.y = 0f;

        direction.Normalize();


        // =====================================================
        // ROTACIÓN
        // =====================================================

        if (direction.magnitude > 0.1f)
        {
            Quaternion newRot = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                newRot,
                rotateSpeed * Time.deltaTime
            );
        }


        // =====================================================
        // GRAVEDAD
        // =====================================================

        if (charCon.isGrounded)
        {
            // Mantener al personaje pegado al suelo
            if (moveAmount.y < 0)
            {
                moveAmount.y = -2f;
            }
        }
        else
        {
            moveAmount.y +=
                Physics.gravity.y *
                gravityScale *
                Time.deltaTime;
        }


        // =====================================================
        // SALTO
        // =====================================================

        if (charCon.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveAmount.y = jumpForce;

            if (anim != null)
            {
                anim.SetBool("IsJumping", true);
            }
        }


        // =====================================================
        // MOVIMIENTO FINAL
        // =====================================================

        moveAmount.x = direction.x * moveSpeed;
        moveAmount.z = direction.z * moveSpeed;

        charCon.Move(moveAmount * Time.deltaTime);


        // =====================================================
        // ANIMACIÓN DE CORRER
        // =====================================================

        bool isMoving = direction.magnitude > 0.1f;

        if (anim != null)
        {
            anim.SetBool("IsRunning", isMoving);
        }


        // =====================================================
        // ANIMACIÓN DE SALTO
        // =====================================================

        if (anim != null)
        {
            // Cuando vuelve al suelo, termina el salto
            if (charCon.isGrounded && !lastGrounded)
            {
                anim.SetBool("IsJumping", false);
            }
        }


        lastGrounded = charCon.isGrounded;
    }


    // =========================================================
    // MUERTE
    // =========================================================

    public void Die()
    {
        isDead = true;

        if (anim != null)
        {
            anim.SetBool("IsDead", true);
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsJumping", false);
        }
    }


    // =========================================================
    // REVIVIR / REINICIAR
    // =========================================================

    public void ResetMovement()
    {
        isDead = false;

        moveAmount = Vector3.zero;

        if (anim != null)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsDead", false);
        }
    }
}