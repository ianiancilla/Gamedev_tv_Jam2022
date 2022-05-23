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
    [SerializeField] int numObstaclesPerPlatform = 1;
    [SerializeField] int numPlatformsWithNoObstaclesAtStart = 2;
    
    // variables
    public IObjectPool<GameObject> PlatformPool { get; private set; }
    private List<GameObject> activePlatforms = new List<GameObject>();
    public int CurrentNumObstaclesOnSpawnedPlatform { get; private set; } = 0;

    private void Awake()
    {
        PlatformPool = new ObjectPool<GameObject>(CreatePlatformObject,
                                                  PositionPlatformOnGet,
                                                  ReleasePlatform,
                                                  DestroyPlatform,
                                                  true,
                                                  platformsToActivateAtOnce,
                                                  platformsToActivateAtOnce + 2);

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
            if (activePlatforms.Count == numPlatformsWithNoObstaclesAtStart)
            {
                CurrentNumObstaclesOnSpawnedPlatform = numObstaclesPerPlatform;
            }
            activePlatforms.Add(PlatformPool.Get());
        }
    }

    // pool methods
    private GameObject CreatePlatformObject()
    {
        GameObject platformObj = Instantiate(platformPrefab);
        platformObj.transform.parent = transform;
        Platform platComponent = platformObj.GetComponent<Platform>();
        platComponent.spawner = this;
        platComponent.platformPool = PlatformPool;

        platComponent.InitialisePlatform();

        return platformObj;
    }

    private void PositionPlatformOnGet(GameObject obj)
    {
        if (!obj.activeSelf) { obj.SetActive(true); }

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

    public void DestroyPlatform(GameObject obj)
    {
        Destroy(obj.gameObject);
    }
}
