using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int index;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<RectTransform>().DOScale(new Vector3(.9f, .9f, .9f), .1f);
        StartCoroutine(NextAfterDelay());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<RectTransform>().DOScale(new Vector3(1.1f, 1.1f, 1.1f), .3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<RectTransform>().DOScale(new Vector3(1f, 1f, 1f), .3f);
    }

    private IEnumerator NextAfterDelay()
    {
        yield return new WaitForSeconds(.1f);
        GetComponent<RectTransform>().DOScale(new Vector3(1f, 1f, 1f), .1f);
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene(index);
    }
}
