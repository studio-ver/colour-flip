using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile[] tiles;
    [SerializeField] private GameObject solution;
    private PlayerControls playerActions;
    private bool canMove = true;

    private void OnEnable()
    {
        playerActions.Default.PEAK.started += ToggleSolutionVisibility;
        playerActions.Default.PEAK.canceled += ToggleSolutionVisibility;
    }

    private void OnDisable()
    {
        playerActions.Default.PEAK.started -= ToggleSolutionVisibility;
        playerActions.Default.PEAK.canceled -= ToggleSolutionVisibility;
        playerActions.Disable();
    }

    private void Awake()
    {
        playerActions = new PlayerControls();
    }

    private void Start()
    {
        playerActions.Enable();
        solution.SetActive(false);
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

        foreach (Tile tile in tiles)
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
        transform.DOMove(direction * .35f, .1f);
        yield return new WaitForSeconds(.1f);
        transform.DOMove(Vector2.zero, .1f);
        yield return new WaitForSeconds(.1f);
        canMove = true;
    }

    private IEnumerator RotateBoardAnimate(Vector2 direction)
    {
        canMove = false;
        transform.DORotate(direction == Vector2.right ? new Vector3(0, 0, -15) : new Vector3(0, 0, 15), .1f);
        yield return new WaitForSeconds(.1f);
        transform.DORotate(Vector2.zero, .1f);
        yield return new WaitForSeconds(.1f);
        canMove = true;
    }

    private void RotateAll(Vector2 direction)
    {
        if (canMove == false)
            return;

        StartCoroutine(RotateBoardAnimate(direction));
        foreach(Tile tile in tiles)
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


    private void ToggleSolutionVisibility(InputAction.CallbackContext context)
    {
        if (context.started) solution.SetActive(true);
        else if (context.canceled) solution.SetActive(false);
    }

    private void CheckSolution()
    {
        bool win = false;

        foreach (Tile tile in tiles)
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
        }
    }

    private bool IsSolved(Tile target)
    {
        foreach(Tile tile in solution.GetComponentsInChildren<Tile>())
        {
            if (tile.Coordinate == target.Coordinate && tile.Type == target.Type) return true;
        }

        return false;
    }
}
