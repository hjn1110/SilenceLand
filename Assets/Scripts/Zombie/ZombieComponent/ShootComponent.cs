using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class ShootComponent : MonoBehaviour
{


    public GameObject bullet;
    public Light2D light2D;
    public GameObjectPoolManager objectPoolManager;
    public Transform player;

    IEnumerator Flash()
    {
        light2D.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        light2D.enabled = false;

    }


    void Shake()
    {
        player.position -= (bullet.transform.position-player.position) * 0.1f;
    }


    private void Start()
    {
        objectPoolManager.CreatPool<BulletPool>("BulletPool");
        objectPoolManager.CreatPool<SoundSpreadPool>("SoundPool");
        objectPoolManager.CreatPool<BulletShellPool>("AShell");

        light2D.enabled = false;

    }

    private void Update()
    {

         
        if (Input.GetMouseButton(0))
        {
            if (Time.frameCount % 1 == 0)//10帧检查一次状态。FPS=30时约1秒检查3次，60时则6次
            {
                //Vector2 MouseScrPos = Input.mousePosition;
                //Vector2 MouseWrdPos = Camera.main.ScreenToWorldPoint(MouseScrPos);

                //GetInstance("BulletPool", MouseWrdPos, 2);
                objectPoolManager.GetInstance("BulletPool", bullet.transform.position, 2);
                objectPoolManager.GetInstance("AShell", bullet.transform.position, 2);
                StopAllCoroutines();
                Shake();
                StartCoroutine(Flash());
            }


        }





    }







}
