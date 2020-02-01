using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    [Range (0, 10)]
    [SerializeField] private float forceInterval;

    [Space (10)]

    [Range(100, 10000)]
    [SerializeField] private float forceStrength;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool applyLeft;
    [SerializeField] private ForceMode2D fm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        applyLeft = true;
        fm = ForceMode2D.Force;

        StartCoroutine(applyForce ());
    }

    // Coroutine for applying force

     IEnumerator applyForce()
    {
        if (applyLeft)
        {
            rb.AddForce(-transform.right * forceStrength, fm);
            Debug.Log("Force applied to the \t \t left");
            rb.velocity = Vector3.zero;
            applyLeft = false;
            yield return new WaitForSeconds(forceInterval);
            StartCoroutine(applyForce());
        }

        if (!applyLeft)
        {
            rb.AddForce(transform.right * forceStrength, fm);
            Debug.Log("Force applied to the \t \t right");
            rb.velocity = Vector3.zero;
            applyLeft = true;
            yield return new WaitForSeconds(forceInterval);
            StartCoroutine(applyForce());
        }

        }

    }

