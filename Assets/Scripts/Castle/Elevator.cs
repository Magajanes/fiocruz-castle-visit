using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public const float FLOOR_ONE_HEIGHT = -16.7f;
    public const float FLOOR_TWO_HEIGHT = -8.75f;
    public const float FLOOR_THREE_HEIGHT = -0.8f;
    public const float FLOOR_FOUR_HEIGHT = 9.02f;

    public const string OPEN_ONE_TRIGGER = "Open1";
    public const string OPEN_TWO_TRIGGER = "Open2";
    public const string OPEN_THREE_TRIGGGER = "Open3";
    public const string OPEN_FOUR_TRIGGER = "Open4";

    [SerializeField]
    private float speed;
    [SerializeField]
    private int currentFloor;
    [SerializeField]
    private Transform elevator;
    [SerializeField]
    private Animator animator;

    private Coroutine moveCoroutine = null;

    public static event Action OnElevatorCalled;
    public static event Action OnElevatorMoved;

    private Dictionary<int, float> floorHeights = new Dictionary<int, float>
    {
        { 1, FLOOR_ONE_HEIGHT },
        { 2, FLOOR_TWO_HEIGHT },
        { 3, FLOOR_THREE_HEIGHT },
        { 4, FLOOR_FOUR_HEIGHT }
    };

    private Dictionary<int, string> openTriggerNames = new Dictionary<int, string>
    {
        { 1, OPEN_ONE_TRIGGER },
        { 2, OPEN_TWO_TRIGGER },
        { 3, OPEN_THREE_TRIGGGER },
        { 4, OPEN_FOUR_TRIGGER }
    };

    private void Start()
    {
        InputController.OnElevatorButtonPress += GoToFloor;
    }

    private void OnDestroy()
    {
        InputController.OnElevatorButtonPress -= GoToFloor;
    }

    public void CallToFloor(int floor)
    {
        if (moveCoroutine != null || currentFloor == floor)
            return;
        
        moveCoroutine = StartCoroutine(MoveElevatorToFloor(floor));
        OnElevatorCalled?.Invoke();
        
        IEnumerator MoveElevatorToFloor(int floorNumber)
        {
            float delta = floorHeights[floorNumber] - elevator.localPosition.z;
            float distance = Mathf.Abs(delta);
            int sign = (int)Mathf.Sign(delta);

            animator.SetTrigger("Close");
            yield return new WaitForSeconds(1);

            while (distance > 0.1f)
            {
                Vector3 position = elevator.localPosition;
                position.z += sign * speed * Time.deltaTime;
                elevator.localPosition = position;
                distance = Mathf.Abs(floorHeights[floorNumber] - elevator.localPosition.z);
                yield return null;
            }

            Vector3 finalPosition = elevator.localPosition;
            finalPosition.z = floorHeights[floorNumber];
            elevator.localPosition = finalPosition;
            currentFloor = floorNumber;

            animator.SetTrigger(openTriggerNames[floorNumber]);
            yield return new WaitForSeconds(1);
            moveCoroutine = null;
        }
    }

    public void GoToFloor(int floor)
    {
        if (moveCoroutine != null || currentFloor == floor)
            return;

        moveCoroutine = StartCoroutine(MoveElevatorToFloor(floor));
        OnElevatorMoved?.Invoke();

        IEnumerator MoveElevatorToFloor(int floorNumber)
        {
            float delta = floorHeights[floorNumber] - elevator.localPosition.z;
            float distance = Mathf.Abs(delta);
            int sign = (int)Mathf.Sign(delta);

            Movement.Instance.LockMovement(true);
            animator.SetTrigger("Close");
            yield return new WaitForSeconds(1);

            while (distance > 0.1f)
            {
                Vector3 position = elevator.localPosition;
                position.z += sign * speed * Time.deltaTime;
                elevator.localPosition = position;

                Vector3 playerPosition = Movement.Instance.transform.position;
                playerPosition.y += sign * speed * Time.deltaTime;
                Movement.Instance.transform.position = playerPosition;

                distance = Mathf.Abs(floorHeights[floorNumber] - elevator.localPosition.z);
                yield return null;
            }

            Vector3 finalPosition = elevator.localPosition;
            finalPosition.z = floorHeights[floorNumber];
            elevator.localPosition = finalPosition;
            currentFloor = floorNumber;

            Movement.Instance.LockMovement(false);

            animator.SetTrigger(openTriggerNames[floorNumber]);
            yield return new WaitForSeconds(1);
            moveCoroutine = null;
        }
    }
}
