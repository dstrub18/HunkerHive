using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofBounds : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OUT OF BOUNDS!");

        if (collision.GetComponent<Furniture>() != null) { collision.GetComponent<Furniture>().hp = -10; }

    }
}
