using UnityEngine;

/// <summary>
/// A generator of comets for solar system.
/// </summary>
public class CometsGenerator : MonoBehaviour
{
    /* ==================================================
     * =================== CONSTANTS ====================
     * ================================================== */
    
    /// <summary>
    /// Maximum number of comets that appear at initial scene.
    /// </summary>
    const int MAX_INITIAL_COMETS = 0;

    /// <summary>
    /// Maximum number of comets created every PERIOD_CYCLE days.
    /// </summary>
    const int MAX_COMETS_GENERATED = 0;

    /// <summary>
    /// Period (in day) to create MAX_COMETS_GENERATED comets.
    /// </summary>
    const int PERIOD_CYCLE = 365;

    /* ==================================================
     * ==================== VARIABLES ===================
     * ================================================== */
    int numCometCreated;                      //Number of comet created via script.
    float currentTime;
    int numCometCurrCreated;

    /* ================================================== 
     * ==================== METHODS =====================
     * ================================================== */

    void Start()
    {
        numCometCreated = 0;
        currentTime = 0.0f;

        //Create a few new comets.
        int numNewComets = Random.Range(0, MAX_INITIAL_COMETS);
        for(int i = 0; i < numNewComets; i++)
            CreateNewComet(true);

        CreateNewComet();
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        if((numCometCurrCreated < MAX_COMETS_GENERATED) && (Random.Range(0.0f, 1.0f) <= 0.01f))
        {
            numCometCurrCreated++;
            CreateNewComet();
        }

        if(currentTime >= ScaleConverter.ScaleTime(PERIOD_CYCLE * 86400))
        {
            currentTime = 0.0f;
            numCometCurrCreated = 0;
        }
    }

    void CreateNewComet(bool isInitial=false)
    {
        numCometCreated++;

        GameObject newComet = Instantiate(GameObject.FindGameObjectWithTag("GenericComet"), transform.position + Vector3.up*5, transform.rotation);
        newComet.name = "Comet " + numCometCreated + " " + isInitial;
        newComet.tag = "Untagged";
        newComet.layer = 0;

        /* ---------- Componet Comet. ---------- */
        Comet nwc = newComet.GetComponent<Comet>();

        //float eccentricity = Random.Range(0.8f, 0.95f);
        //float semiMajorAxis = Random.Range(0.2f, 0.5f) * 149597887.5f / (1 - eccentricity);
        float eccentricity = 0.96658f;
        float semiMajorAxis = 2.298678f * Mathf.Pow(10.0f, 9);

        nwc.SetOrbitalCharateristics(semiMajorAxis, eccentricity, Random.Range(0.0f, 90.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 30.0f), GameObject.Find("Sun").GetComponent<Sun>());
        nwc.GenerateOrbit(isInitial);

        print("Position magnitude" + newComet.transform.position.magnitude);
    }
}
