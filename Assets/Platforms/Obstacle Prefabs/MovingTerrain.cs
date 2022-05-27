using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTerrain : MonoBehaviour
{
    [SerializeField] float phaseTime = 2f;
    [SerializeField] Vector3 movement;

    Vector3 startingPos;

    public void Start()
    {
        startingPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(startingPos,
                                               startingPos + movement,
                                               (Mathf.Sin(phaseTime * Time.time) + 1.0f) / 2.0f);
    }
}
