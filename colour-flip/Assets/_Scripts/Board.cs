using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    private PlayerControls playerActions;
    private bool canMove = true;

    [SerializeField] private GameObject gameGrid;
    private Tile[] gameTiles;
    [SerializeField] private GameObject solutionGrid;
    private Tile[] solutionTiles;
    private bool win = false;

    private void OnEnable()
    {
        playerActions.Default.RESET.started += ResetScene;
        playerActions.Default.NEXT.started += NextScene;
    }

    private void OnDisable()
    {
        playerActions.Default.NEXT.started -= NextScene;
        playerActions.Default.RESET.started -= ResetScene;
        playerActions.Disable();
    }

    private void Awake()
    {
        playerActions = new PlayerControls();
        gameTiles = gameGrid.GetComponentsInChildren<Tile>();
        solutionTiles = solutionGrid.GetComponentsInChildren<Tile>();
    }

    private void Start()
    {
        playerActions.Enable();
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
            Debug.Log("Solved");
            AudioManager.Sounds.PlayWin();
        }
    }

    private bool IsSolved(Tile target)
    {
        foreach(Tile tile in solutionTiles)
        {
            if (tile.Coordinate == target.Coordinate && tile.Type == target.Type) return true;
        }

        return false;
    }

    private void ResetScene(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void NextScene(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
