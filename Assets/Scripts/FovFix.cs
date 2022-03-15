using UnityEngine;

[ExecuteAlways]
public class FovFix : MonoBehaviour
{
    public float horizontalFOV = 50f;
    private float calcVertivalFOV(float hFOVInDeg, float aspectRatio)
    {
        float hFOVInRads = hFOVInDeg * Mathf.Deg2Rad;
        float vFOVInRads = 2 * Mathf.Atan(Mathf.Tan(hFOVInRads / 2) / aspectRatio);
        float vFOV = vFOVInRads * Mathf.Rad2Deg;
        return vFOV;
    }

    void Update()
    {
        Camera.main.fieldOfView = calcVertivalFOV(horizontalFOV, Camera.main.aspect);
    }
}
