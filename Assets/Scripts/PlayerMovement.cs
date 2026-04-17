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
    public FMODUnity.EventReference accelerateSound;
    public FMODUnity.EventReference startSound;
    public FMODUnity.EventReference stopSound;

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

        if (currentSpeed > -maxSpeed)
        {
            currentSpeed -= decelerationRate;
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