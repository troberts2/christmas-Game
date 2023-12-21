using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform groundCheck;
    public Transform wallCheck;
    public float enemySpeed = 2;
    public float activeRange;
    public float attackRange = 0;
    [HideInInspector]public bool canMove = false;
    [HideInInspector]public bool isRight = false;
    [HideInInspector]public bool attacking = false;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Animator animator;
    public LayerMask groundLayer;
    [HideInInspector]public RaycastHit2D hitGround;
    [HideInInspector]public RaycastHit2D hitWall;
    [HideInInspector]public PlayerMovement pm;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        hitGround = Physics2D.Raycast(groundCheck.position, -transform.up, 1f, groundLayer);
        hitWall = Physics2D.Raycast(wallCheck.position, Vector2.left, .5f, groundLayer);
        if(Vector2.Distance(transform.position, pm.gameObject.transform.position) < activeRange && !attacking){
            canMove = true;
        }
        if(rb.velocity != Vector2.zero && !attacking){
            animator.SetTrigger("walking");
        }
        else if(!attacking){
            animator.SetTrigger("idle");
        }
    }
    private void FixedUpdate() {
        if(canMove && !attacking){
            if(hitGround.collider != false && hitWall != true){
                if(isRight){
                    rb.velocity = new Vector2(enemySpeed, rb.velocity.y);
                }else{
                    rb.velocity = new Vector2(-enemySpeed, rb.velocity.y);
                }
            } else{
                isRight = !isRight;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            } 
        }

    }

    private void Flip(){

        isRight = !isRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
