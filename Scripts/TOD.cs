using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Water;

public class TOD : MonoBehaviour
{
	public float slider;
	public float slider2;
	public float Hour;
	private float Tod;

	public Light sun;

	public int speed = 50;


	public Color NightAmbientLight;
	public Color DuskAmbientLight;
	public Color MorningAmbientLight;
	public Color MiddayAmbientLight;
	public Color SunNight;
	public Color SunDay;

	[SerializeField] private List<WaterPropertyBlockSetter> _waterPropertyBlockSetters;
	[SerializeField] private List<GameObject> _lights;
	public Color WaterNight;
	public Color WaterDay;

    private void Start()
    {
		slider = 0.35f;
    }
    void Update()
	{

		if (slider >= 1.0)
		{
			slider = 0;
		}

		//slider = GUI.HorizontalSlider(new Rect(20, 20, 200, 30), slider, 0, 1.0f);
		Hour = slider * 24;
		Tod = slider2 * 24;
		sun.transform.localEulerAngles = new Vector3((slider * 360) - 90, 0, 0);
		slider = slider + Time.deltaTime / speed;
		sun.color = Color.Lerp(SunNight, SunDay, slider * 2);
		if (slider < 0.5)
		{
			slider2 = slider;
		}
		if (slider > 0.5)
		{
			slider2 = (1 - slider);
		}
		sun.intensity = (slider2 - 0.2f) * 1.7f;


		if (Tod < 4)
		{
			//it is Night

			RenderSettings.skybox.SetFloat("_Blend", 0);
			RenderSettings.ambientLight = NightAmbientLight;
			
		}
		if (Tod > 4 && Tod < 6)
		{

			RenderSettings.skybox.SetFloat("_Blend", 0);
			RenderSettings.skybox.SetFloat("_Blend", (Tod / 2) - 2);
			RenderSettings.ambientLight = Color.Lerp(NightAmbientLight, DuskAmbientLight, (Tod / 2) - 2);
			foreach(WaterPropertyBlockSetter waterPropertyBlockSetter in _waterPropertyBlockSetters)
            {
				waterPropertyBlockSetter.waterColor = Color.Lerp(WaterNight, WaterDay, (Tod / 2) - 2);
            }
			foreach (GameObject light in _lights)
			{
				light.SetActive(true);
			}
			//it is Dusk

		}
		if (Tod > 6 && Tod < 8)
		{
			RenderSettings.skybox.SetFloat("_Blend", 0);
			RenderSettings.skybox.SetFloat("_Blend", (Tod / 2) - 3);
			RenderSettings.ambientLight = Color.Lerp(DuskAmbientLight, MorningAmbientLight, (Tod / 2) - 3);;
			foreach (GameObject light in _lights)
			{
				light.SetActive(false);
			}
			//it is Morning

		}
		if (Tod > 8 && Tod < 10)
		{
			RenderSettings.ambientLight = MiddayAmbientLight;
			RenderSettings.skybox.SetFloat("_Blend", 1);
			RenderSettings.ambientLight = Color.Lerp(MorningAmbientLight, MiddayAmbientLight, (Tod / 2) - 4);

			//it is getting Midday
			foreach (GameObject light in _lights)
			{
				light.SetActive(false);
			}
		}
	}
}