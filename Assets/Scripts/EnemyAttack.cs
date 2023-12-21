using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Enemy
{
    public override void Update()
    {
        hitGround = Physics2D.Raycast(groundCheck.position, -transform.up, 1f, groundLayer);
        hitWall = Physics2D.Raycast(wallCheck.position, Vector2.left, .5f, groundLayer);
        if(Vector2.Distance(transform.position, pm.gameObject.transform.position) < attackRange && !attacking && attackRange > 0){
            StartCoroutine(Attack());
        }
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
    protected virtual IEnumerator Attack(){
        attacking = true;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(1f);
        attacking = false;
    }
}
