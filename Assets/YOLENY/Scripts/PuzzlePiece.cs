using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PuzzlePiece : MonoBehaviour
{
    [Header("Posición correcta")]
    public Transform posicionCorrecta;
    public float distanciaParaEncajar = 0.5f;

    [Header("Giro")]
    public float velocidadGiro = 5f;

    private Camera camara;
    private bool arrastrando = false;
    private bool colocada = false;
    private bool girando = false;

    private Vector3 offset;
    private Plane planoMovimiento;


    void Start()
    {
        camara = Camera.main;

        // El movimiento será sobre el mismo plano donde está la ficha.
        planoMovimiento = new Plane(Vector3.forward, transform.position);
    }


    void Update()
    {
        if (colocada || girando)
            return;

        // CLICK IZQUIERDO
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = camara.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    arrastrando = true;

                    Vector3 posicionMouse = ObtenerPosicionMouse();

                    if (posicionMouse != Vector3.zero)
                    {
                        offset = transform.position - posicionMouse;
                    }
                }
            }
        }


        // ARRASTRAR
        if (arrastrando && Mouse.current.leftButton.isPressed)
        {
            Vector3 posicionMouse = ObtenerPosicionMouse();

            if (posicionMouse != Vector3.zero)
            {
                transform.position = posicionMouse + offset;
            }
        }


        // SOLTAR
        if (arrastrando && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            arrastrando = false;

            ComprobarPosicion();
        }


        // CLICK DERECHO = GIRAR
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = camara.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    StartCoroutine(GirarFicha());
                }
            }
        }
    }


    Vector3 ObtenerPosicionMouse()
    {
        Ray ray = camara.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (planoMovimiento.Raycast(ray, out float distancia))
        {
            return ray.GetPoint(distancia);
        }

        return Vector3.zero;
    }


    void ComprobarPosicion()
    {
        if (posicionCorrecta == null)
            return;

        float distancia = Vector3.Distance(
            transform.position,
            posicionCorrecta.position
        );

        if (distancia <= distanciaParaEncajar)
        {
            transform.position = posicionCorrecta.position;

            colocada = true;

            PuzzleManager.Instance.PiezaColocada();
        }
    }


    IEnumerator GirarFicha()
    {
        girando = true;

        Quaternion inicio = transform.rotation;

        Quaternion final = inicio * Quaternion.Euler(0f, 0f, 180f);

        float tiempo = 0f;

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadGiro;

            transform.rotation = Quaternion.Slerp(
                inicio,
                final,
                Mathf.Clamp01(tiempo)
            );

            yield return null;
        }

        transform.rotation = final;

        girando = false;
    }
}