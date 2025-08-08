using UnityEngine;
using StudioVer.ColorFlip;
using Unity.VisualScripting;
public class Tile : MonoBehaviour
{

    [SerializeField] private TileState state;
    [SerializeField] private Color face, back;
    private TileState previousState;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Vector2 coordinates;

    [SerializeField] private Tile[] linkedTiles;

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
        previousState = state;
        SetColor();
    }

    private void OnMouseDown()
    {
        if (state == TileState.Disabled)
        {
            SetState(previousState, linkedTiles);
            LockManager.Instance.OnLockExit();
        }
        else
        {
            if (LockManager.Instance.Count == 0)
                return;

            previousState = state;
            SetState(TileState.Disabled, linkedTiles);
            LockManager.Instance.OnLockEnter();
        }
    }

    private void SwitchSide()
    {
        if (state == TileState.Disabled)
        {
            SetState(previousState);
            LockManager.Instance.OnLockExit();
            return;
        }
            

        if (state == TileState.Face)
            SetState(TileState.Back);
        else
            SetState(TileState.Face);
    }

    public void SetState(TileState target)
    {
        if (state == target)
            return;

        previousState = state;
        state = target;
        if (state == TileState.Disabled) transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
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

    private void SetColor()
    {
        switch (state)
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
}
