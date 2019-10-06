using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSign : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetButtonDown("Shop") && CanOpenShop)
            ShopController.Instance.ToggleShop();

    }



    private bool CanOpenShop = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            CanOpenShop = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            CanOpenShop = false;
    }
}
