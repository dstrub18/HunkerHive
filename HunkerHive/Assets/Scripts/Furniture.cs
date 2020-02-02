using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof (Rigidbody2D))]
//[RequireComponent (typeof (Collider2D))]
//[RequireComponent (typeof (SpriteRenderer))]
public class Furniture : MonoBehaviour
{
    [Space (10)]
    [SerializeField] private FurnitureManager furnitureManager;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D cl;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject explosion;
    [SerializeField] private FurnitureOutline furnitureOutline;
    [SerializeField] private GameObject nailPrefab;
    [SerializeField] private float nailHealthMax;
    [SerializeField] private GameManager gameManager;

    public bool nailedState = false;
    [Space(10)]
    [SerializeField] public float hp = 10;
    [SerializeField] public float maxHp = 10;

    private float nailHealth;
    [SerializeField] private float velocity;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        nailHealth = nailHealthMax;
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("rb assigned");
        cl = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

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

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown (KeyCode.Alpha0))
        {
            this.transform.position = new Vector3(0, 3, 1);
        }
        */

        if (nailedState)
        {
            gameObject.transform.position = furnitureOutline.transform.position;
            gameObject.transform.rotation = furnitureOutline.transform.rotation;
        }

        if (hp <= 0)
        {
            Debug.Log("DIED!!!!");
            sr.enabled = false;
            explosion.SetActive(true);
        }
        if (nailHealth <= 0 && nailedState == true)
        {
            explosion.SetActive(false);
            nailedState = false;
            nailPrefab.SetActive(false);
        }
    }

    public void Repair() 
    {

        Debug.Log("Repaired");
        hp = maxHp;
        sr.enabled = true;
        transform.position = furnitureOutline.transform.position;
    
    }

    public void Nail()
    {
        Debug.Log("Nailed");
        nailedState = true;
        nailPrefab.SetActive(true);
        gameObject.transform.position = furnitureOutline.transform.position;
        gameObject.transform.rotation = furnitureOutline.transform.rotation;


    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        velocity = Mathf.Abs (rb.velocity.magnitude * Mathf.Pow (10, 1));
        if (nailedState == false)
        {
            hp -= velocity;
        }
        else if(nailedState == true)
        {
            nailHealth -= velocity;
      
        }
    }

    }
