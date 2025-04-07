using UnityEditor.PackageManager.UI;
using UnityEngine;
public class MusicAnimation : MonoBehaviour
{
    public Material warper;
    float[] data = new float[64];
    public FFTWindow window;
    public int frequencyBoost = 50;
    void Update()
    {
        AudioListener.GetSpectrumData(data, 0, window);

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Mathf.Clamp01(data[i] * frequencyBoost);
        }

        warper.SetFloatArray("_Bars", data);
    }
}