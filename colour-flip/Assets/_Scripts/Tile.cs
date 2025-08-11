using UnityEngine;
using Slide.Variables;

public class Tile : MonoBehaviour
{
    [field: SerializeField] public Vector2 Coordinate { get; private set; }
    [field: SerializeField] public TileType Type { get; private set; }
    [SerializeField] private SpriteRenderer gfxRenderer;

    private void Awake()
    {
        TileProcessor processor = GetComponent<TileProcessor>();
        if (processor != null) processor.enabled = false;
    }

    public void Init()
    {
        MoveTo(Coordinate);
        SetType(Type);
    }

    public void MoveTo(Vector2 coordinate)
    {
        transform.position = coordinate * (transform.localScale.x + .2f);
        Coordinate = coordinate;
    }

    public void SetType(TileType type)
    {
        Type = type;

        switch(type)
        {
            case TileType.Idle: gfxRenderer.color = Color.grey; break;
            case TileType.Red: gfxRenderer.color = Color.red; break;
            case TileType.Blue: gfxRenderer.color = Color.cyan; break;
        }
    }

    ~Tile()
    {
        TileProcessor processor = GetComponent<TileProcessor>();
        if (processor != null) processor.enabled = true;
    }
}
