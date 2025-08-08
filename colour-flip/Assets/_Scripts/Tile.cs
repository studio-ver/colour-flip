using UnityEngine;
using StudioVer.ColorFlip;
using Unity.VisualScripting;
public class Tile : MonoBehaviour
{

    [field: SerializeField] public TileState State { get; private set; }
    [SerializeField] private Color face, back;
    private TileState previousState;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Vector2 coordinates;
    [SerializeField] private Tile[] linkedTiles;
    private float scaleReference;
    private float shrinkAmount = .75f;

    private void OnEnable()
    {
        FlipButton.OnFlip += SwitchSide;
    }

    private void OnDisable()
    {
        FlipButton.OnFlip -= SwitchSide;
    }

    private void Start()
    {
        GetComponent<TileProcessor>().enabled = false;

        scaleReference = transform.localScale.x;

        previousState = State;
        SetColor();
    }

    private void OnMouseDown()
    {
        if (State == TileState.Disabled)
        {
            SetState(previousState, linkedTiles);
        }
        else
        {
            previousState = State;
            SetState(TileState.Disabled, linkedTiles);
        }
    }

    private void SwitchSide()
    {
        if (State == TileState.Disabled)
        {
            SetState(previousState);
            return;
        }


        if (State == TileState.Face)
            SetState(TileState.Back);
        else
            SetState(TileState.Face);
    }

    public void SetState(TileState target)
    {
        if (State == target)
            return;

        previousState = State;
        State = target;
        float shrink = scaleReference * shrinkAmount;
        if (State == TileState.Disabled) transform.localScale = new Vector3(shrink, shrink, shrink);
        else
        {
            transform.localScale = new Vector3(scaleReference, scaleReference, scaleReference);
            SetColor();
        }
    }

    public void SetState(TileState target, Tile[] linkedTiles)
    {
        SetState(target);

        if (linkedTiles.Length == 0)
            return;

        foreach (Tile tile in linkedTiles)
        {
            tile.SetState(target);
        }
    }

    public void SetColor()
    {
        switch (State)
        {
            case TileState.Face:
                renderer.color = face;
                break;

            case TileState.Back:
                renderer.color = back;
                break;
        }
    }

    public Vector2 GetCoordinates()
    {
        return coordinates;
    }

    ~Tile()
    {
        GetComponent<TileProcessor>().enabled = true;
    }
}
