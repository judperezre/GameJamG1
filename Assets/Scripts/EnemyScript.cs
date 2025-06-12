using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    private Rigidbody2D enemyRb;
    private GameObject player;
    public GameObject projectilePrefab;
    private bool isDeath = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        StartCoroutine(FireProjectile());
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyRb.transform.Translate((player.transform.position - transform.position).normalized * speed * Time.deltaTime);
    }

    IEnumerator FireProjectile()
    {
        while (!isDeath) 
        {
            Instantiate(projectilePrefab,transform.position,projectilePrefab.transform.rotation);
            yield return new WaitForSeconds(3);
        
        }
    }
}
