using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    //input actions
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    public Rigidbody2D rb;
    public Transform Ground;
    public LayerMask groundLayer;

    public float Maxhealth = 3;
    public float Currenthealth;
    public int Enemydmg =1;
    private float horizontal;
    private float speed = 5f;
    private float jumpingPower = 10f;
    private bool isRight = true;
    private Animator animator;
    private void Start() {
        animator = GetComponent<Animator>();
        Currenthealth = Maxhealth;
        playerControls = new PlayerInputActions();
    }
    private void OnEnable(){
        move = playerControls.Player.Move;
        move.Enable();
        move.performed += Move;
        move.canceled += Move;

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
    }
    private void OnDisable(){
        move = playerControls.Player.Move;
        move.Disable();
        move.performed += Move;
        move.canceled += Move;

        jump = playerControls.Player.Jump;
        jump.Disable();
        jump.performed += Jump;
        jump.canceled += Jump;
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
            Flip();

         }
         else if (isRight && horizontal < 0f){
            Flip();
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

    private void Flip(){

        isRight = !isRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Move( InputAction.CallbackContext context) {
        horizontal = context.ReadValue<Vector2>().x;
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Enemy")){
            TakeDamage(Enemydmg);
        }
    }
    public void TakeDamage (int dmg){
        
        Currenthealth -= dmg;

        if (Currenthealth <= 0 ){
            //die animation
            //die UI
            //reload scene
            this.enabled = false;
        }
    }



}
