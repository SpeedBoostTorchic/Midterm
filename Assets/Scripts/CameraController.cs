using UnityEngine;

public class CameraController : MonoBehaviour
{
	public CharControl target;
	public Vector3 trackingOffset;
	public float trackingSpeed = 10f;

	private Transform thisTransform;
	private Transform cameraTransform;

	private float shakeTimer = 0f, shakeDuration = 0f;
	private float shakeFreq = 0f, shakeAmpX = 0f, shakeAmpY = 0f, shakeAmpZ = 0f;
	private float noiseOffsetX = 0f, noiseOffsetY = 0f, noiseOffsetZ = 0f;

	void Start()
	{
		// Store local component references
		thisTransform = GetComponent<Transform>();
		cameraTransform = GetComponentInChildren<Camera>().GetComponent<Transform>();

		// Create random offsets for each rotation axis
		noiseOffsetX = Random.Range(0, 1000f);
		noiseOffsetY = Random.Range(0, 1000f);
		noiseOffsetZ = Random.Range(0, 1000f);
	}

	void Update()
	{
		// Shake the camera if necessary
		if (shakeTimer > 0f)
		{
			// Calculate the effect intensity
			var intensity = shakeTimer / shakeDuration;

			// Get values from the perlin noise function
			float noiseValueX = Mathf.PerlinNoise(Time.timeSinceLevelLoad * shakeFreq, noiseOffsetX),
			noiseValueY = Mathf.PerlinNoise(Time.timeSinceLevelLoad * shakeFreq, noiseOffsetY),
			noiseValueZ = Mathf.PerlinNoise(Time.timeSinceLevelLoad * shakeFreq, noiseOffsetZ);

			// Create new Euler angles
			float eulerX = Mathf.Lerp(-shakeAmpX * intensity, shakeAmpX * intensity, noiseValueX),
			eulerY = Mathf.Lerp(-shakeAmpY * intensity, shakeAmpY * intensity, noiseValueY),
			eulerZ = Mathf.Lerp(-shakeAmpZ * intensity, shakeAmpZ * intensity, noiseValueZ);

			// Apply new rotation
			cameraTransform.localRotation = Quaternion.Euler(eulerX, eulerY, eulerZ);

			// Decrement the shake timer and clamp at 0
			shakeTimer = Mathf.Max(shakeTimer - Time.deltaTime, 0f);
		}
	}

	public void ShakeCamera(float duration = 1f, float frequency = 15f, float amplitudeX = 2f, float amplitudeY = 2f, float amplitudeZ = 2f)
	{
		shakeTimer = shakeDuration = duration;
		shakeFreq = frequency;
		shakeAmpX = amplitudeX;
		shakeAmpY = amplitudeY;
		shakeAmpZ = amplitudeZ;
	}
}
