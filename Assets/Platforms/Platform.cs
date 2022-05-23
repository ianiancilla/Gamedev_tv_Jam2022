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
    [SerializeField] Vector2Int platformDimensionInTilesXZ = new Vector2Int(3, 4);
    [SerializeField] Vector3 backLeftmostTilePosition = new Vector3(-1f, -0.5f, 0);    
                                    // TODO should tie in with lane number, no time


    [SerializeField] GameObject[] obstacleOptions;
    [SerializeField] GameObject groundTilePrefab;

    
    // variables
    public IObjectPool<GameObject> platformPool;   //this is set to the pool that
                                                   //it gets generated in, when platform
                                                   //is created

    float platformZDimension;
    List<Vector3> groundTilesPositions = new List<Vector3>();
    public List<GameObject> groundTiles = new List<GameObject>();
    public List<GameObject> obstacleTiles = new List<GameObject>();

    public int NumObstacles { get; set; } = 1;

    private void Awake()
    {
        platformZDimension = platformDimensionInTilesXZ.y;

        BoxCollider triggerDespawner = GetComponent<BoxCollider>(); // the box collider which
                                                                    // triggers releasing platform
                                                                    // to pool when the player
                                                                    // crosses it
        triggerDespawner.center = new Vector3(transform.position.x,
                                              transform.position.y,
                                              transform.position.z + platformZDimension + 5f);


        InitialiseTilePositions();
        GenerateTilesGround();
        RandomisePlatform();
    }

    private void OnEnable()
    {
        RandomisePlatform();
    }

    private void InitialiseTilePositions()
    {
        for (int x = 0; x < platformDimensionInTilesXZ.x; x++)
        {
            for (int z = 0; z < platformDimensionInTilesXZ.y; z++)
            {
                Vector3 position = new Vector3(backLeftmostTilePosition.x + x,
                                               backLeftmostTilePosition.y,
                                               backLeftmostTilePosition.z + z);
                groundTilesPositions.Add(position);
            }
        }
    }

    private void GenerateTilesGround()
    {
        foreach (Vector3 position in groundTilesPositions)
        {
            GameObject newTile = CreateTileAtPosition(groundTilePrefab, position);
            groundTiles.Add(newTile);
        }
    }

    private GameObject CreateTileAtPosition(GameObject prefab, Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity, this.transform);
    }

    void RandomisePlatform()
    {
        RemoveObstacles();
        GenerateRandomObstacles();
    }

    private void RemoveObstacles()
    {
        foreach (GameObject obstacle in obstacleTiles)
        {
            CreateTileAtPosition(groundTilePrefab, obstacle.transform.position);
            Destroy(obstacle);
        }
        obstacleTiles.Clear();
    }

    private void GenerateRandomObstacles()
    {
        for (int i = 0; i < NumObstacles; i++)
        {
            int randIndex;
            // select a ranom obstacle from available ones
            randIndex = UnityEngine.Random.Range(0, obstacleOptions.Length);
            GameObject obstaclePrefab = obstacleOptions[randIndex];

            // select a random tile
            randIndex = UnityEngine.Random.Range(0, groundTiles.Count);
            GameObject tileToRemove = groundTiles[randIndex];

            // find position
            Vector3 position = tileToRemove.transform.position;

            // remove tile from list and destroy it
            groundTiles.Remove(tileToRemove);
            Destroy(tileToRemove);

            // create obstacle and add to list
            GameObject obstacle = CreateTileAtPosition(obstaclePrefab, position);
            obstacleTiles.Add(obstacle);
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
