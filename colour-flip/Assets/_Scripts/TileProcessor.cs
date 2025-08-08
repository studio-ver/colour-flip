using UnityEngine;
using StudioVer.ColorFlip;

[ExecuteInEditMode]
public class TileProcessor : MonoBehaviour
{
    [SerializeField] private Tile tile;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Update()
    {
        if (tile.State == TileState.Disabled) return;

        tile.SetColor();
    }
}