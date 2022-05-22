using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlatformSpawner : MonoBehaviour
{
    // properties
    [SerializeField] GameObject platformPrefab;
    [SerializeField] int platformsToActivateAtOnce = 10;
    
    // variables
    public IObjectPool<GameObject> PlatformPool { get; private set; }
    private List<GameObject> activePlatforms = new List<GameObject>();


    private void Awake()
    {
        PlatformPool = new ObjectPool<GameObject>(CreatePlatformObject,
                                                  PositionPlatformOnGet,
                                                  ReleasePlatform,
                                                  null,
                                                  true,
                                                  platformsToActivateAtOnce *2,
                                                  100);

        SpawnPlatforms();
    }

    private void Update()
    {
        SpawnPlatforms();
    }
    private void SpawnPlatforms()
    {
        while (activePlatforms.Count < platformsToActivateAtOnce)
        {
            activePlatforms.Add(PlatformPool.Get());
        }
    }

    // pool methods
    private GameObject CreatePlatformObject()
    {
        GameObject platformObj = Instantiate(platformPrefab);
        platformObj.transform.parent = transform;

        return platformObj;
    }

    private void PositionPlatformOnGet(GameObject obj)
    {
        if (activePlatforms.Count == 0)
        {
            obj.transform.position = Vector3.zero;
        }
        else
        {
            var previousPlatform = activePlatforms[activePlatforms.Count - 1];
            float prevPlatformLength = previousPlatform.GetComponent<Platform>().GetPlatformZDimension();
            float zSpawn = previousPlatform.transform.position.z + prevPlatformLength;
            obj.transform.position = new Vector3(0, 0, zSpawn);
        }
    }

    public void ReleasePlatform(GameObject obj)
    {
        activePlatforms.Remove(obj);
        obj.SetActive(false);
    }

}
