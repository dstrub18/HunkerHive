using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureTrigger : MonoBehaviour
{

    [SerializeField] public FurnitureManager furnitureManager;
    [SerializeField] public float nailhealth;
    public bool nailed;
    public bool repaired;

    void OnMouseOver()
    {
        Debug.Log("Over furniture");
        furnitureManager.furnitureTrigger = this;
    }

    void OnMouseExit()
    {
        Debug.Log("Not over furniture");
        furnitureManager.furnitureTrigger = null;
    }
}
