using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSelector : MonoBehaviour
{
    [SerializeField] private int beeAmount;
    [SerializeField] private GameObject dragBeePrefab;
    [SerializeField] private FurnitureManager furnitureManager;
    [SerializeField] private GameObject canvas;
    [SerializeField] public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PopulateHive();
    }

    public void PopulateHive() 
    { 
        if(gameObject.transform.childCount > 0)
        {
            for(var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        for (var i = 0; i < beeAmount; i++)
        {
            GameObject newBee = Instantiate(dragBeePrefab, gameObject.transform);
            newBee.transform.SetParent(gameObject.transform);
            var dragBeeScript = newBee.GetComponent<DragBee>();
            dragBeeScript.furnitureManager = furnitureManager;
            dragBeeScript.beeSelector = gameObject;
            dragBeeScript.canvas = canvas;

            var randomInt = Random.Range(1, 10);
            if(randomInt > 5)
            {
                dragBeeScript.beeType = DragBee.BeeTypes.nail;
            }
            else
            {
                dragBeeScript.beeType = DragBee.BeeTypes.repair;
            }
        }
    }
}
