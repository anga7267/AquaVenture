using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{

	public GameObject main;
	public GameObject mainController;

	public GameObject controller;
	public GameObject controllerOffer;
	public GameObject controllerAnswer;

	public GameObject settings;
	public GameObject settingsInvertY;

	public void StartGame()
	{
		SceneManager.LoadScene("Main", LoadSceneMode.Single);
	}

	public void Start()
	{
		Controller.Init();
		Global.Init();
	}



	//Controller
	public void ShowController()
	{
		controller.SetActive(true);
		main.SetActive(false);
		StartCoroutine(Controller.StartRTC(this.CodeHandle));
	}

	void CodeHandle(string code)
	{
		controllerOffer.GetComponent<InputField>().text = "Offer code: " + code;
	}
	public void AnswerHandle(InputField input)
	{
		var code = input.text;
		StartCoroutine(Controller.AnswerRTC(code, this.ResultHandle));
	}
	public void AnswerFixHandle(InputField input)
	{
		var text = input.text;
		if (text != text.ToUpper())
		{
			input.text = text.ToUpper();
		}
	}
	public void ResultHandle(bool success)
	{
		if (success)
		{
			main.SetActive(true);
			controller.SetActive(false);
			mainController.GetComponentInChildren<Text>().text = "Connected";
			mainController.GetComponent<Button>().interactable = false;
		}
		else
		{
			controllerOffer.GetComponent<InputField>().text = "Wrong code";
			controllerAnswer.GetComponent<InputField>().text = "";
		}
	}
	public void HideController()
	{
		controller.SetActive(false);
		main.SetActive(true);
	}


	//Settings
	public void ShowSettings()
	{
		settings.SetActive(true);
		main.SetActive(false);
		settingsInvertY.GetComponentInChildren<Toggle>().isOn = Global.settings.invertUp;
	}

	public void ToggleInvertUp(Toggle toggle)
	{

		var colors = toggle.colors;
		var a = colors.normalColor;
		var b = colors.pressedColor;
		colors.normalColor = b;
		colors.highlightedColor = b;
		colors.pressedColor = a;
		toggle.colors = colors;
		Global.settings.invertUp = toggle.isOn;
	}

	public void SaveSettings()
	{
		Global.Save();
		settings.SetActive(false);
		main.SetActive(true);
	}

	public void DiscardSettings()
	{
		Global.Reset();
		settings.SetActive(false);
		main.SetActive(true);
	}
}
