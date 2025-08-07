using UnityEngine;
using StudioVer.ColorFlip;
public class Tile : MonoBehaviour
{

    [SerializeField] private TileState state;
    [SerializeField] private Color face, back, disable;
    private TileState previousState;
    [SerializeField] private SpriteRenderer renderer;

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
        SetColor();
    }

    private void OnMouseDown()
    {
        if (state == TileState.Disabled)
            SetState(previousState);
        else
        {
            previousState = state;
            SetState(TileState.Disabled);
        }
    }

    private void SwitchSide()
    {
        if (state == TileState.Disabled)
        {
            SetState(previousState);
            return;
        }
            

        if (state == TileState.Face)
            SetState(TileState.Back);
        else
            SetState(TileState.Face);
    }

    private void SetState(TileState target)
    {
        state = target;
        SetColor();
    }

    private void SetColor()
    {
        switch (state)
        {
            case TileState.Disabled:
                renderer.color = disable;
                break;
            
            case TileState.Face:
                renderer.color = face;
                break;

            case TileState.Back:
                renderer.color = back;
                break;
        }
    }
}
