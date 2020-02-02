using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    //[Range (0, 10)]
    [SerializeField] private float forceInterval;

    [Space (10)]

    //[Range(100, 10000)]
    [SerializeField] private float forceStrength;
    [SerializeField] private float impulseLength;
    [SerializeField] private float calmLength;
    [SerializeField] private float currentCalmLength;
    [SerializeField] private float currentImpulseLength;
    [SerializeField] private bool impulseCalmSwitcher = true;

    private Vector2 forceStrVector;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool applyLeft = true;
    [SerializeField] private ForceMode2D fm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        applyLeft = true;
        forceStrVector = new Vector2(forceStrength, 0.0f);
        fm = ForceMode2D.Force;

        StartCoroutine(applyForce());
    }

    //private void Update()
    //{
    //    if (impulseCalmSwitcher == true)
    //    {
    //        currentImpulseLength += Time.deltaTime;
    //        if(currentImpulseLength < impulseLength)
    //        {
             
    //        }
    //        else
    //        {
    //            impulseCalmSwitcher = false;
    //            currentImpulseLength = 0.0f;
    //            if (applyLeft == true)
    //            {
    //                rb.AddForceAtPosition(forceStrVector, rb.worldCenterOfMass);
    //            }
    //            else if (applyLeft == false)
    //            {
    //                rb.AddForce(-forceStrVector * Time.deltaTime);
    //            }
    //        }
    //    }

    //    if (impulseCalmSwitcher == false)
    //    {
    //        currentCalmLength += Time.deltaTime;
    //        if (currentCalmLength < impulseLength)
    //        {
    //        }
    //        else
    //        {
    //            impulseCalmSwitcher = true;
    //            currentCalmLength = 0.0f;

    //        }
    //    }
    //}
   
    
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

