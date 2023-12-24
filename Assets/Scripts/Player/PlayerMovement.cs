using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
public class PlayerMovement : MonoBehaviour
{
    private bool canTalkToSanta = false;
    //input actions
    private InputAction move;
    private InputAction jump;
    private InputAction interact;
    private InputAction shoot;
    private InputAction pause;
    public Rigidbody2D rb;
    [SerializeField] private Transform Ground;
    [SerializeField] private LayerMask groundLayer;
    internal bool isPaused = false;
    [SerializeField] internal GameObject pauseMenu;
    [SerializeField] private GameObject slotHolderFood;
    private GameObject[] slotsFood;

    internal float maxHp = 100;
    internal float curHp;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject interactCanvas;

    //move
    private float horizontal;
    private float speed = 3f;
    private float jumpingPower = 10f;
    private int maxJumps = 1;
    private int numberJumpsLeft = 1;
    private float damageModifier = 1;

    //combat
    [SerializeField] private float fireRate = 2f;
    private float coolDownStamp;
    private int numberOfBullets = 1;
    internal float explosionRadius = 0f;
    internal bool bulletsExplode = false;
    [SerializeField] private SpriteRenderer gunImage;
    [SerializeField] private Transform gunPt;
    List<Quaternion> bullets;
    internal int damage = 35;


    private bool isRight = true;
    [SerializeField] private GameObject bulletPrefab;
    //refernces
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private Sprite pistolSprite;
    private InventoryManager inventoryManager;
    private SantaBehavior santaBehavior;
    private PlayerControls playerControls;
    private void Start() {
        animator = GetComponent<Animator>();
        curHp = maxHp;
        inventoryManager = GetComponent<InventoryManager>();
        audioSource = GetComponent<AudioSource>();
        santaBehavior = FindObjectOfType<SantaBehavior>();
        slotsFood = new GameObject[slotHolderFood.transform.childCount];
        for(int i = 0; i < slotHolderFood.transform.childCount; i ++){
            slotsFood[i] = slotHolderFood.transform.GetChild(i).gameObject;
        }
        SetWeaponVals();
        SetMovementVals();
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

        pause = playerControls.Player.Pause;
        pause.Enable();
        pause.performed += Pause;
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
         if(shoot.IsPressed()){
            StartCoroutine(ShootIt());
         }
        
    }

    public void Jump(InputAction.CallbackContext context){
        if (context.performed && numberJumpsLeft > 0){
            rb.velocity = new Vector2( rb.velocity.x, jumpingPower);
            animator.SetTrigger("jump");
            audioSource.clip = jumpSound;
            audioSource.Play();
            numberJumpsLeft--;
        }

        if ( context.canceled && rb.velocity.y > 0f){
            rb.velocity = new Vector2(rb.velocity.x ,rb.velocity.y * .5f);
        }
    }
    public void Shoot(InputAction.CallbackContext callbackContext){
        // if(callbackContext.performed){
        //     StartCoroutine(ShootIt());
        // }
    }

    public void Interact(InputAction.CallbackContext callbackContext){
        if(callbackContext.performed && canTalkToSanta){
            StartCoroutine(InteractWithSanty());
        }
    }

    public void Pause(InputAction.CallbackContext callbackContext){
        if(callbackContext.performed){
            PauseGame();
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
            TakeDamage(20);
        }
        if(other.gameObject.CompareTag("ground")){
            numberJumpsLeft = maxJumps; 
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Santa")){
            canTalkToSanta = true;
            interactCanvas.SetActive(true);
        }
        if(other.CompareTag("food")){
            inventoryManager.AddCollectable(other.GetComponent<food>().collectable);
            audioSource.clip = collectSound;
            audioSource.Play();
            for(int i = 0; i < slotsFood.Length; i++){
                if(i == other.GetComponent<food>().collectable.id){
                    slotsFood[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                }
            }
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Santa")){
            canTalkToSanta = false;
            interactCanvas.SetActive(false);
        }
    }
    public void TakeDamage (int dmg){
        audioSource.clip = hurtSound;
        audioSource.Play();
        curHp -= dmg * damageModifier;
        healthBar.fillAmount = curHp/maxHp;

        if (curHp <= 0 ){
            SceneManager.LoadScene("DiedScreen");
            this.enabled = false;
        }
    }

    IEnumerator InteractWithSanty(){
        yield return null;
        StartCoroutine(inventoryManager.PlayerInteractWithSanta());
    }

    IEnumerator ShootIt(){
        SetWeaponVals();
        animator.SetTrigger("shoot");
        if(Time.time > coolDownStamp){
            audioSource.clip = shootSound;
            audioSource.Play();
            coolDownStamp = Time.time + fireRate;
            if(numberOfBullets > 1){
                for(int i = 0; i < numberOfBullets; i++){
                    GameObject b = Instantiate(bulletPrefab, gunPt.position, Quaternion.identity);
                    Vector2 dir;
                    if(isRight){
                        dir = new Vector2(1, Random.Range(-.2f, .2f));
                        b.GetComponent<Rigidbody2D>().AddForce(dir * 400);
                    }else{
                        dir = new Vector2(-1, Random.Range(-.2f, .2f));
                        b.GetComponent<Rigidbody2D>().AddForce(dir * 400);
                    }
                }
            }
            else{
                GameObject b = Instantiate(bulletPrefab, gunPt.position, Quaternion.identity);
                if(isRight){
                        b.GetComponent<Rigidbody2D>().AddForce(Vector2.right* 400);
                    }else{
                        b.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 400);
                    }
            }

        }
        yield return new WaitForSeconds(.5f);
    }

    internal void SetWeaponVals(){
        fireRate = .35f;
        numberOfBullets = 1;
        explosionRadius = 0;
        bulletsExplode = false;
        if(inventoryManager.currentUpgrades[0] != null){
            gunImage.sprite = inventoryManager.currentUpgrades[0].gunSprite;  
        }
        else{
            gunImage.sprite = pistolSprite;  
        }
        if(inventoryManager.currentUpgrades[0] != null){
            switch(inventoryManager.currentUpgrades[0].itemName){
                case "Shotgun":
                    numberOfBullets += 3;
                    break;
                case "Machine Gun":
                    fireRate = .1f;
                    break;
                case "RPG":
                    explosionRadius += 1f;
                    bulletsExplode = true;
                    break;
            }
        }
        if(inventoryManager.currentUpgrades[1] != null){
            switch(inventoryManager.currentUpgrades[1].itemName){
                case "Shotgun":
                    numberOfBullets += 1;
                    break;
                case "Machine Gun":
                    fireRate = .1f;
                    break;
                case "RPG":
                    if(explosionRadius > 1){
                        explosionRadius += 2.5f;
                    }else{
                        explosionRadius += 3f;
                    }
                    
                    bulletsExplode = true;
                    break;
            }
        }

        
    }
    internal void SetMovementVals(){
        speed = 5f;
        jumpingPower = 10f;
        maxJumps = 1;
        damageModifier = 1f;
        if(inventoryManager.currentUpgrades[2] != null){
            switch(inventoryManager.currentUpgrades[2].itemName){
                case "Extra Jump":
                    maxJumps += 1;
                    numberJumpsLeft = maxJumps;
                    break;
                case "Extra Speed":
                    speed += 2;
                    break;
                case "Take Half Damage":
                    damageModifier *= .5f;
                    break;
            }
        }
        if(inventoryManager.currentUpgrades[3] != null){
            switch(inventoryManager.currentUpgrades[3].itemName){
                case "Extra Jump":
                    maxJumps += 1;
                    numberJumpsLeft = maxJumps;
                    break;
                case "Dash":
                    speed += 2;
                    break;
                case "Take Half Damage":
                    damageModifier *= .5f;
                    break;
            }
        }

        
    }

    private void PauseGame(){
        if(!isPaused){
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if(isPaused){
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }



}
