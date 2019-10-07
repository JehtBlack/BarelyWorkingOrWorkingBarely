using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSign : MonoBehaviour
{

    [SerializeField]
    public Animator SpeachBubbleAnim = null;

    [SerializeField]
    public bool HasConverse = false;
    [SerializeField]
    public bool CanConverse = false;
    [SerializeField]
    public string[] Conversation;

    private void Update()
    {
        if (Input.GetButtonDown("Shop") && CanOpenShop) {
            if (CanConverse && !HasConverse && !SpeechController.Instance.GetOpen())
            {
                SpeechController.Instance.SpeechEnded += Instance_SpeechEnded;
                SpeechController.Instance.Open();
                SpeechController.Instance.SetDialogue(Conversation);
                SpeechController.Instance.StartDialogue();
                
            }else if(!SpeechController.Instance.GetOpen())
            {
                ShopController.Instance.ToggleShop();
                GameManagerInstance.Instance.CacheWorldState(transform.position);
            }
        }
    }

    private void Instance_SpeechEnded(bool obj)
    {
        SpeechController.Instance.SpeechEnded -= Instance_SpeechEnded;
        ShopController.Instance.ToggleShop();
        GameManagerInstance.Instance.CacheWorldState(transform.position);
        HasConverse = true;
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
