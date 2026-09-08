using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    void StopJump()
    {
        anim.SetBool("IsJumping", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("IsJumping", true);
        }
    }
}