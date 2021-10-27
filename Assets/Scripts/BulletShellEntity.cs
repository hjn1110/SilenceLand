using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShellEntity : MonoBehaviour
{
    public Rigidbody2D rigid;
    public float forceMax;
    public float forceMin;

    public float lifetime = 4;
    public float fadetime = 2;

    SpriteRenderer sprite;
    Material mat;
    Color initiaColour;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        mat = GetComponent<Renderer>().material;
        initiaColour = mat.color;

        Init();
    }

    void Init()
    {
        mat.color = initiaColour;
        float force = Random.Range(forceMin, forceMax);
        rigid.AddForce(transform.right * force);
        rigid.AddTorque(force);
        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    private void OnEnable()
    {
        Init();
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1 / fadetime;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initiaColour, Color.clear, percent);
            yield return null;
        }

        //Destroy(gameObject);
        sprite.enabled = false;
    }

 
}
