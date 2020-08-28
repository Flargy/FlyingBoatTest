using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] Transform meshTransform;

    [SerializeField] private Vector3 shipRotationValue;

    private Coroutine rotationRoutine;
    private Quaternion shipRotation;
    private Quaternion inverseRotation;
    private Quaternion currentRotationTargetValue;
    private bool coroutineRunning;
    private Vector3 inputDirection;
    private Vector3 verticalInput = Vector3.zero;
    private float horizontalInput;

    void Start()
    {
        shipRotation = Quaternion.Euler(shipRotationValue);
        inverseRotation = Quaternion.Inverse(shipRotation);

    }

    // Update is called once per frame
    void Update()   
    {
       
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput.y = Input.GetAxis("Vertical");

        inputDirection.x = horizontalInput;

        FaceTowardsDirection();
        transform.position += (transform.forward + verticalInput )* Time.deltaTime;

        

    }

    /// <summary>
    /// Thanks Hjalmar for being so awesome with your knowledge about code. I owe you my life.
    /// </summary>
    private void FaceTowardsDirection()
    {
       
        float angle = Vector3.SignedAngle(transform.forward, WindManager.GetWind() + transform.localRotation * inputDirection, Vector3.up);

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


    void RotateMyShip(float angle)
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
