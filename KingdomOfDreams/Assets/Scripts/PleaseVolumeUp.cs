using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PleaseVolumeUp : MonoBehaviour
{

	public Text txtVolume;

	// Use this for initialization
	void Start()
	{
		txtVolume = GetComponent<Text>();
		StartCoroutine(BlinkText());

	}

    private void Update()
    {
        if (this.txtVolume.gameObject.activeSelf == false)
        {
			StopCoroutine(BlinkText());
        }
    }

    public IEnumerator BlinkText()
	{
		while (true)
		{
			txtVolume.text = "";
			yield return new WaitForSeconds(.7f);
			txtVolume.text = "*볼륨을 높여주세요*";
			yield return new WaitForSeconds(.7f);
			txtVolume.text = "";
			yield return new WaitForSeconds(.7f);
			txtVolume.text = "*볼륨을 높여주세요*";
			yield return new WaitForSeconds(.7f);
			txtVolume.text = "";
			yield return new WaitForSeconds(.7f);
			txtVolume.text = "*볼륨을 높여주세요*";
			yield return new WaitForSeconds(.7f);

			this.txtVolume.gameObject.SetActive(false);

		}

	}
}