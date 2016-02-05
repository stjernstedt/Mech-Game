using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutText : MonoBehaviour
{
	public float timeToFadeIn = 1f;
	public float timeToFadeOut = 1f;
	float startTime;
	float timePassed;

	Text text;

	void OnEnable()
	{
		text = GetComponent<Text>();
		StartCoroutine(FadeIn());
	}

	IEnumerator FadeIn()
	{
		startTime = Time.time;
		timePassed = 0;
		text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
		while (timePassed < timeToFadeIn)
		{
			timePassed = Time.time - startTime;
			float lerpValue = timePassed / timeToFadeIn;
			text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(0, 1, lerpValue));
			yield return null;
		}
		StartCoroutine(FadeOut());
	}

	IEnumerator FadeOut()
	{
		startTime = Time.time;
		timePassed = 0;
		while (timePassed < timeToFadeOut)
		{
			timePassed = Time.time - startTime;
			float lerpValue = timePassed / timeToFadeOut;
			text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1, 0, lerpValue));
			yield return null;
		}
		gameObject.SetActive(false);
	}
}
