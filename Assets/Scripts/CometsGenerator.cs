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
    const int MAX_INITIAL_COMETS = 5;

    /// <summary>
    /// Probability to generated a comet.
    /// </summary>
    const float PROBABILITY_GENERATE_COMET_CYCLE = 0.001f;

    /// <summary>
    /// Maximum number of comets created every PERIOD_CYCLE days.
    /// </summary>
    const int MAX_COMETS_GENERATED_CYCLE = 10;

    /// <summary>
    /// Period (in day) to create MAX_COMETS_GENERATED_CYCLE comets.
    /// </summary>
    const int PERIOD_CYCLE = 64;

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
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        if((numCometCurrCreated < MAX_COMETS_GENERATED_CYCLE) && (Random.Range(0.0f, 1.0f) <= PROBABILITY_GENERATE_COMET_CYCLE))
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
        Sun sun = GameObject.Find("Sun").GetComponent<Sun>();

        GameObject newComet = Instantiate(GameObject.FindGameObjectWithTag("GenericComet"), transform.position + Vector3.up*5, transform.rotation);
        newComet.name = "Comet " + numCometCreated + " " + isInitial;
        newComet.tag = "Untagged";
        newComet.layer = 0;

        /* ---------- Componet Comet. ---------- */
        Comet nwc = newComet.GetComponent<Comet>();

        nwc.SetOrbitalCharateristics(Random.Range(10.0f, 20.0f) * 149597887.5f, 
                                     Random.Range(0.8f, 0.95f), 
                                     Random.Range(0.0f, 180.0f), 
                                     Random.Range(0.0f, 180.0f), 
                                     Random.Range(0.0f, 20.0f), 
                                     sun);
        nwc.GenerateOrbit(isInitial);

        if (!isInitial)
        {
            Vector3 randomVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            newComet.GetComponent<Rigidbody>().velocity = -0.5f * (newComet.transform.position - sun.transform.position + randomVector).normalized;
        }
    }
}
