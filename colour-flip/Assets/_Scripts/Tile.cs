using UnityEngine;

public class Tile : MonoBehaviour
{
    [field: SerializeField] public Vector2 Coordinate { get; private set; }
    [field: SerializeField] public TileType Type { get; private set; }
    [SerializeField] private SpriteRenderer gfxRenderer;

    public enum TileType
    {
        Red,
        Blue
    };

    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;


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
        transform.position = coordinate * (transform.localScale.x + .19f);
        Coordinate = coordinate;
    }

    public void SetType(TileType type)
    {
        Type = type;

        switch(type)
        {
            case TileType.Red: gfxRenderer.color = redColor; break;
            case TileType.Blue: gfxRenderer.color = blueColor; break;
        }
    }

    ~Tile()
    {
        TileProcessor processor = GetComponent<TileProcessor>();
        if (processor != null) processor.enabled = true;
    }
}
