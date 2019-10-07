using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletControl : MonoBehaviour
{
    public float power = 0.0f;
    [SerializeField]
    private float speed = 1.0f;

    private Rigidbody2D m_Rigidbody2D;
    public LayerMask Ignore;

    private float BulletLifetime;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        BulletLifetime = Time.time + 1;
    }

    void Update() {
        if (Time.time > BulletLifetime)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector2 targetVelocity = this.transform.right * speed;

        //m_Rigidbody2D.velocity = targetVelocity * Time.deltaTime;
    }
    
    public void SetDirection(Vector2 dir)
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.velocity = dir * speed;
    }

    void OnTriggerEnter2D(Collider2D col) {

        if (((1 << col.gameObject.layer) & Ignore.value) != 0)
            return;

        IDamageable other = col.gameObject.GetComponent<IDamageable>();
        if (other != null) {
            other.Damage(power);
        }

        Destroy(gameObject);
    }
}

