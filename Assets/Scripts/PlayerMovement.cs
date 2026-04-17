using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int maxSpeed = 15;
    private int accelerationRate = 1;
    private int decelerationRate = 1;
    private int currentSpeed;

    private bool acceleratePending;
    private bool deceleratePending;
    private bool turnRightPending;
    private bool turnLeftPending;

    private const float ACTION_DELAY = 0.2f;
    private const float TURN_DELAY = 0.1f;

    private void Start()
    {
        currentSpeed = 0;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !acceleratePending)
        {
            StartCoroutine(DelayedAccelerate());
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !deceleratePending)
        {
            StartCoroutine(DelayedDecelerate());
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !turnRightPending)
        {
            StartCoroutine(DelayedTurnRight());
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !turnLeftPending)
        {
            StartCoroutine(DelayedTurnLeft());
        }

        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
    }

    private IEnumerator DelayedAccelerate()
    {
        acceleratePending = true;
        yield return new WaitForSeconds(ACTION_DELAY);
        Accelerate();
        acceleratePending = false;
    }

    private IEnumerator DelayedDecelerate()
    {
        deceleratePending = true;
        yield return new WaitForSeconds(ACTION_DELAY);
        Decelerate();
        deceleratePending = false;
    }

    private IEnumerator DelayedTurnRight()
    {
        turnRightPending = true;
        yield return new WaitForSeconds(TURN_DELAY);
        TurnRight();
        turnRightPending = false;
    }

    private IEnumerator DelayedTurnLeft()
    {
        turnLeftPending = true;
        yield return new WaitForSeconds(TURN_DELAY);
        TurnLeft();
        turnLeftPending = false;
    }

    public int Accelerate()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += accelerationRate;
            PlayerManager.Instance?.ConsumeFuel(2);
        }
        return currentSpeed;
    }

    public int Decelerate()
    {
        if (currentSpeed > -maxSpeed)
        {
            currentSpeed -= decelerationRate;
            PlayerManager.Instance?.ConsumeFuel(2);
        }
        return currentSpeed;
    }

    public void TurnRight()
    {
        transform.Rotate(0, 0, -10);
        PlayerManager.Instance?.ConsumeFuel(1);    
    }

    public void TurnLeft()
    {
        transform.Rotate(0, 0, 10);
        PlayerManager.Instance?.ConsumeFuel(1);
    }
}