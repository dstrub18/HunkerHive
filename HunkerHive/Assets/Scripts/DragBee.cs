using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragBee : MonoBehaviour
{

    public enum BeeTypes { repair, nail}

    [SerializeField] public BeeTypes beeType;
    [SerializeField] public FurnitureManager furnitureManager;
    [SerializeField] public GameObject beeSelector;
    [SerializeField] public GameObject canvas;
    [SerializeField] private Sprite repairBee;
    [SerializeField] private Sprite nailBee;
    private bool followMouse;
    private bool clickable;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        if(beeType == BeeTypes.nail)
        {
            image.sprite = nailBee;
        }
        else
        {
            image.sprite = repairBee;
        }
    }

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

            Destroy(gameObject);
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
