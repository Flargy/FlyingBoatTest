using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    private static WindManager instance;

    [SerializeField] private float minimumWindDuration = 10f;
    [SerializeField] private float maximumWindDuration = 60f;

    [SerializeField] private float minimumCalmWindDuration = 30f;
    [SerializeField] private float maximumCalmWIndDuration = 120f;
    [Range(0, 100)]
    [SerializeField] private int oddsOfNoWind = 0;

    [SerializeField] private float minimumStormDuration = 30f;
    [SerializeField] private float maximumStormDuration = 100f;
    [Range(0, 100)]
    [SerializeField] private int oddsOfStorm = 0;
    [Range(40, 160)]
    [SerializeField] private int stormCooldown = 100;

    private float windStrenght = 1f;
    private float windDuration = 0;
    private Vector3 windDirection = Vector3.zero;
    private float min = -1;
    private float max = 1;
    private bool stormAllowed = true;
    private bool stormActive = false;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        windDirection = Vector3.forward;
        StartCoroutine(WeatherSwapDelay());

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
        if (StopWinds() == true)
        {
            return;

        }
       

        windDuration = Random.Range(minimumWindDuration, maximumWindDuration);
        Vector3 newWindDirection = windDirection * -1;

        while (Vector3.Dot(windDirection, newWindDirection) < 0.78f)
        {
            newWindDirection = new Vector3(Random.Range(min, max), 0, Random.Range(min, max));
        }

        windStrenght = 1.0f;
        windDirection = newWindDirection.normalized;
        
    }

    private bool StartStorm()
    {
        if (stormAllowed == false || Random.Range(minimumStormDuration, maximumStormDuration) > oddsOfStorm)
        {
            return false;
        }
        //start wind
        Debug.Log("storm");

        return true;

    }

    private bool StopWinds()
    {
        if (Random.Range(1, 101) <= oddsOfNoWind)
        {
            windStrenght = 0f;
            windDuration = Random.Range(minimumCalmWindDuration, maximumCalmWIndDuration);
            return true;
        }
        return false;
    }

    public static Vector3 GetWindDirection()
    {
        return instance.windDirection;
    }

    public static float GetWindStrength()
    {

        return instance.windStrenght;
    }

    public static Vector3 GetWind()
    {
        return instance.windDirection * instance.windStrenght;
    }


    private IEnumerator WeatherSwapDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(windDuration);
            StartNewWind();
        }
    }
}
