using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour
{
    private PlayerMovement pm;
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    [SerializeField] private Sprite explosion;
    private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(pm.bulletsExplode){
            transform.localScale = new Vector2(pm.explosionRadius, pm.explosionRadius);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<SpriteRenderer>().sprite = explosion;
            audioSource.clip = explosionSound;
            audioSource.Play();
            Destroy(gameObject, .5f);
        }
        else{
            Destroy(gameObject);
        }

    }
}
