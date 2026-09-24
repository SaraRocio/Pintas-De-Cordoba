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
        if (charCon == null || cam == null)
            return;

        yStore = moveAmount.y;

        moveAmount =
            (cam.transform.forward * Input.GetAxisRaw("Vertical")) +
            (cam.transform.right * Input.GetAxisRaw("Horizontal"));

        moveAmount.y = 0f;
        moveAmount = moveAmount.normalized;

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

        charCon.Move(
            new Vector3(
                moveAmount.x * moveSpeed,
                moveAmount.y,
                moveAmount.z * moveSpeed
            ) * Time.deltaTime
        );

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

    public void ResetMovement()
    {
        moveAmount = Vector3.zero;

        // Detener la animación de correr
        if (anim != null)
        {
            anim.SetBool("isRunning", false);
        }
    }
}