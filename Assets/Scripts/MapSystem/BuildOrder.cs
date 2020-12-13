using Dungeon.MapSystem;
using Dungeon.Variables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildOrder : MonoBehaviour
{
    public bool mine = false;
    public bool ground = true;
    Tilemap tilemap;
    Tilemap bgtilemap;
    public int index;

    public void Start()
    {
        tilemap = Statics.TileMapFG;
        bgtilemap = Statics.TileMapBG;
    }

    void Execute()
    {
        if (mine)
        {
            BreakTile(ground, Statics.TileMapFG.WorldToCell(gameObject.transform.position));
        }
        else
        {
            ReplaceTile(ground, Statics.TileMapFG.WorldToCell(gameObject.transform.position), newData: Statics.TileDictionary[Enum.GetName(typeof(Register.TileTypes), index)]);
        }
    }

    public IExtendedTile BreakTile(bool ground, Vector3Int pos)
    {
        IExtendedTile tile;
        if (ground)
        {
            tile = tilemap.GetTile(pos) as IExtendedTile;
        }
        else
        {
            tile = bgtilemap.GetTile(pos) as IExtendedTile;
        }
        ExtendedTileData? newTileData = null;
        if (tile != null)
        {
            newTileData = tile.TileWhenBroken;
        }

        return ReplaceTile(ground, pos, currentTile: tile, newData: newTileData);
    }

    public IExtendedTile ReplaceTile(bool ground, Vector3Int pos, IExtendedTile currentTile = null, ExtendedTileData? newData = null, IExtendedTile newTile = null)
    {
        if (newTile == null && newData?.TileToUse != null)
        {
            newTile = Instantiate(newData.Value.TileToUse.AsTile) as IExtendedTile;
        }

        if (currentTile == null)
            if (ground)
            {
                currentTile = tilemap.GetTile(pos) as IExtendedTile;
            }
            else
            {
                currentTile = bgtilemap.GetTile(pos) as IExtendedTile;
            }
        if (newTile != null)
        {
            newTile.PreTileChanged(currentTile);
        }
        if (ground)
        {
            tilemap.SetTile(pos, newTile?.AsTile);
        }
        else
        {
            bgtilemap.SetTile(pos, newTile?.AsTile);
        }

        return newTile;
    }
}
