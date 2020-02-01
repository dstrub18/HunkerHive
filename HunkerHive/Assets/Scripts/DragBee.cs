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
    private Animator animator;

    private void Start()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
        if(beeType == BeeTypes.nail)
        {
            animator.SetTrigger("nailIdle");
            image.sprite = nailBee;
        }
        else
        {
            animator.SetTrigger("repairIdle");
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
            if(beeType == BeeTypes.nail)
            {
                animator.SetTrigger("pickUpNail");
            }
            else
            {
                animator.SetTrigger("pickUpRepair");
            }
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
            if (beeType == BeeTypes.nail)
            {
                animator.SetTrigger("nailIdle");
                image.sprite = nailBee;
            }
            else
            {
                animator.SetTrigger("repairIdle");
                image.sprite = repairBee;
            }
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
