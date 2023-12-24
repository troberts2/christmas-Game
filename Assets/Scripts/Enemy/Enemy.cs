using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform groundCheck;
    public Transform wallCheck;
    public float enemySpeed = 2;
    public float activeRange;
    public float attackRange = 0;

    public float maxHP = 100;
    public float curHp;
    [HideInInspector]public bool canMove = false;
    [HideInInspector]public bool isRight = false;
    [HideInInspector]public bool attacking = false;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Animator animator;
    public LayerMask groundLayer;
    public Image healthBar = null;
    [HideInInspector]public RaycastHit2D hitGround;
    [HideInInspector]public RaycastHit2D hitWall;
    [HideInInspector]public PlayerMovement pm;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        pm = FindObjectOfType<PlayerMovement>();
        healthBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        curHp = maxHP;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        hitGround = Physics2D.Raycast(groundCheck.position, -transform.up, 1f, groundLayer);
        if(isRight){
            hitWall = Physics2D.Raycast(wallCheck.position, Vector2.right, .5f, groundLayer);
        }else{
            hitWall = Physics2D.Raycast(wallCheck.position, Vector2.left, .5f, groundLayer);
        }
        if(Vector2.Distance(transform.position, pm.gameObject.transform.position) < activeRange && !attacking){
            canMove = true;
        }
        if(Vector2.Distance(transform.position, pm.gameObject.transform.position) < attackRange && !attacking){
            StartCoroutine(Attack());
        }
        if(animator.parameterCount > 0){
            if(rb.velocity != Vector2.zero && !attacking){
                animator.SetTrigger("walking"); 
            }
            else if(!attacking){
                animator.SetTrigger("idle");
            }
        }
        if(curHp == maxHP){
            healthBar.enabled = false;
        }
        else{
            healthBar.enabled = true;
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
                Flip();
            } 
        }

    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("playerBullet")){
            StartCoroutine(TakeDamage());
        }
    }

    private void Flip(){

        isRight = !isRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        transform.GetChild(0).localScale = new Vector3(-transform.GetChild(0).localScale.x, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
    }

    IEnumerator TakeDamage(){
        if(curHp > 0){
            curHp -= pm.damage;
            healthBar.fillAmount = curHp/maxHP;
            if(curHp <= 0){
                Destroy(gameObject);
            }else{
                Vector2 forceDir = gameObject.transform.position - pm.transform.position;
                forceDir.Normalize();
                forceDir *= 3;
                rb.velocity = Vector2.zero;
                rb.AddForce(forceDir, ForceMode2D.Impulse);
                yield return new WaitForSeconds(1f);
                rb.velocity = Vector2.zero;
                rb.WakeUp();
                canMove = true;
            }
        }else{
            Destroy(gameObject);
        }
        yield return null;
    }
    protected virtual IEnumerator Attack(){
        yield return null;
    }

}
