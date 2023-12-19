using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform Ground;
    public LayerMask groundLayer;

    private float horizontal;
    private float speed = 5f;
    private float jumpingPower = 10f;
    private bool isRight = true;
    private Animator animator;
    private void Start() {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
         rb.velocity = new Vector2( horizontal * speed, rb.velocity.y);
        if(horizontal != 0){
            animator.SetBool("isWalking", true);
        }
        else{
            animator.SetBool("isWalking", false);
        }
         if (!isRight && horizontal > 0f){
            flip();

         }
         else if (isRight && horizontal < 0f){
            flip();
         }

    }

    public void Jump(InputAction.CallbackContext context){
        if (context.performed && isGrounded()){
            rb.velocity = new Vector2( rb.velocity.x, jumpingPower);
            animator.SetTrigger("jump");
        }

        if ( context.canceled && rb.velocity.y > 0f){
            rb.velocity = new Vector2(rb.velocity.x ,rb.velocity.y * .5f);
        }
    }

    private bool isGrounded(){
        return Physics2D.OverlapCircle(Ground.position,0.1f,groundLayer);
    }

    private void flip(){
        isRight = !isRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Move( InputAction.CallbackContext context) {
        horizontal = context.ReadValue<Vector2>().x;
    }

}
