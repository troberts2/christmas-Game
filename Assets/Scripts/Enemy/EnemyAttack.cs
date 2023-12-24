using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Enemy
{
    protected override IEnumerator Attack(){
        attacking = true;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(1f);
        attacking = false;
    }
}
