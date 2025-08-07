using UnityEngine;

public class FlipButton : MonoBehaviour
{
    public delegate void FlipButtonEvent();
    public static event FlipButtonEvent OnFlip;
    
    public void Flip()
    {
        if (OnFlip == null)
            return;

        OnFlip();
    }
}
