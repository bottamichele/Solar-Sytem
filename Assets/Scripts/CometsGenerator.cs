using UnityEngine;

/// <summary>
/// A generator of comets for solar system.
/// </summary>
public class CometsGenerator : MonoBehaviour
{
    /* ==================================================
     * ==================== VARIABLES ===================
     * ================================================== */
    int totalCometCreated;                      //Number of comet created via script.

    /* ================================================== 
     * ==================== METHODS =====================
     * ================================================== */

    void Start()
    {
        CreateNewComet();
    }

    void Update()
    {
        
    }

    void CreateNewComet()
    {
        GameObject newComet = Instantiate(GameObject.FindGameObjectWithTag("GenericComet"));

        Comet nwc = newComet.GetComponent<Comet>();
        nwc.SetOrbitalCharateristics(1500000000, 0, 0, 0, 0, GameObject.Find("Sun").GetComponent<Sun>());
    }
}
