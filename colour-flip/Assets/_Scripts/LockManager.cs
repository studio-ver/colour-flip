using TMPro;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public static LockManager Instance;
    [SerializeField] private TMP_Text lockLabel;
    public int Count { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

    }

    private void Start()
    {
        Count = 3;
        lockLabel.text = Count.ToString();
    }

    public void OnLockEnter()
    {
        UpdateLock(-1);
    }

    public void OnLockExit()
    {
        UpdateLock(1);
    }

    private void UpdateLock(int increment)
    {
        Count += increment;
        lockLabel.text = Count.ToString();
    }
}
