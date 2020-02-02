using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureOutline : MonoBehaviour
{

    [SerializeField] private Furniture linkedFurnishing;
    [SerializeField] private FurnitureManager furnitureManager;

    void OnMouseOver()
    {
     
            Debug.Log("Over furniture");
            furnitureManager.furnitureTrigger = linkedFurnishing;
    }

    void OnMouseExit()
    {

            Debug.Log("Not over furniture");
            furnitureManager.furnitureTrigger = linkedFurnishing;

    }
}
