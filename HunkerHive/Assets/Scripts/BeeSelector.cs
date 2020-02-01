using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSelector : MonoBehaviour
{
    [SerializeField] private int beeAmount;
    [SerializeField] private GameObject dragBeePrefab;
    [SerializeField] private FurnitureManager furnitureManager;
    [SerializeField] private GameObject canvas;

    private void Start()
    {
        PopulateHive();
    }

    private void PopulateHive() 
    { 
        for(var i = 0; i < beeAmount; i++)
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
