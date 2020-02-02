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

    [Space]
    [Header("Audio Sources")]
    public GameObject humLoop;
    public GameObject repairDrop;
    public GameObject nailDrop;

    public AudioSource humLoopSource;
    public AudioSource repairDropSource;
    public AudioSource nailDropSource;

    private void Start()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();

        humLoop = GameObject.Find("SfxSources/humloop");
        repairDrop = GameObject.Find("SfxSources/repairBeeDrop");
        nailDrop = GameObject.Find("SfxSources/nailBeeDrop");

        humLoopSource = humLoop.GetComponent<AudioSource>();
        repairDropSource = repairDrop.GetComponent<AudioSource>();
        nailDropSource = nailDrop.GetComponent<AudioSource>();

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

            if (!humLoopSource.isPlaying)
            {
                humLoopSource.Play();
            }
        }

    }

    public void OnPointerUp()
    {
        Debug.Log("OnPointerUp called.");
        followMouse = false;
        humLoopSource.Stop();
        if (furnitureManager.furnitureTrigger != null)
        {
            if(beeType == BeeTypes.nail)
            {
                furnitureManager.furnitureTrigger.Nail();
                nailDropSource.Play();

                Debug.Log("Nail dropped");
            } 

            if(beeType == BeeTypes.repair)
            {
                furnitureManager.furnitureTrigger.Repair();
                repairDropSource.Play();
                Debug.Log("Repair dropped");
            }

            animator.SetTrigger("boom");
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
