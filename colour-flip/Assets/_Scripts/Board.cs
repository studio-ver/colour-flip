using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Dictionary<Vector2, Tile> tiles;

    private void Awake()
    {
        tiles = new Dictionary<Vector2, Tile>();

        foreach(Tile tile in GetComponentsInChildren<Tile>())
        {
            tiles.Add(tile.GetCoordinates(), tile);
        }
    }
}
