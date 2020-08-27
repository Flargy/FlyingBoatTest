using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    private static WindManager instance;

    private float windStrenght = 2f;
    private float windDuration = 0;
    private Vector3 windDirection = Vector3.zero;
    private float min = -1;
    private float max = 1;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        windDirection = Vector3.forward;
        StartNewWind();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartNewWind();
        }
    }

    // Update is called once per frame
    private void StartNewWind()
    {
        Vector3 newWindDirection = windDirection * -1;

        while (Vector3.Dot(windDirection, newWindDirection) < 0.78f)
        {
            newWindDirection = new Vector3(Random.Range(min, max), 0, Random.Range(min, max));
        }
        windDirection = newWindDirection.normalized;

    }

    public static Vector3 GetWindDirection()
    {
        return instance.windDirection;
    }

    public static float GetWindStrength()
    {

        return instance.windStrenght;
    }
}
