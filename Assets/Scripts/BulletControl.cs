using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletControl : MonoBehaviour
{
    private float power = 0.0f;
    [SerializeField]
    private float speed = 1.0f;

    private Rigidbody2D m_Rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector2 targetVelocity = this.transform.right * speed;

        //m_Rigidbody2D.velocity = targetVelocity * Time.deltaTime;
    }
    
    public void SetDirection(Transform t, bool IsLeft)
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.velocity = IsLeft ? t.right * speed: -t.right * speed;
    }


}

