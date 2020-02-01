using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBee : MonoBehaviour
{

    enum BeeTypes { repair, nail}

    [SerializeField] private BeeTypes beeType;
    [SerializeField] private FurnitureManager furnitureManager;
    [SerializeField] private GameObject beeSelector;
    [SerializeField] private GameObject canvas;
    private bool followMouse;
    private bool clickable;

    public void OnPointerEnter()
    {
        Debug.Log("OnPointerEnter called.");
        clickable = true;
    }

    public void OnPointerExit()
    {
        Debug.Log("OnPointerEnter called.");
        clickable = true;
    }

    public void OnPointerDown()
    {
        Debug.Log("OnPointerDown called.");
        if (clickable)
        {
            gameObject.transform.SetParent(canvas.transform, false);
            followMouse = true;
        }

    }

    public void OnPointerUp()
    {
        Debug.Log("OnPointerUp called.");
        followMouse = false;
        if (furnitureManager.furnitureTrigger != null)
        {
            if(beeType == BeeTypes.nail)
            {
                furnitureManager.furnitureTrigger.nailed = true;
            } 

            if(beeType == BeeTypes.repair)
            {
                furnitureManager.furnitureTrigger.repaired = true; 
            }
        }
        else
        {
            Debug.Log("Snapback");
            gameObject.transform.SetParent(beeSelector.transform, false);
        }

    }

    private void Update()
    {

        if (followMouse)
        {
            gameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,gameObject.transform.position.z);
        } 
    }
}
