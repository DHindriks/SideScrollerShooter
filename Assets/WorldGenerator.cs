using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldGenerator : MonoBehaviour
{
    System.Random WorldGenRandom;

    public List<Tileset> tilesets;

    [SerializeField] int RDMSeed = 0;

    Tileset CurrentTileset;

    List<Tile> CurrentTiles;
    int TileAmount = 0;


    void Awake()
    {
        if (RDMSeed == 0)
        {
            RDMSeed = UnityEngine.Random.Range(0, int.MaxValue);
        }

        CurrentTiles = new List<Tile>();

        WorldGenRandom = new System.Random(RDMSeed);
        CurrentTileset = tilesets[UnityEngine.Random.Range(0, tilesets.Count)];

        InvokeRepeating("SpawnTile", 0, 5);

    }

    void SpawnTile()
    {
        Vector3 NewPos = Vector3.zero;
        Tile TileToSpawn = CurrentTileset.Tiles[WorldGenRandom.Next(CurrentTileset.Tiles.Count)];

        if (TileAmount != 0 && CurrentTiles[TileAmount - 1] != null)
        {
            NewPos = CurrentTiles[TileAmount - 1].transform.position;
            NewPos.x += CurrentTiles[TileAmount - 1].TileSizeX / 2;
            NewPos.x += TileToSpawn.TileSizeX / 2;
        }

        GameObject NewTile = Instantiate(TileToSpawn.gameObject);
        NewTile.transform.position = NewPos;
        CurrentTiles.Add(NewTile.GetComponent<Tile>());
        TileAmount++;
    }

}

[Serializable]
public class Tileset
{
    public List<Tile> Tiles;
}