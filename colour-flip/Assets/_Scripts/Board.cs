using DG.Tweening;
using System.Collections;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    private PlayerControls playerActions;
    private bool canMove = false;
    [SerializeField] private GameObject gameGrid;
    private Tile[] gameTiles;
    [SerializeField] private GameObject solutionGrid;
    private Tile[] solutionTiles;
    private bool win = false;
    [SerializeField] private string targetScene;

    private void OnEnable()
    {
        playerActions.Default.PEAK.started += Peak;
        playerActions.Default.PEAK.canceled += Peak;
    }

    private void OnDisable()
    {
        playerActions.Default.PEAK.canceled -= Peak;
        playerActions.Default.PEAK.started -= Peak;
        playerActions.Disable();
    }

    private void Awake()
    {
        playerActions = new PlayerControls();
        gameTiles = gameGrid.GetComponentsInChildren<Tile>();
        Hide(gameTiles);
        solutionTiles = solutionGrid.GetComponentsInChildren<Tile>();
    }

    private void Start()
    {
        playerActions.Enable();
        StartCoroutine(ShowAfterDelay(gameTiles));
    }

    private void Hide(Tile[] tiles)
    {
        foreach(Tile tile in tiles)
        {
            tile.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowAfterDelay(Tile[] tiles)
    {
        yield return new WaitForSeconds(.25f);
        Show(tiles);
        yield return new WaitForSeconds(tiles.Length * .3f);
        canMove = true;
    }

    private void Show(Tile[] tiles)
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            StartCoroutine(RevealAfterDelay(tiles[i], i * .3f));
        }
    }

    private IEnumerator RevealAfterDelay(Tile tile, float delay)
    {
        yield return new WaitForSeconds(delay);
        tile.gameObject.SetActive(true);
        tile.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .3f);
        AudioManager.Sounds.PlayOnReveal();
        yield return new WaitForSeconds(.3f);
        tile.transform.DOScale(new Vector3(.9f, .9f, .9f), .3f);
    }

    private void Update()
    {
        if (playerActions.Default.UP.triggered) MoveAll(Vector2.up);
        else if (playerActions.Default.RIGHT.triggered) MoveAll(Vector2.right);
        else if (playerActions.Default.DOWN.triggered) MoveAll(Vector2.down);
        else if (playerActions.Default.LEFT.triggered) MoveAll(Vector2.left);

        if (playerActions.Default.ROTATE_LEFT.triggered) RotateAll(Vector2.left);
        else if (playerActions.Default.ROTATE_RIGHT.triggered) RotateAll(Vector2.right);
    }

    private void MoveAll(Vector2 direction)
    {
        if (canMove == false)
            return;

        StartCoroutine(MoveBoardAnimate(direction));
        Vector2 targetCoordinate;

        foreach (Tile tile in gameTiles)
        {
            targetCoordinate = tile.Coordinate + direction;

            if (targetCoordinate.x > 1) targetCoordinate.x = -1;
            else if (targetCoordinate.x < -1) targetCoordinate.x = 1;
            else if (targetCoordinate.y > 1) targetCoordinate.y = -1;
            else if (targetCoordinate.y < -1) targetCoordinate.y = 1;

            tile.MoveTo(targetCoordinate);
        }

        CheckSolution();
    }

    private IEnumerator MoveBoardAnimate(Vector2 direction)
    {
        canMove = false;
        AudioManager.Sounds.PlaySlide();
        transform.DOMove(direction * .35f, .1f);
        yield return new WaitForSeconds(.1f);
        transform.DOMove(Vector2.zero, .1f);
        yield return new WaitForSeconds(.1f);
        if (win == false) canMove = true;
    }

    private IEnumerator RotateBoardAnimate(Vector2 direction)
    {
        canMove = false;
        AudioManager.Sounds.PlayRotate();
        transform.DORotate(direction == Vector2.right ? new Vector3(0, 0, -15) : new Vector3(0, 0, 15), .1f);
        yield return new WaitForSeconds(.1f);
        transform.DORotate(Vector2.zero, .1f);
        yield return new WaitForSeconds(.1f);
        if (win == false) canMove = true;
    }

    private void RotateAll(Vector2 direction)
    {
        if (canMove == false)
            return;

        StartCoroutine(RotateBoardAnimate(direction));
        foreach(Tile tile in gameTiles)
        {
            tile.MoveTo(GetRotatePosition(tile.Coordinate, direction));
        }

        CheckSolution();
    }

    private Vector2 GetRotatePosition(Vector2 startCoordinate, Vector2 direction)
    {
        switch (startCoordinate)
        {
            case Vector2 v when v == new Vector2(-1, -1):
                if (direction == Vector2.right) return new Vector2(-1, 1);
                else return new Vector2(1, -1);

            case Vector2 v when v == new Vector2(0, -1):
                if (direction == Vector2.right) return new Vector2(-1, 0);
                else return new Vector2(1, 0);

            case Vector2 v when v == new Vector2(1, -1):
                if (direction == Vector2.right) return new Vector2(-1, -1);
                else return new Vector2(1, 1);

            case Vector2 v when v == new Vector2(-1, 0):
                if (direction == Vector2.right) return new Vector2(0, 1);
                else return new Vector2(0, -1);

            case Vector2 v when v == new Vector2(1, 0):
                if (direction == Vector2.right) return new Vector2(0, -1);
                else return new Vector2(0, 1);

            case Vector2 v when v == new Vector2(-1, 1):
                if (direction == Vector2.right) return new Vector2(1, 1);
                else return new Vector2(-1, -1);

            case Vector2 v when v == new Vector2(0, 1):
                if (direction == Vector2.right) return new Vector2(1, 0);
                else return new Vector2(-1, 0);

            case Vector2 v when v == new Vector2(1, 1):
                if (direction == Vector2.right) return new Vector2(1, -1);
                else return new Vector2(-1, 1);

            default:
                return Vector2.zero;
        }
    }

    private void CheckSolution()
    {
        foreach (Tile tile in gameTiles)
        {
            win = IsSolved(tile);

            if (win == false)
            {
                return;
            }
        }

        if (win)
        {
            AudioManager.Sounds.PlayWin();
            StartCoroutine(SetupNextScene());
        }
    }

    private IEnumerator SetupNextScene()
    {
        foreach (Tile tile in solutionTiles)
        {
            tile.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(.2f);

        foreach (Tile tile in gameTiles)
        {
            tile.transform.DOScale(new Vector3(.4f, .4f, .4f), .3f);
        }

        yield return new WaitForSeconds(.3f);

        foreach (Tile tile in gameTiles)
        {
            tile.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(.2f);

        SceneSwitcher.NextAdditiveScene();
    }

    private bool IsSolved(Tile target)
    {
        foreach(Tile tile in solutionTiles)
        {
            if (tile.Coordinate == target.Coordinate && tile.Type == target.Type) return true;
        }

        return false;
    }

    private void Peak(InputAction.CallbackContext context)
    {
        if (win)
            return;
        
        if (context.started)
        {
            AudioManager.Sounds.PlayOnPeakEnter();
            foreach (Tile tile in gameTiles)
            {
                tile.gameObject.SetActive(false);
            }

            canMove = false;
        }
        else if (context.canceled)
        {
            foreach (Tile tile in gameTiles)
            {
                tile.gameObject.SetActive(true);
            }
            
            AudioManager.Sounds.PlayOnPeakExit();
            canMove = true;
        }
    }
}
