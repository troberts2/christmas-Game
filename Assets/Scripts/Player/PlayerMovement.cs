using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private bool canTalkToSanta = false;
    //input actions
    private InputAction move;
    private InputAction jump;
    private InputAction interact;
    private InputAction shoot;
    public Rigidbody2D rb;
    [SerializeField] private Transform Ground;
    [SerializeField] private LayerMask groundLayer;

    public float Maxhealth = 3;
    public float Currenthealth;
    public int Enemydmg =1;
    private float horizontal;
    private float speed = 5f;
    private float jumpingPower = 10f;

    //combat
    [SerializeField] private float fireRate = 1f;
    private float coolDownStamp;
    private int numberOfBullets = 1;
    private float explosionRadius = 0f;
    [SerializeField] private SpriteRenderer gunImage;
    [SerializeField] private Transform gunPt;
    List<Quaternion> bullets;


    private bool isRight = true;
    [SerializeField] private GameObject bulletPrefab;
    //refernces
    private Animator animator;
    private InventoryManager inventoryManager;
    private SantaBehavior santaBehavior;
    private PlayerControls playerControls;
    private void Start() {
        animator = GetComponent<Animator>();
        Currenthealth = Maxhealth;
        inventoryManager = GetComponent<InventoryManager>();
        santaBehavior = FindObjectOfType<SantaBehavior>();
        SetWeaponVals();
    }
    private void Awake(){
        playerControls = new PlayerControls();
        coolDownStamp = Time.time;
    }
    private void OnEnable(){
        move = playerControls.Player.Move;
        move.Enable();
        move.performed += Move;
        move.canceled += Move;

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        
        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;

        shoot = playerControls.Player.Fire;
        shoot.Enable();
        shoot.performed += Shoot;
    }
    private void OnDisable(){
        move = playerControls.Player.Move;
        move.Disable();
        move.performed -= Move;
        move.canceled -= MoveCancelled;

        jump = playerControls.Player.Jump;
        jump.Disable();
        jump.performed -= Jump;
        jump.canceled -= Jump;

        interact = playerControls.Player.Interact;
        interact.Disable();

        shoot = playerControls.Player.Fire;
        shoot.Disable();
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
    public void Shoot(InputAction.CallbackContext callbackContext){
        if(callbackContext.performed){
            StartCoroutine(ShootIt());
        }
    }

    public void Interact(InputAction.CallbackContext callbackContext){
        if(callbackContext.performed && canTalkToSanta){
            StartCoroutine(InteractWithSanty());
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

    private void Move(InputAction.CallbackContext callbackContext){
        horizontal = callbackContext.ReadValue<Vector2>().x;
    }
    private void MoveCancelled(InputAction.CallbackContext callbackContext){
        horizontal = 0f;
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Enemy")){
            TakeDamage(Enemydmg);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Santa")){
            canTalkToSanta = true;
        }
        if(other.CompareTag("food")){
            inventoryManager.AddCollectable(other.GetComponent<food>().collectable);
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Santa")){
            canTalkToSanta = false;
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

    IEnumerator InteractWithSanty(){
        santaBehavior.animator.SetTrigger("walk");
        yield return new WaitForSeconds(4f);
        inventoryManager.PlayerInteractWithSanta();
    }

    IEnumerator ShootIt(){
        SetWeaponVals();
        animator.SetTrigger("shoot");
        if(Time.time > coolDownStamp){
            coolDownStamp = Time.time + fireRate;
            if(numberOfBullets > 1){
                for(int i = 0; i < numberOfBullets; i++){
                    GameObject b = Instantiate(bulletPrefab, gunPt.position, Quaternion.identity);
                    Vector2 dir;
                    if(isRight){
                        dir = new Vector2(1, Random.Range(-.2f, .3f));
                        b.GetComponent<Rigidbody2D>().AddForce(dir * 200);
                    }else{
                        dir = new Vector2(-1, Random.Range(-.2f, .3f));
                        b.GetComponent<Rigidbody2D>().AddForce(dir * 200);
                    }
                    Destroy(b, 5f);
                }
            }

        }
        yield return new WaitForSeconds(.5f);
    }

    internal void SetWeaponVals(){
        fireRate = 1f;
        numberOfBullets = 1;
        explosionRadius = 0;
        gunImage.sprite = inventoryManager.currentUpgrades[0].gunSprite;
        switch(inventoryManager.currentUpgrades[0].itemName){
            case "Shotgun":
                numberOfBullets += 3;
                break;
            case "Machine Gun":
                fireRate = .1f;
                break;
            case "RPG":
                explosionRadius += 1.5f;
                break;
        }
        switch(inventoryManager.currentUpgrades[1].itemName){
            case "Shotgun":
                numberOfBullets += 3;
                break;
            case "Machine Gun":
                fireRate = .1f;
                break;
            case "RPG":
                explosionRadius += 1.5f;
                break;
        }
        
    }



}
