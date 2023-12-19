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
    void Update()
    {
         rb.velocity = new Vector2( horizontal * speed, rb.velocity.y);

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
        Vector3 localScale = transform.localScale;
        transform.localScale = localScale;
    }

    public void Move( InputAction.CallbackContext context) {
        horizontal = context.ReadValue<Vector2>().x;
    }

}
