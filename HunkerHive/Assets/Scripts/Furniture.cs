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
    [SerializeField] private FurnitureTrigger selfFurnitureTrigger;

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
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("rb assigned");
        cl = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        selfFurnitureTrigger = GetComponent<FurnitureTrigger>();
        nailHealth = selfFurnitureTrigger.nailhealth;
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

        nailedState = selfFurnitureTrigger.nailed;
        if(nailedState == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (hp < 0)
        {
            Debug.Log("DIED!!!!");
        }
        if (nailHealth <= 0 && nailedState == true)
        {
            nailedState = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
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
