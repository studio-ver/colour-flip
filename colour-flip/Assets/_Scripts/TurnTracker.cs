using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TurnTracker : MonoBehaviour
{
    public static TurnTracker Instance;
    [SerializeField] private TMP_Text turnTracker;
    public int Count { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        Count = 4;
        turnTracker.text = Count.ToString();
    }

    public void NextTurn()
    {
        Count--;
        turnTracker.text = Count.ToString();
    }
}
