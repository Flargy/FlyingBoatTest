using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BoatMovement : MonoBehaviour
{
    private static BoatMovement instance;
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private Transform meshTransform;
    [SerializeField] private Vector3 shipRotationValue;

    private Coroutine rotationRoutine;
    private Quaternion shipRotation;
    private Quaternion inverseRotation;
    private Quaternion currentRotationTargetValue;
    private bool coroutineRunning;
    private Vector3 inputDirection;
    private Vector3 verticalInput = Vector3.zero;
    private Vector3 windEffect;
    private float horizontalInput;
    private float horizontalTurnSpeed;
    private float lastHorizontalInputValue;
    private float currentSpeed = 1.0f;
    private float speedIncreaseInput = 0.0f;

    void Start()
    {
        shipRotation = Quaternion.Euler(shipRotationValue);
        inverseRotation = Quaternion.Inverse(shipRotation);
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()   
    {
        PlayerDirectionInput();
        FaceTowardsDirection();
        MoveShip();
    }

    private void PlayerDirectionInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput.y = Input.GetAxisRaw("Vertical");
        speedIncreaseInput = Input.GetAxisRaw("Jump");

        if (Mathf.Approximately(horizontalInput, 0.0f) == false)
        {
            horizontalTurnSpeed += horizontalInput * Time.deltaTime * 5.0f;
            horizontalTurnSpeed = Mathf.Clamp(horizontalTurnSpeed, -1.0f, 1.0f);
            lastHorizontalInputValue = horizontalInput;
        }
        else if (Mathf.Approximately(horizontalTurnSpeed, 0.0f) == false)
        {
            horizontalTurnSpeed -= lastHorizontalInputValue * Time.deltaTime * 5.0f;
            if (Mathf.Abs(horizontalTurnSpeed) < 0.1f)
            {
                horizontalTurnSpeed = 0.0f;
            }
        }

        if (Mathf.Approximately(speedIncreaseInput, 1.0f) && currentSpeed < maxSpeed)
        {
            currentSpeed += maxSpeed * 0.1f * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 1, maxSpeed);
        }
        else if(Mathf.Approximately(speedIncreaseInput, 0) && Mathf.Approximately(currentSpeed, 1.0f) == false)
        {
            currentSpeed -= maxSpeed * 0.1f * Time.deltaTime;
            if(currentSpeed < 1.0f)
            {
                currentSpeed = 1.0f;
            }
        }

        inputDirection.x = horizontalTurnSpeed;

    }

    private void MoveShip()
    {
        transform.position += (transform.forward + verticalInput) * Time.deltaTime * currentSpeed;
    }

    public static void AffecteedByWind(Vector3 wind)
    {
        instance.windEffect = wind;
    }

    /// <summary>
    /// Thanks Hjalmar for being so awesome with your knowledge about code. I owe you my life.
    /// </summary>
    private void FaceTowardsDirection()
    {
       
        float angle = Vector3.SignedAngle(transform.forward, windEffect + transform.localRotation * inputDirection, Vector3.up);

        if(angle > 5 || angle < -5)
        {
            RotateMyShip(angle);
        }
        else
        {
            if(coroutineRunning == false && currentRotationTargetValue != Quaternion.identity)
            {
                currentRotationTargetValue = Quaternion.identity;
                rotationRoutine = StartCoroutine(RotateMyMesh(currentRotationTargetValue));
            }
            if(coroutineRunning == true && currentRotationTargetValue != Quaternion.identity)
            {
                StopCoroutine(rotationRoutine);
                currentRotationTargetValue = Quaternion.identity;
                rotationRoutine = StartCoroutine(RotateMyMesh(currentRotationTargetValue));
            }

        }
    }


    private void RotateMyShip(float angle)
    {

        if (angle > 0)
        {
            
            transform.RotateAround(transform.position, Vector3.up, (WindManager.GetWindStrength() + inputDirection.magnitude * rotationSpeed) * Time.deltaTime);

            if(coroutineRunning == false && currentRotationTargetValue != shipRotation)
            {
                currentRotationTargetValue = shipRotation;

                rotationRoutine = StartCoroutine(RotateMyMesh(shipRotation));
            }
            else if (coroutineRunning == true && currentRotationTargetValue != shipRotation)
            {
                StopCoroutine(rotationRoutine);
                currentRotationTargetValue = shipRotation;

                rotationRoutine = StartCoroutine(RotateMyMesh(shipRotation));
            }

        }
        else
        {
            transform.RotateAround(transform.position, Vector3.up, (-WindManager.GetWindStrength() - inputDirection.magnitude * rotationSpeed )* Time.deltaTime);

            if(coroutineRunning == false && currentRotationTargetValue != inverseRotation)
            {
                currentRotationTargetValue = inverseRotation;
                rotationRoutine = StartCoroutine(RotateMyMesh(inverseRotation));
            }
            else if(coroutineRunning == true && currentRotationTargetValue != inverseRotation)
            {
                StopCoroutine(rotationRoutine);
                currentRotationTargetValue = inverseRotation;
                rotationRoutine = StartCoroutine(RotateMyMesh(inverseRotation));
            }
  

        }
    }

    private IEnumerator RotateMyMesh(Quaternion newRotation)
    {
        coroutineRunning = true;
        Quaternion currentRotation = meshTransform.localRotation;
        float timer = 0f;
        float duration = 5f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            meshTransform.localRotation = Quaternion.Slerp(currentRotation, newRotation, timer / duration);
            yield return null;
        }
        coroutineRunning = false;
        meshTransform.localRotation = newRotation;
    }
   
}
