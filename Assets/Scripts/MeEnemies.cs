using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class MeEnemies : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform Ground;
    public LayerMask groundLayer;
    private PlayerMovement pm;
    private float horizontal;
    private float speed = 3f;
    private bool isRight = true;
    private Animator animator;
   

    private void Start() {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pm = FindObjectOfType<PlayerMovement>();
        pm.Enemydmg = 1;
    }

    

    void Update()
    {
        rb.velocity = new Vector2( horizontal * speed,0);
        if(horizontal != 0){
            //animator.SetBool("isWalking", true);
        }
        else{
            //animator.SetBool("isWalking", false);
        }
         if (!isRight && horizontal > 0f){
            flip();

         }
         else if (isRight && horizontal < 0f){
            flip();
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
}
