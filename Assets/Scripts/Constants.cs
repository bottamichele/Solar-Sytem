using UnityEngine;

public class Constants
{
    /// <summary>
    /// Gravity costant.
    /// </summary>
    public static float gravity = 6.674f * Mathf.Pow(10.0f, -11.0f) *
                                  Mathf.Pow(ScaleConverter.ScaleLength(0.001f), 3.0f) / ScaleConverter.ScaleMass(1) /
                                  Mathf.Pow(ScaleConverter.ScaleTime(1), 2.0f);

    /// <summary>
    /// Astronomic unit (A.U.). This is a value of 1 A.U.
    /// </summary>
    public static float au = ScaleConverter.ScaleLength(149597887.5f);
}
