using UnityEngine;
using UnityEngine.EventSystems;

public class RevealButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public GameObject board;

    private void Start()
    {
        board.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        board.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        board.SetActive(false);
    }
}
