using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameScript : MonoBehaviour {
    Text numberInputLabel;
    Text timeDisplay;
    InputField numberInput;
    Button startButton;
	Button stopButton;
	RectTransform backgroundPanel;
    Button rectanglePrefabricate;
	Text finishMessage;
	Button quitButton;

	List<Button> rectangles;
	int rectanglesQuantity;

    int seconds;
    int minutes;

    void Start() {
        numberInputLabel = GameObject.Find("NumberInputLabel").GetComponent<Text>();
        timeDisplay = GameObject.Find("TimeDisplay").GetComponent<Text>();
        timeDisplay.gameObject.SetActive(false);
        numberInput = GameObject.Find("NumberInput").GetComponent<InputField>();
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButton.onClick.AddListener(StartButtonOnClick);
		stopButton = GameObject.Find("StopButton").GetComponent<Button>();
		stopButton.onClick.AddListener(StopButtonOnClick);
		stopButton.gameObject.SetActive(false);
		backgroundPanel = GameObject.Find("BackgroundPanel").GetComponent<RectTransform>();
        rectanglePrefabricate = GameObject.Find("RectanglePrefabricate").GetComponent<Button>();
        rectanglePrefabricate.gameObject.SetActive(false);
		finishMessage = GameObject.Find("FinishMessage").GetComponent<Text>();
		finishMessage.gameObject.SetActive(false);
		quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
		quitButton.onClick.AddListener(QuitButtonOnClick);

		rectangles = new List<Button>();

		seconds = 0;
		minutes = 0;
    }

	void QuitButtonOnClick() {
		Application.Quit();
	}

	void StopButtonOnClick() {
		finishMessage.text = "";
	
		Finish();
	}

    void StartButtonOnClick()
    {
        if (Validate())
        {
			finishMessage.gameObject.SetActive(false);
            numberInput.gameObject.SetActive(false);
            timeDisplay.gameObject.SetActive(true); 
            startButton.gameObject.SetActive(false);
			stopButton.gameObject.SetActive(true);
			numberInputLabel.gameObject.SetActive(false);

            InvokeRepeating("UpdateTimeDisplay", 0, 1.0f);
            int number = Int32.Parse(numberInput.text);
            GenerateRectangles(number);
        }
        else
        {
            numberInput.image.color = Color.red;
        }
    }

    bool Validate()
    {
        if (numberInput.text.Length > 0 && numberInput.text[0] != '0')
        {
            foreach (char c in numberInput.text)
            {
                if (c < '0' || c > '9') return false;
            }
        }
        else return false;
        return true;
    }

    void UpdateTimeDisplay()
    {
		seconds++;
        if (seconds == 60)
        {
            seconds = 0;
            minutes++;
            if (minutes == 60)
            {
				finishMessage.text = "Koniec czasu!";
				Finish();
            }
        }
		timeDisplay.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

	Button InstantiateRectangle() {
		Button newRectangle = Instantiate(rectanglePrefabricate);
		newRectangle.transform.SetParent(backgroundPanel, false);
		newRectangle.transform.position = new Vector3(UnityEngine.Random.value * (backgroundPanel.rect.size.x - 200) + 100, UnityEngine.Random.value * (backgroundPanel.rect.size.y - 100) + 50, 0);
		newRectangle.transform.localScale = new Vector3(UnityEngine.Random.value * 0.8f + 0.2f, UnityEngine.Random.value * 0.8f + 0.2f, 0);
		newRectangle.image.color = UnityEngine.Random.ColorHSV();
		newRectangle.gameObject.SetActive(true);
		return newRectangle;
	}

    void GenerateRectangles(int quantity)
    {
		rectanglesQuantity = quantity;
		for (int i = 0; i < quantity; i++)
        {
			Button newRectangle = InstantiateRectangle();
			int rectangleIndex = i;
			newRectangle.onClick.AddListener(() => RectangleOnClick(rectangleIndex));
			rectangles.Add(newRectangle);
        }
    }

	void RectangleOnClick(int rectangleIndex) {
		rectangles[rectangleIndex].gameObject.SetActive(false);
		rectanglesQuantity--;
		if (rectanglesQuantity == 0) {
			finishMessage.text = "Gratulacje! Twój czas to " + minutes.ToString("D2") + ":" + seconds.ToString("D2");
			Finish();
		}
	}

	void Clear() {
		numberInput.gameObject.SetActive(true);
		numberInput.text = "";
		numberInput.image.color = Color.white;
		timeDisplay.gameObject.SetActive(false);
		startButton.gameObject.SetActive(true);
		stopButton.gameObject.SetActive(false);
		numberInputLabel.gameObject.SetActive(true);

		foreach (Button b in rectangles) {
			b.gameObject.SetActive (false);
			Destroy(b);
		}
		rectangles.Clear();

		seconds = 0;
		minutes = 0;
	}

	void Finish() {
		CancelInvoke();
		finishMessage.gameObject.SetActive(true);
		Clear();
	}
}