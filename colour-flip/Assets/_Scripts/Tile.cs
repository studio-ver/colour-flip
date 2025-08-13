using UnityEngine;

public class Tile : MonoBehaviour
{
    [field: SerializeField] public Vector2 Coordinate { get; private set; }
    [field: SerializeField] public TileType Type { get; private set; }
    [SerializeField] private SpriteRenderer gfxRenderer;

    public enum TileType
    {
        Yellow,
        Green
    };

    private Color yellow = new Color(0.9137254901960784f, 0.7686274509803922f, 0.41568627450980394f);
    private Color green = new Color(0.16470588235294117f, 0.615686274509804f, 0.5607843137254902f);


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
            case TileType.Yellow: gfxRenderer.color = yellow; break;
            case TileType.Green: gfxRenderer.color = green; break;
        }
    }

    ~Tile()
    {
        TileProcessor processor = GetComponent<TileProcessor>();
        if (processor != null) processor.enabled = true;
    }
}
