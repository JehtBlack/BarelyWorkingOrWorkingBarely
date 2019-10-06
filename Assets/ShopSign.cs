using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSign : MonoBehaviour
{

    [SerializeField]
    public Animator SpeachBubbleAnim = null; 


    private void Update()
    {
        if (Input.GetButtonDown("Shop") && CanOpenShop)
            ShopController.Instance.ToggleShop();

    }



    private bool CanOpenShop = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            CanOpenShop = true;

            if (SpeachBubbleAnim != null)
                SpeachBubbleAnim.SetBool("In", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CanOpenShop = false;

            if (SpeachBubbleAnim != null)
                SpeachBubbleAnim.SetBool("In", false);
        }
    }
}
