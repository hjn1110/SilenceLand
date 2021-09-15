using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCreater : MonoBehaviour
{

    #region Singleton
    public static TriggerCreater instance;

    private void Awake()
    {
        instance = this;

    }
    #endregion


    public void AddTriggerComponent(GameObject target, float r)
    {
        Global global = Global.instance;
        Grid grid = global.grid;

        CircleCollider2D circle = target.AddComponent<CircleCollider2D>();
        circle.isTrigger = true;
        circle.radius = r;
        circle.offset = new Vector2(0, 0);
    }


    public void AddTriggerComponent(GameObject target, int r)
    {
        Global global = Global.instance;
        Grid grid = global.grid;

        CircleCollider2D circle = target.AddComponent<CircleCollider2D>();
        circle.isTrigger = true;
        circle.radius = r * grid.cellSize.x;
        circle.offset = new Vector2(0, 0);
    }

    public GameObject AddTriggerObject(float r, Transform parentTrans, string name)
    {
        //Global global = Global.instance;
        //Grid grid = global.grid;

        GameObject TriggerField = new GameObject(name);
        AddTriggerComponent(TriggerField, r);

        TriggerField.transform.SetParent(parentTrans);
        TriggerField.transform.localPosition = Vector3.zero;

        return TriggerField;
    }

    public GameObject AddTriggerObject(int r, Transform parentTrans, string name)
    {
        //Global global = Global.instance;
        //Grid grid = global.grid;

        GameObject TriggerField = new GameObject(name);
        AddTriggerComponent(TriggerField, r);
 
        TriggerField.transform.SetParent(parentTrans);
        TriggerField.transform.localPosition = Vector3.zero;


        return TriggerField;
    }

    public void AddRidWithTemplate1(GameObject target)
    {
        AddRigid(target,10f,5f,0.05f);
    }


    private void AddRigid(GameObject target,float mass,float drag,float angularDrag)
    {
        Rigidbody2D rigid = target.AddComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.mass = mass;
        rigid.drag = drag;
        rigid.angularDrag = angularDrag;
        rigid.gravityScale = 0;
        rigid.freezeRotation = true;
        rigid.bodyType = RigidbodyType2D.Kinematic;

    }






}
