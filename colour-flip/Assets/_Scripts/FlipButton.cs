using UnityEngine;

public class FlipButton : MonoBehaviour
{
    public delegate void FlipButtonEvent();
    public static event FlipButtonEvent OnFlip;
    
    public void Flip()
    {
        if (TurnTracker.Instance.Count == 0)
            return;

        if (OnFlip == null)
            return;

        OnFlip();
        TurnTracker.Instance.NextTurn();
    }
}
