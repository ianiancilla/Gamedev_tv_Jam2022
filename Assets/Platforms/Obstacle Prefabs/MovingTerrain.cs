using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTerrain : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;

    // variables
    private float phase = 0;
    bool goingfwd = true; // trie if going fwd, false of going back


    private void OnEnable()
    {
        transform.localPosition = pos1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }
}
