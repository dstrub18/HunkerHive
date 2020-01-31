using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeMover : MonoBehaviour
{
    private float angleModulator;
    [SerializeField] private float angleModulatorMin;
    [SerializeField] private float angleModulatorMax;
    [SerializeField] private float smooth;

    private Quaternion target;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angleModulator = Random.Range (angleModulatorMin, angleModulatorMax);
        target = Quaternion.Euler(0, 0, 0 + angleModulator);

        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

    }
}
