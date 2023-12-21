using UnityEngine;

/// <summary>
/// Scale Converter of system units from real world to virtual world.
/// </summary>
public class ScaleConverter
{
    /// <summary>
    /// Scale an object' mass. 
    /// </summary>
    /// <param name="mass">Mass (in kg) of object</param>
    /// <returns>Mass scaled of a object.</returns>
    public static float ScaleMass(float mass)
    {
        return mass / (1.9885f * Mathf.Pow(10, 30)) * Mathf.Pow(10, 9);
    }

    /// <summary>
    /// Scale a length (in km).
    /// </summary>
    /// <param name="length">Length (in km).</param>
    /// <returns>Length scaled.</returns>
    public static float ScaleLength(float length)
    {
        return length / (1.495978707f * Mathf.Pow(10, 8) / 10);
    }

    /// <summary>
    /// Scale time.
    /// </summary>
    /// <param name="time">Time in seconds</param>
    /// <returns>Time scaled.</returns>
    public static float ScaleTime(float time)
    {
        return time / 86400.0f;
    }


    /// <summary>
    /// Scale celestial object towards a specific cartesian axis.
    /// </summary>
    /// <param name="radius">Length of celestial object (in km) for a specific cartesian axis</param>
    /// <returns>Celestial object scaled for a specific cartesian axis.</returns>
    public static float ScaleSingleAxisOfCelestialObject(float radius)
    {
        return radius / 6378.137f;
    }

    /// <summary>
    /// Scale a spherical object celestial
    /// </summary>
    /// <param name="equatorialRadius">Equatorial radius (in km) of spherical object celestial.</param>
    /// <param name="polarRadius">Polar radius (in km) of spherical object celestial.</param>
    /// <returns>Dimension scaled of spherical object celestial.</returns>
    public static Vector3 ScaleSphericalCelestialObject(float equatorialRadius, float polarRadius)
    {
        float equatorialRadiusScaled = ScaleSingleAxisOfCelestialObject(equatorialRadius);
        float polarRadiusScaled = ScaleSingleAxisOfCelestialObject(polarRadius);
        
        return new Vector3(equatorialRadiusScaled, polarRadiusScaled, equatorialRadiusScaled);
    }
}