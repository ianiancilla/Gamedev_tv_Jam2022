using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Platform : MonoBehaviour
{
    // disclaimer: works only with building blocks which are 1x1x1 unit cubes.
    // can easily be modified to do otherwise but no time now

    // properties
    //[SerializeField] Vector2Int platformDimensionInTilesXZ = new Vector2Int(3, 4);
    [SerializeField] int platformZDimension = 4;
    [SerializeField] Vector3 backCentrePosition = new Vector3(-1f, -0.5f, 0);    
                                    // TODO should tie in with lane number, no time

    [Space]
    [Header("Prefabs")]
    [SerializeField] GameObject groundRowPrefab;
    [SerializeField] GameObject[] obstacleOptions;

    // variables
    public IObjectPool<GameObject> platformPool;   //this is set to the pool that
                                                   //it gets generated in, when platform
                                                   //is created

    List<Vector3> rowsPositions = new List<Vector3>();
    List<GameObject> groundRows = new List<GameObject>();
    List<GameObject> obstacleRows = new List<GameObject>();

    [HideInInspector] public PlatformSpawner spawner;

    bool initialised = false;

    public void InitialisePlatform()
    {
        BoxCollider triggerDespawner = GetComponent<BoxCollider>(); // the box collider which
                                                                    // triggers releasing platform
                                                                    // to pool when the player
                                                                    // crosses it
        triggerDespawner.center = new Vector3(transform.position.x,
                                              transform.position.y,
                                              transform.position.z + (float)(platformZDimension) + 5f);


        InitialiseRowPositions();
        GenerateRowsGround();

        initialised = true;

        RandomisePlatform();
    }

    private void OnEnable()
    {
        RandomisePlatform();
    }

    private void InitialiseRowPositions()
    {
        for (int z = 0; z < platformZDimension; z++)
        {
            Vector3 position = new Vector3(backCentrePosition.x,
                                           backCentrePosition.y,
                                           backCentrePosition.z + z);
            rowsPositions.Add(position);
        }
    }

    private void GenerateRowsGround()
    {
        foreach (Vector3 position in rowsPositions)
        {
            GameObject newRow = CreateRowAtPosition(groundRowPrefab, position);
            groundRows.Add(newRow);
        }
    }

    private GameObject CreateRowAtPosition(GameObject prefab, Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity, this.transform);
    }

    void RandomisePlatform()
    {
        if (!initialised) { return; }
        RemoveObstacles();
        GenerateRandomObstacles();
    }

    private void RemoveObstacles()
    {
        foreach (GameObject obstacle in obstacleRows)
        {
            var newRow = CreateRowAtPosition(groundRowPrefab, obstacle.transform.position);
            groundRows.Add(newRow);
            Destroy(obstacle);
        }
        obstacleRows.Clear();
    }

    private void GenerateRandomObstacles()
    {
        for (int i = 0; i < spawner.CurrentNumObstaclesOnSpawnedPlatform; i++)
        {
            int randIndex;
            // select a random obstacle from available ones
            randIndex = UnityEngine.Random.Range(0, obstacleOptions.Length);
            GameObject obstaclePrefab = obstacleOptions[randIndex];

            // select a random row
            randIndex = UnityEngine.Random.Range(0, groundRows.Count);
            GameObject rowToRemove = groundRows[randIndex];

            // find position
            Vector3 position = rowToRemove.transform.position;

            // remove row from list and destroy it
            groundRows.Remove(rowToRemove);
            Destroy(rowToRemove);

            // create obstacle and add to list
            GameObject obstacle = CreateRowAtPosition(obstaclePrefab, position);
            obstacleRows.Add(obstacle);
        }
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
