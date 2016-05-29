using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
[AddComponentMenu("Image Effects/Screen Space Ambient Occlusion")]
public class SSGIEffect : MonoBehaviour
{
	public enum SSGISamples {
		Low = 0,
		Medium = 1,
		High = 2,
	}
	
	public float m_Radius = 0.4f;
	public SSGISamples m_SampleCount = SSGISamples.Medium;
	public float m_OcclusionIntensity = 1.5f;
	public int m_Blur = 2;
	public int m_Downsampling = 2;
	public float m_OcclusionAttenuation = 1.0f;
	public float m_MinZ = 0.01f;

	public Shader m_SSGIShader;
	private Material m_SSGIMaterial;

	public Texture2D m_RandomTexture;
	
	private bool m_Supported;

	private static Material CreateMaterial (Shader shader)
	{
		if (!shader)
			return null;
		Material m = new Material (shader);
		m.hideFlags = HideFlags.HideAndDontSave;
		return m;
	}
	private static void DestroyMaterial (Material mat)
	{
		if (mat)
		{
			DestroyImmediate (mat);
			mat = null;
		}
	}
	
	
	void OnDisable()
	{
		DestroyMaterial (m_SSGIMaterial);
	}
	
	void Start()
	{
		if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Depth))
		{
			m_Supported = false;
			enabled = false;
			return;
		}
		
		CreateMaterials ();
		if (!m_SSGIMaterial || m_SSGIMaterial.passCount != 5)
		{
			m_Supported = false;
			enabled = false;
			return;
		}
		
		//CreateRandomTable (26, 0.2f);
					
		m_Supported = true;
	}
	
	void OnEnable () {
		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
	}

	private void CreateMaterials ()
	{
		if (!m_SSGIMaterial && m_SSGIShader.isSupported)
		{
			m_SSGIMaterial = CreateMaterial (m_SSGIShader);
			m_SSGIMaterial.SetTexture ("_RandomTexture", m_RandomTexture);
		}
	}
	
	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (!m_Supported || !m_SSGIShader.isSupported) {
			enabled = false;
			return;
		}
		CreateMaterials ();

		m_Downsampling = Mathf.Clamp (m_Downsampling, 1, 6);
		m_Radius = Mathf.Clamp (m_Radius, 0.05f, 1.0f);
		m_MinZ = Mathf.Clamp (m_MinZ, 0.00001f, 0.5f);
		m_OcclusionIntensity = Mathf.Clamp (m_OcclusionIntensity, 0.5f, 4.0f);
		m_OcclusionAttenuation = Mathf.Clamp (m_OcclusionAttenuation, 0.2f, 2.0f);
		m_Blur = Mathf.Clamp (m_Blur, 0, 4);

		// Render SSGI term into a smaller texture
		RenderTexture rtGI = RenderTexture.GetTemporary (source.width / m_Downsampling, source.height / m_Downsampling, 0);
		float fovY = GetComponent<Camera>().fieldOfView;
		float far = GetComponent<Camera>().farClipPlane;
		float y = Mathf.Tan (fovY * Mathf.Deg2Rad * 0.5f) * far;
		float x = y * GetComponent<Camera>().aspect;
		m_SSGIMaterial.SetVector ("_FarCorner", new Vector3(x,y,far));
		int noiseWidth, noiseHeight;
		if (m_RandomTexture) {
			noiseWidth = m_RandomTexture.width;
			noiseHeight = m_RandomTexture.height;
		} else {
			noiseWidth = 1; noiseHeight = 1;
		}
		m_SSGIMaterial.SetVector ("_NoiseScale", new Vector3 ((float)rtGI.width / noiseWidth, (float)rtGI.height / noiseHeight, 0.0f));
		m_SSGIMaterial.SetVector ("_Params", new Vector4(
			m_Radius,
			m_MinZ,
			1.0f / m_OcclusionAttenuation,
			m_OcclusionIntensity));
			
		bool doBlur = m_Blur > 0;
		Graphics.Blit (doBlur ? null : source, rtGI, m_SSGIMaterial, (int)m_SampleCount);

		if (doBlur)
		{
			// Blur SSGI horizontally
			RenderTexture rtBlurX = RenderTexture.GetTemporary (source.width, source.height, 0);
			m_SSGIMaterial.SetVector ("_TexelOffsetScale",
				new Vector4 ((float)m_Blur / source.width, 0,0,0));
			m_SSGIMaterial.SetTexture ("_SSGI", rtGI);
			Graphics.Blit (null, rtBlurX, m_SSGIMaterial, 3);
			RenderTexture.ReleaseTemporary (rtGI); // original rtGI not needed anymore

			// Blur SSGI vertically
			RenderTexture rtBlurY = RenderTexture.GetTemporary (source.width, source.height, 0);
			m_SSGIMaterial.SetVector ("_TexelOffsetScale",
				new Vector4 (0, (float)m_Blur/source.height, 0,0));
			m_SSGIMaterial.SetTexture ("_SSGI", rtBlurX);
			Graphics.Blit (source, rtBlurY, m_SSGIMaterial, 3);
			RenderTexture.ReleaseTemporary (rtBlurX); // blurX RT not needed anymore

			rtGI = rtBlurY; // GI is the blurred one now
		}

		// Modulate scene rendering with SSGI
		m_SSGIMaterial.SetTexture ("_SSGI", rtGI);
		Graphics.Blit (source, destination, m_SSGIMaterial, 4);

		RenderTexture.ReleaseTemporary (rtGI);
	}
	
	/*
	private void CreateRandomTable (int count, float minLength)
	{
		Random.seed = 1337;
		Vector3[] samples = new Vector3[count];
		// initial samples
		for (int i = 0; i < count; ++i)
			samples[i] = Random.onUnitSphere;
		// energy minimization: push samples away from others
		int iterations = 100;
		while (iterations-- > 0) {
			for (int i = 0; i < count; ++i) {
				Vector3 vec = samples[i];
				Vector3 res = Vector3.zero;
				// minimize with other samples
				for (int j = 0; j < count; ++j) {
					Vector3 force = vec - samples[j];
					float fac = Vector3.Dot (force, force);
					if (fac > 0.00001f)
						res += force * (1.0f / fac);
				}
				samples[i] = (samples[i] + res * 0.5f).normalized;
			}
		}
		// now scale samples between minLength and 1.0
		for (int i = 0; i < count; ++i) {
			samples[i] = samples[i] * Random.Range (minLength, 1.0f);
		}		

		string table = string.Format ("#define SAMPLE_COUNT {0}\n", count);
		table += "const float3 RAND_SAMPLES[SAMPLE_COUNT] = {\n";
		for (int i = 0; i < count; ++i) {
			Vector3 v = samples[i];
			table += string.Format("\tfloat3({0},{1},{2}),\n", v.x, v.y, v.z);
		}
		table += "};\n";
		Debug.Log (table);
	}
	*/
}
