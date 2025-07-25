using UnityEngine;
using System.Collections.Generic;

public class PlayerItemCollector : MonoBehaviour
{
    void Start()
    {
       
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Item"))
        {
            Item item = Collision.GetComponent<Item>();

            if (itemAdded)
            {
                //
            }
        }
    }
}
