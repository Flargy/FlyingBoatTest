using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    private static WindManager instance;

    private float windStrenght = 0;
    private float windDuration = 0;
    private Vector3 windDirection = Vector3.zero;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        windDirection = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 GetWindDirection()
    {
        return instance.windDirection;
    }
}
