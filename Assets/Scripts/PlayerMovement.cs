using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform trans { get { return gameObject.transform; } }
    private Vector3 pos { get { return trans.position; } }
    private Transform DirTrans { get { return Dir.transform; } }

    [HideInInspector]
    public bool movable = true;
    public Animator ani;
    public PlayerSetting setting;

    private float Vertical;
    private float Horizontal;
    private Vector2 MouseScrPos;
    private Vector2 ObjScrPos;
    private Vector3 dir;
    public GameObject Dir;
    private Vector3 normalSpeed;
    private float speed{ get{ if (Input.GetKeyDown(KeyCode.LeftShift)) { return setting.moveSpeedHolding; } else return setting.moveSpeed; } }

    private Rigidbody2D rigid{get{ return GetComponent<Rigidbody2D>(); }}

    
    void Update()
    {
        if (movable)
        {
            //手电转角控制
            MouseScrPos = Input.mousePosition;
            ObjScrPos = Camera.main.WorldToScreenPoint(pos);
            dir = (MouseScrPos - ObjScrPos).normalized;
            DirTrans.up = dir;

            //角色移动控制
            Vertical = Input.GetAxis("Vertical");
            Horizontal = Input.GetAxis("Horizontal");
            normalSpeed = (new Vector3(Horizontal, Vertical, 0)).normalized;
            if (normalSpeed != Vector3.zero)
            {
                //trans.position += normalSpeed * speed * Time.smoothDeltaTime;
                rigid.velocity += (Vector2) normalSpeed * speed * Time.smoothDeltaTime;
                //rigid.AddForce(normalSpeed * speed*100);
                //Debug.Log("速度=" + speed);

            }

            //角色屏息控制
            //切换动画以及速度
            //还需补充切换音量
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //speed = setting.moveSpeedHolding;
                //Debug.Log("Hold速度="+speed);
                ani.SetBool("Hold", true);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                //speed = setting.moveSpeed;
                ani.SetBool("Hold", false);
            }
            

            /*
            if (Vertical!=0)
            {
                trans.Translate(Vector3.up * Vertical * moveSpeed);
            }
            if (Horizontal != 0)
            {
                trans.Translate(Vector3.right * Horizontal * moveSpeed);
            }
            */
            /*
            if (Input.GetMouseButton(0))
            {
                Debug.Log("鼠标左键");
                trans.Translate(Vector3.up * moveSpeed * Time.smoothDeltaTime);
                //trans.DOMove(trans.position+dir,1,true);

            }
            */
        }
    }
    

}
