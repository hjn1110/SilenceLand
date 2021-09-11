using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 20f;
    private Global global;
    private GameObject player;
    public Rigidbody2D rigid;
    

    private void Start()
    {
        init();

        
    }

    private void init()
    {
        global = Global.instance;
        player = global.player;
    }

    private void OnEnable()
    {
        Move();
    }

    public void Move()
    {

        init();
        Vector3 dir = (transform.position - player.transform.position).normalized;
        dir.z = 0;

        StartCoroutine(Moving(dir));

    }

    IEnumerator Moving(Vector3 dir)
    {
        while (true)
        {
            //transform.position += dir * speed;
            rigid.velocity += (Vector2)dir * speed * Time.smoothDeltaTime;
            yield return null;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 16 )
        {
            Debug.Log("碰撞");
            Destroy();
        }
    }
    private void Destroy()
    {
        gameObject.SetActive(false);
    }

}
