using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureOutline : MonoBehaviour
{

    [SerializeField] private Furniture linkedFurnishing;
    [SerializeField] private FurnitureManager furnitureManager;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseOver()
    {
     
            Debug.Log("Over furniture");
            furnitureManager.furnitureTrigger = linkedFurnishing;
    }

    void OnMouseExit()
    {

            Debug.Log("Not over furniture");
            furnitureManager.furnitureTrigger = null;

    }

    public void TurnOFFRB(bool command)
    {
        if (command)
        {
            Debug.Log("Destory RB");
            Destroy(rb);
        }
        else
        {
            Debug.Log("Make RB");
            rb = gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}



