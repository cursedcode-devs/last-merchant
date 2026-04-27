using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int maxSpeed = 15;
    private int accelerationRate = 1;
    private int decelerationRate = 1;
    private int currentSpeed;
    public bool canMove = true;

    private bool acceleratePending;
    private bool deceleratePending;
    private bool turnRightPending;
    private bool turnLeftPending;

    private float dragTimer = 0f;
    private float dragInterval = 1f;

    private const float ACTION_DELAY = 0.2f;
    private const float TURN_DELAY = 0.1f;
    public FMODUnity.EventReference accelerateSound;
    public FMODUnity.EventReference startSound;
    public FMODUnity.EventReference stopSound;

    private void Start()
    {
        currentSpeed = 0;
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        if (currentSpeed < 0)
        {
            currentSpeed = 0;
            StopAllCoroutines();
        }

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !acceleratePending)
        {
            StartCoroutine(DelayedAccelerate());
        }
        else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !deceleratePending)
        {
            StartCoroutine(DelayedDecelerate());
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !turnRightPending)
        {
            StartCoroutine(DelayedTurnRight());
        }
        else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !turnLeftPending)
        {
            StartCoroutine(DelayedTurnLeft());
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            stopPlayer();
        }

        bool isAccelerating = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        if (!isAccelerating && currentSpeed > 0)
        {
            dragTimer += Time.deltaTime;
            if (dragTimer >= dragInterval)
            {
                currentSpeed -= decelerationRate;
                if (currentSpeed < 0) currentSpeed = 0;
                dragTimer = 0f;
            }
        }
        else if (isAccelerating)
        {
            dragTimer = 0f;
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

    public void stopPlayer()
    {
        StopAllCoroutines();
        currentSpeed = 0;
        acceleratePending = false;
        deceleratePending = false;
        turnRightPending = false;
        turnLeftPending = false;
    }

    public int Accelerate()
    {
        FMODUnity.RuntimeManager.PlayOneShot(accelerateSound, this.transform.position);
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += accelerationRate;
            PlayerManager.Instance?.ConsumeFuel(0.2f);
        }
        return currentSpeed;
    }

    public int Decelerate()
    {
        FMODUnity.RuntimeManager.PlayOneShot(stopSound, this.transform.position);

        if (currentSpeed > 0)
        {
            currentSpeed = currentSpeed - (2 * decelerationRate);
            if (currentSpeed < 0)
            {
                currentSpeed = 0;
                StopAllCoroutines();
            }
            PlayerManager.Instance?.ConsumeFuel(0.2f);
        }
        return currentSpeed;
    }

    public void TurnRight()
    {
        transform.Rotate(0, 0, -10);
        PlayerManager.Instance?.ConsumeFuel(0.1f);    
    }

    public void TurnLeft()
    {
        transform.Rotate(0, 0, 10);
        PlayerManager.Instance?.ConsumeFuel(0.1f);
    }
}