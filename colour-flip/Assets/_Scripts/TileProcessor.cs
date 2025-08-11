using UnityEngine;

[ExecuteInEditMode]
public class TileProcessor : MonoBehaviour
{
    private Tile self;

    private void Awake()
    {
        if (self == null) self = GetComponent<Tile>();
    }

    private void Update()
    {
        if (self == null) return;

        self.MoveTo(self.Coordinate);
        self.SetType(self.Type);
    }
}
