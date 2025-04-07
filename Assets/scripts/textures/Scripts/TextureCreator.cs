using UnityEngine;
using static UnityEditor.PlayerSettings;


public class TextureCreator : MonoBehaviour
{
	// Add your own pattern types here:
	public enum PatternType { Noise, None, GradientCircle, Mandelbrot, SinRainbow, Circle, OutlineCircle, Checkers};

	public PatternType patternType;

	const int SIZE = 1024;

	Texture2D texture = null;
	Color[] cols = null;

	public float Modifier;

	public float Wave1;
	public float Wave2;
	public float Wave3;

	public float degrees;
	void Start()
    {
		// Create a texture and pass it to the material of this game object's renderer:
		Renderer rend = GetComponent<Renderer>();
		texture = new Texture2D(SIZE, SIZE, TextureFormat.RGBA32, false);
		rend.material.mainTexture = texture;
		texture.wrapMode = TextureWrapMode.Clamp;

		Draw();
	}

	/// <summary>
	/// Returns the pixel color for texture coordinate (u,v), for a given pattern.
	/// </summary>
	Color CalculatePixelColor(float u, float v, PatternType pattern) {
		
		// Variables for circles
        Vector2 pos = new Vector2(u, v);
        Vector2 center = new Vector2(0.5f, 0.5f);
        float dist = Vector2.Distance(center, pos);

        // TODO Exercise 1: insert your own pattern creation code here:
        switch (pattern) {
			case PatternType.Noise: // white noise				
				return Random.value * Color.white;
			case PatternType.Mandelbrot:
				return Mandelbrot(3 * (u - 0.75f), 3 * (v - 0.5f));
			case PatternType.GradientCircle:
				return Color.magenta * Mathf.Clamp01(1 - (dist / 0.5f));
			case PatternType.Circle:
				if (dist <= 0.5f) return Color.magenta;
				return Color.black;
			case PatternType.OutlineCircle:
				if (dist >= 0.375f && dist <= 0.4f) return Color.magenta;
				else return Color.black;
            case PatternType.SinRainbow:
                return new Color((v <= 0.5 + 0.5f * Mathf.Cos((u * Modifier) + Wave1) ? 1 : 0), (v <= 0.5 + 0.5f * Mathf.Cos((u * Modifier) + Wave2) ? 1 : 0), (v <= 0.5 + 0.5f * Mathf.Cos((u * Modifier) + Wave3) ? 1 : 0));
			case PatternType.Checkers:
				int n = (int)(u / 0.1f);
				int m = (int)(v / 0.1f);
				return Color.white * ((n + m) % 2);
			default:
				return Color.blue; 
		}
	}

	/// <summary>
	/// Draws a pattern given by the [pattern] number to the [cols] array, which
	/// should have size [width] * [height].
	/// </summary>
	void DrawPattern(Color[] cols, int width, int height, PatternType pattern) {
		for (int index = 0; index < width * height; index++)
		{
			int x = index % width; // maps input to 0.. width-1
			int y = index / width; // maps index to 0.. height-1
			float u = (float)x / (width - 1); // go from 0 to 1 -> force float division!
			float v = (float)y / (height - 1); // go from 0 to 1 -> force float division!

            // Rotate image
            float angle = degrees * Mathf.Deg2Rad;

			Vector2 centerShift = new Vector2(0.5f, 0.5f);

			u -= centerShift.x;
			v -= centerShift.y;

            Vector2 rotated = new Vector2(u * Mathf.Cos(angle) - v * Mathf.Sin(angle), u * Mathf.Sin(angle) + v * Mathf.Cos(angle));

			u = rotated.x + centerShift.x;
			v = rotated.y + centerShift.x;

            cols[index] = CalculatePixelColor(u, v, pattern);
		}
	}

	void Draw() {
		if (cols == null) {
			cols = texture.GetPixels();
		}
		DrawPattern(cols, SIZE, SIZE, patternType);

		texture.SetPixels(cols);
		texture.Apply();
	}

	// OnValidate is called whenever an inspector value is changed - even in edit mode!
	void OnValidate() {
		// To prevent calling Draw code in edit mode,
		// we check whether a texture has been created (in Start)
		if (texture == null) return;
		Draw();
	}

	private void Update() {
		// Control + S saves to file:
		if (Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
			var exporter = GetComponent<TextureExporter>();
			if (exporter != null) {
				exporter.ExportTexture(texture);
			}
		}
	}

	#region Mandelbrot
	// Used for the Mandelbrot fractal:
	const int maxIterations = 30;
	const float escapeLengthSquared = 4;

	Color Mandelbrot(float cReal, float cImaginary) {
		int iteration = 0;

		float zReal = 0;
		float zImaginary = 0;

		while (zReal * zReal + zImaginary * zImaginary < escapeLengthSquared && iteration < maxIterations) {
			// Use Mandelbrot's magic iteration formula: z := z^2 + c 
			// (using complex number multiplication & addition - 
			//   see https://mathbitsnotebook.com/Algebra2/ComplexNumbers/CPArithmeticASM.html)
			float newZr = zReal * zReal - zImaginary * zImaginary + cReal;
			zImaginary = 2 * zReal * zImaginary + cImaginary;
			zReal = newZr;
			iteration++;
		}
		// Return a color value based on the number of iterations that were needed to "escape the circle":
		float grad = 1f * iteration / maxIterations; // between 0 and 1
		// TODO: use a nicer gradient
		return new Color(grad, grad, grad);
	}
	#endregion
}
