using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpAttack : MonoBehaviour
{
    [Header("For jump attacking")]
     private Rigidbody2D enemyRb;
     private GameObject player;
    [SerializeField] float jumpHeight;
    private bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        JumpAttack();
    }
    void JumpAttack()
    {
        float distanceFromPlayer = (player.transform.position.x - transform.position.x);
        if (isGrounded)
        {
            enemyRb.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);
        }
    }
}
