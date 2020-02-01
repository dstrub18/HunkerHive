using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof (Rigidbody2D))]
//[RequireComponent (typeof (Collider2D))]
//[RequireComponent (typeof (SpriteRenderer))]
public class Furniture : MonoBehaviour
{
    [Space (10)]

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D cl;
    [SerializeField] private SpriteRenderer sr;

    [Space(10)]
    [SerializeField] private float hp;

    [SerializeField] private float velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("rb assigned");
        cl = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();



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

        if (hp < 0)
        {
            Debug.Log("DIED!!!!");
            gameObject.SetActive(false);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        velocity = Mathf.Abs (rb.velocity.magnitude * Mathf.Pow (10, 1));

        hp -= velocity;
    }

    }
