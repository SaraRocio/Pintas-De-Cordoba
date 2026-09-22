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
    private Animator anim;

    public GameObject jumpParticle, landingParticle;

    private bool lastGrounded;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        // Buscar automáticamente los componentes
        if (charCon == null)
        {
            charCon = GetComponent<CharacterController>();
        }

        // Buscar automáticamente el Animator
        anim = GetComponent<Animator>();

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
        if (charCon == null || isDead)
            return;

        if (!charCon.isGrounded)
        {
            moveAmount.y +=
                Physics.gravity.y * gravityScale * Time.fixedDeltaTime;
        }
        else
        {
            moveAmount.y =
                Physics.gravity.y * gravityScale * Time.deltaTime;
        }
    }

    void Update()
    {
        if (charCon == null || cam == null || isDead)
            return;

        yStore = moveAmount.y;

        moveAmount =
            (cam.transform.forward * Input.GetAxisRaw("Vertical")) +
            (cam.transform.right * Input.GetAxisRaw("Horizontal"));

        moveAmount.y = 0f;
        moveAmount = moveAmount.normalized;

        // ROTACIÓN
        if (moveAmount.magnitude > 0.1f)
        {
            Quaternion newRot = Quaternion.LookRotation(moveAmount);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                newRot,
                rotateSpeed * Time.deltaTime
            );
        }

        moveAmount.y = yStore;

        // SALTO
        if (charCon.isGrounded)
        {
            if (!lastGrounded)
            {
                // landingParticle.SetActive(true);
            }

            if (Input.GetButtonDown("Jump"))
            {
                moveAmount.y = jumpForce;

                // Activar animación de salto
                if (anim != null)
                {
                    anim.SetBool("IsJumping", true);
                }

                // jumpParticle.SetActive(true);
            }
            else
            {
                if (anim != null)
                {
                    anim.SetBool("IsJumping", false);
                }
            }
        }

        lastGrounded = charCon.isGrounded;

        // MOVIMIENTO
        charCon.Move(
            new Vector3(
                moveAmount.x * moveSpeed,
                moveAmount.y,
                moveAmount.z * moveSpeed
            ) * Time.deltaTime
        );

        // VELOCIDAD DE MOVIMIENTO
        float moveVel = new Vector3(
            moveAmount.x,
            0f,
            moveAmount.z
        ).magnitude * moveSpeed;

        // ANIMACIONES
        if (anim != null)
        {
            // Correr cuando se está moviendo
            anim.SetBool("IsRunning", moveVel > 0.1f);
        }
    }

    // LLAMAR ESTE MÉTODO CUANDO DEIVA MUERA
    public void Morir()
    {
        if (isDead)
            return;

        isDead = true;

        // Detener movimiento
        moveAmount = Vector3.zero;

        // Animaciones
        if (anim != null)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsDead", true);
        }

        Debug.Log("Deiva ha muerto.");
    }

    public void ResetMovement()
    {
        moveAmount = Vector3.zero;

        if (anim != null)
        {
            anim.SetBool("IsRunning", false);
        }
    }
}