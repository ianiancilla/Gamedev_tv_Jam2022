using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Platform : MonoBehaviour
{
    // properties
    [SerializeField] float platformZDimension = 3f;

    public IObjectPool<GameObject> platformPool;



    // Start is called before the first frame update
    void Start()
    {
        RandomisePlatform();
    }


    void RandomisePlatform()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroCharacterController>() != null)
        {
            platformPool.Release(gameObject);
        }
    }

    // getters
    public float GetPlatformZDimension() { return platformZDimension; }
}
