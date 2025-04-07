using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModificationType {
	Normal, Blur, Invert, Limit, Pixelated, Light, ShowBrightness, ShowSaturation, Darken, Saturate, HueShift
};


public class TextureModifier : MonoBehaviour
{
	public Texture2D inputTexture;
	[Tooltip("When true, the texture is modified every time an inspector value is changed")]
	public bool autoRecompute = true;
	public ModificationType patternType = 0;
	[Tooltip("When true, alpha values lower than 1 are shown by blending in the alphaColor")]
	public bool showAlpha = true;
	public Color alphaColor;

	public int limitAmount;
	public float pixelate;
	public float brightness;
	public float saturation;
	[Range(0, 360)]
	public int hueShift;

	// colors from inputTexture: 
	Color[] inputCols;
	// output texture and its colors:
	Texture2D texture = null;
	Color[] cols = null;
	void Start() {
		CreateTexture();
		Draw();
    }

	void CreateTexture() {
		Debug.Log("(Re)creating texture");
		// Read colors from this array:
		inputCols = inputTexture.GetPixels();

		// Create a texture and pass it to the material of this game object's renderer:
		Renderer rend = GetComponent<Renderer>();
		texture = new Texture2D(inputTexture.width, inputTexture.height, TextureFormat.RGBA32, false);
		rend.material.mainTexture = texture;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = FilterMode.Point; // No interpolation
		// Write colors to this array, then call
		//  texture.SetPixels(cols) and texture.Apply() to see them:
		cols = texture.GetPixels();
	}

	/// <summary>
	/// Draws a pattern given by the [pattern] type to the [cols] array, which
	/// should have size [width] * [height].
	/// </summary>
	void DrawPattern(Color[] outputColors, Color[] inputColors, int width, int height, ModificationType pattern) {
		for (int index = 0; index < width * height; index++) {
			// start with transparent black:
			Color output = new Color(0, 0, 0, 0);

			// map x and y 0..1
			int x = index % width;
			int y = index / height;

			// Used for HSV and HSB
			float max = Mathf.Max(inputColors[index].r, inputColors[index].g, inputColors[index].b);
			float min = Mathf.Min(inputColors[index].r, inputColors[index].g, inputColors[index].b);
            float diff = max - min;

            switch (pattern) {
				case ModificationType.Normal:
					output = inputColors[index];
					break;
				case ModificationType.Blur:
                    output = Blur(inputColors, x, y, width, height);
					break;
				case ModificationType.Invert:
					output = Color.white - inputColors[index];
					break;
				case ModificationType.Limit:
					float r = Mathf.Round(inputColors[index].r * limitAmount) / limitAmount;
					float g = Mathf.Round(inputColors[index].g * limitAmount) / limitAmount;
					float b = Mathf.Round(inputColors[index].b * limitAmount) / limitAmount;

					output = new Color(r, g, b);
					break;
				case ModificationType.Pixelated:
					x = (int)(Mathf.Floor(x / pixelate) * pixelate);
					y = (int)(Mathf.Floor(y / pixelate) * pixelate);
					int inpIndex = x + (y * width);
					output = inputColors[inpIndex];
					break;
				case ModificationType.Light:
					Color light = Color.yellow;
					output = inputColors[index] * light;
					break;
				case ModificationType.ShowBrightness:
					output = Color.white * max;
					break;
				case ModificationType.ShowSaturation:
					output = Color.white * (diff / max);
					break;
				case ModificationType.Darken:
					output = inputColors[index] * brightness;
					break;
				case ModificationType.Saturate:
					output = Color.LerpUnclamped(max * Color.white, inputColors[index], saturation);
					break;
				case ModificationType.HueShift:
					Color.RGBToHSV(inputColors[index], out float h, out float s, out float v);
					
					h += (float)(hueShift / 360f);
					if (h > 1) h -= 1;

					output = Color.HSVToRGB(h, s, v);
					break;
			}

			if (showAlpha) {
				outputColors[index] =
					output * output.a +
					alphaColor * (1 - output.a);
			} else {
				outputColors[index] = output;
			}
		}
	}

	void Draw() {
		if (texture.width!=inputTexture.width || texture.height!=inputTexture.height) {
			CreateTexture();
		}
		DrawPattern(cols, inputCols, texture.width, texture.height, patternType);

		texture.SetPixels(cols);
		texture.Apply();
	}

	// OnValidate is called whenever an inspector value is changed - even in edit mode!
	void OnValidate() {
		// To prevent calling Draw code in edit mode,
		// we check whether a texture has been created (in Start)
		if (texture == null || !autoRecompute) return;
		Draw();
	}

	private void Update() {
		// Control + S saves to file:
		if (Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
			var exporter = GetComponent<TextureExporter>();
			if (exporter!=null) {
				exporter.ExportTexture(texture);
			}
		}
	}

	#region convolution
	// Gaussian blur convolution matrix:
	static int[,] blur =
	{
		{1,2,1},
		{2,4,2},
		{1,2,1}
	};
	static Color ApplyConvolution(Color[] inputColors, int x, int y, int width, int height, int[,] convolution, float numerator, bool includeAlpha = false) {
		Color output = new Color(0, 0, 0, 0);
		// Assumes the convolution matrix is a 3x3 matrix:
		for (int dx = 0; dx < 3; dx++) {
			for (int dy = 0; dy < 3; dy++) {
				int inX = x + dx - 1; // From x-1 to x+1
				int inY = y + dy - 1; // From y-1 to y+1
				if (inX >= 0 && inX < width && inY >= 0 && inY < height) {
					output += inputColors[inX + inY * width] * convolution[dx, dy];
				} // out of range is implicitly considered to be black
			}
		}
		output /= numerator;
		if (!includeAlpha) output.a = 1;
		return output;
	}

	static Color Blur(Color[] inputColors, int x, int y, int width, int height) {
		return ApplyConvolution(inputColors, x, y, width, height, blur, 16, true);
	}
	#endregion
}
