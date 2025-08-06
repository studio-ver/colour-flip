using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq.Expressions;

public class Tile : MonoBehaviour
{
    private bool disableInput = false;
    private bool disableFlip = false;
    [SerializeField] private float flipSpeed = .4f;
    [SerializeField] private Sprite face, back, disable;
    private Sprite lastSprite;
    [SerializeField] private SpriteRenderer gfxRenderer;

    private void Awake()
    {
        gfxRenderer.sprite = face;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (disableFlip == false)
            {
                lastSprite = gfxRenderer.sprite;
                gfxRenderer.sprite = disable;
                disableFlip = true;
            } else
            {
                gfxRenderer.sprite = lastSprite;
                disableFlip = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (disableInput || disableFlip)
            return;

        if (transform.rotation.x == 0)
            transform.DORotateQuaternion(Quaternion.Euler(180, 0, 0), flipSpeed);
        else
            transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), flipSpeed);

        StartCoroutine(Cooldown());
        StartCoroutine(Flip());

    }

    private IEnumerator Cooldown()
    {
        disableInput = true;
        yield return new WaitForSeconds(flipSpeed);
        disableInput = false;
    }

    private IEnumerator Flip()
    {
        yield return new WaitForSeconds(flipSpeed * .33f);
        if (gfxRenderer.sprite == face)
            gfxRenderer.sprite = back;
        else
            gfxRenderer.sprite = face;
    }
}
