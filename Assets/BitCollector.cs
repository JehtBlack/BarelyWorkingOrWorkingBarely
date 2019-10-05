using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitCollector : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            collision.gameObject.GetComponent<ICollectible>().Collected();
        }
    }
}
