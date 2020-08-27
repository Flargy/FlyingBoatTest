using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10;
    [SerializeField] Transform meshTransform;

    private Vector3 lookDirection = Vector3.zero;
    private float rotationAmount = 0f;

    private Quaternion startRotationForMesh;
    [SerializeField] private Vector3 shipRotationValue;

    private Coroutine rotationRoutine;
    private Quaternion shipRotation;
    private Quaternion currentRotationTargetValue;
    private bool coroutineRunning;

    void Start()
    {
        startRotationForMesh = meshTransform.rotation;
        shipRotation = Quaternion.Euler(shipRotationValue);
    }

    // Update is called once per frame
    void Update()   
    {
        rotationAmount += Time.deltaTime * 0.05f;
       
        FaceTowardsDirection();
        transform.position += transform.forward * rotationSpeed * Time.deltaTime;

    }

    /// <summary>
    /// Thanks Hjalmar for being so awesome with your knowledge about code. I owe you my life.
    /// </summary>
    private void FaceTowardsDirection()
    {
        lookDirection = transform.position + WindManager.GetWindDirection();

        float angle = Vector3.SignedAngle(transform.forward, WindManager.GetWindDirection(), Vector3.up);

        if(angle > 5 || angle < -5)
        {
            RotateMyShip(angle);
        }
        else
        {
            if (coroutineRunning == true && transform.rotation != Quaternion.identity)
            {
                StopCoroutine(rotationRoutine);
                rotationRoutine = StartCoroutine(RotateMyMesh(Quaternion.identity));
            }
        }
    }


    void RotateMyShip(float angle)
    {

        if (angle > 0)
        {
            
            transform.RotateAround(transform.position, Vector3.up, WindManager.GetWindStrength() * Time.deltaTime);
            if (coroutineRunning == true && currentRotationTargetValue != shipRotation)
            {
                StopCoroutine(rotationRoutine);
                rotationRoutine = StartCoroutine(RotateMyMesh(shipRotation));
                currentRotationTargetValue = shipRotation;
            }
            else if(coroutineRunning == false && currentRotationTargetValue != transform.rotation)
            {
                rotationRoutine = StartCoroutine(RotateMyMesh(shipRotation));
                currentRotationTargetValue = shipRotation;
            }
        }
        else
        {
            transform.RotateAround(transform.position, Vector3.up, -WindManager.GetWindStrength() * Time.deltaTime);
            if (coroutineRunning == true && currentRotationTargetValue != Quaternion.Inverse(shipRotation))
            {
                StopCoroutine(rotationRoutine);
                rotationRoutine = StartCoroutine(RotateMyMesh(Quaternion.Inverse(shipRotation)));
            }
            else if (coroutineRunning == false && transform.rotation != currentRotationTargetValue)
            {
                rotationRoutine = StartCoroutine(RotateMyMesh(Quaternion.Inverse(shipRotation)));
            }

        }
    }

    private IEnumerator RotateMyMesh(Quaternion newRotation)
    {
        coroutineRunning = true;
        Quaternion currentRotation = meshTransform.rotation;
        float timer = 0f;
        float duration = 5f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            meshTransform.rotation = Quaternion.Slerp(currentRotation, newRotation, timer / duration);
            yield return null;
        }
        coroutineRunning = false;
        meshTransform.rotation = newRotation;
    }
   
}
