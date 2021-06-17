using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public float zSpawn=0;
    public float tileLength = 30;
    public int nuberOfTiles = 5;
    private List<GameObject> activeTile = new List<GameObject>();

    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nuberOfTiles; i++)
        {
            if (i == 0)
                SpawnTile(0);
            else
            SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z -35> zSpawn - (nuberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeletTile();
        }
    }
    public void SpawnTile(int TileIndex)
    {
        GameObject go= Instantiate(tilePrefabs[TileIndex], transform.forward * zSpawn, transform.rotation);
        activeTile.Add(go);
        zSpawn += tileLength;
    }
    private void DeletTile()
    {
        Destroy(activeTile[0]);
        activeTile.RemoveAt(0);
    }
}
