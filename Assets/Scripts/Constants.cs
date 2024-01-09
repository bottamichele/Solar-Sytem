using UnityEngine;

public class Constants
{
    public static float gravity = 6.674f * Mathf.Pow(10.0f, -11.0f) *
                                  Mathf.Pow(ScaleConverter.ScaleLength(0.001f), 3.0f) / ScaleConverter.ScaleMass(1) /
                                  Mathf.Pow(ScaleConverter.ScaleTime(1), 2.0f);
}
