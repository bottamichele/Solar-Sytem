using UnityEngine;

/// <summary>
/// A planet.
/// </summary>
public class Planet : SphericalCelestialObject
{
    /* ================================================== 
     * ============= INSPECTOR'S PROPERTIES =============
     * ================================================== */
    [Header("Orbital charateristics")]

    [SerializeField]
    [Tooltip("Semi-axis major of orbital (in km).")]
    float semiAxisMajor;                //Semi-axis major of orbital.

    [SerializeField]
    [Tooltip("Eccentricity of orbital.")]
    float eccentricity;                 //Eccentricity of orbital.

    [SerializeField]
    [Tooltip("Inclination orbital (in degree).")]
    float inclination;                  //Inclination orbital.
}
