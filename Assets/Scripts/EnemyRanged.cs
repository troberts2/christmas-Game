using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyAttack
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed;

    protected override IEnumerator Attack(){
        attacking = true;
        if(!isRight){
            bulletSpeed *= -1;
        }
        else{
            Math.Abs(bulletSpeed);
        }
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletSpeed, 0), ForceMode2D.Force);
        Destroy(bullet, 5f);
        yield return new WaitForSeconds(2f);
        attacking = false;
    }
}
