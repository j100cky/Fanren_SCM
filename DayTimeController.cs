using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DayTimeController : MonoBehaviour
{
	const float secondsInDay = 86400f;
	const float phaseLength = 900f; //15 minutes chunk of time
									//[SerializeField] Color nightLightColor;
									//[SerializeField] Color dayLightColor = Color.white;

	public float time;
	//public int timeInt; 
	[SerializeField] float timeScale = 600f;
	[SerializeField] Text timeText;
	[SerializeField] Text seasonText;
	[SerializeField] Text dayText;
	[SerializeField] float startAtTime = 28800f; //the day starts at 8am in the morning.

	[SerializeField] Dictionary dayWords;
	[SerializeField] Dictionary hourWords;
	[SerializeField] Dictionary seasonWords;

	[SerializeField] bool chineseMode;

	List<TimeAgent> agents;

	private void Awake()
	{
		agents = new List<TimeAgent>();
	}

	private void Start()
	{
		time = startAtTime;
	}

	public void Subscribe(TimeAgent timeAgent)
	{
		agents.Add(timeAgent);
	}

	public void Unsubscribe(TimeAgent timeAgent)
	{
		agents.Remove(timeAgent);
	}

	float Seasons
	{
		get { return Days / 30f; }
	}

	float Days
	{
		get { return time / secondsInDay; }
	}

	float ChineseHours
	{
		get { return time / 7200f; }
	}

	float Hours
	{
		get { return time / 3600f; }
	}

	float ChineseMinutes
	{
		get { return time % 7200f / 60f; }
	}

	float Minutes
	{
		get { return time % 3600 / 60f; }
	}

	private void Update()
	{
		if (GameManager.instance.GetComponent<GamePauseController>().isPaused == true) { return; } //Not implementing time counting when isPaused is true.//
		time += Time.deltaTime * timeScale; //One second in real life will be equal to one minute in the game.//
		//timeInt = (int)time; //For the ease of calculation.
		TimeValueCalculator();
		TimeAgents();
	}

	private void TimeValueCalculator()
	{

		int chh = (int)ChineseHours;
		int cmm = (int)ChineseMinutes;
		int hh = GetHour();
		int mm = GetMinute();
		int dd = GetDay();
		int season = GetSeason();
		if (chineseMode == true)
		{
			timeText.text = HourConverter(chh) + MinuteConverter(cmm);
		}
		else
		{
			timeText.text = hh.ToString("00") + ":" + mm.ToString("00");
		}

		dayText.text = DayConverter(dd);
		seasonText.text = SeasonConverter(season);
	}

	int oldPhase = 0;
	private void TimeAgents()
	{
		int currentPhase = (int)(time / phaseLength);
		if (oldPhase != currentPhase)
		{
			oldPhase = currentPhase;
			for (int i = 0; i < agents.Count; i++)
			{
				agents[i].Invoke();
			}
		}


	}

	private string HourConverter(int hour)
	{
		if (hour >= 12)
		{
			while (hour >= 12)
			{
				hour = hour - 12;
			}
		}
		return hourWords.wordList[hour];
	}

	private string MinuteConverter(int minute)
	{
		if (minute > 0 && minute <= 15)
		{
			return "一刻";
		}
		else if (minute > 15 && minute <= 30)
		{
			return "二刻";
		}
		else if (minute > 30 && minute <= 45)
		{
			return "三刻";
		}
		else if (minute > 45 && minute <= 60)
		{
			return "四刻";
		}
		else if (minute > 60 && minute <= 75)
		{
			return "五刻";
		}
		else if (minute > 75 && minute <= 90)
		{
			return "六刻";
		}
		else if (minute > 90 && minute <= 105)
		{
			return "七刻";
		}
		else
		{
			return "八刻";
		}
	}

	private string DayConverter(int day)
	{
		if (day > 30)
		{
			day = day - 30;
		}
		return dayWords.wordList[day];
	}

	private string SeasonConverter(int season)
	{
		if (season >= 120)
		{
			season = season - 120;
		}
		return seasonWords.wordList[season];
	}

	//This is called by clicking the clock icon next to the time text . It switches the time format between standard and chinese. 
	public void SwitchTimeFormat()
	{
		chineseMode = !chineseMode;
	}

	//These getters are used both in the TimeValueCalculator() and by other scripts that want to access the time. 

	public int GetMinute()
	{
		return 10 * ((int)(Minutes / 10f));
	}

	public int GetHour()
	{
		int hh = (int)Hours;
		if (hh >= 24)
		{
			while (hh >= 24)
			{
				hh = hh - 24;
			}
		}
		return hh;
	}

	public int GetDay()
	{
		return (int)(Days);
	}

	public int GetSeason()
	{
		return (int)Seasons;
	}

	//For testing buttons that increase/decrease time and speed. 
	public void AddOneHour()
    {
		time = time + 3600f;
    }
	public void DeductOneHour()
	{
		time = time - 3600f;
	}
	public void Increase100Speed()
    {
		timeScale = timeScale + 100f;
    }
	public void Decrease100Speed()
    {
		timeScale = timeScale - 100f;
    }

	//Saving season, day, hour, and minute. 

	[Serializable]
	public class SaveTimeData
    {
		public float savedTime;


		public SaveTimeData(float tt)
        {
			savedTime = tt;
        }

    }

	public void Save()
    {
		SaveTimeData savedTimeData = new SaveTimeData(time);
		string jsonString = JsonUtility.ToJson(savedTimeData);
		System.IO.File.WriteAllText(Application.persistentDataPath + "/TimeData.json", jsonString);
    }

	public void Load()
    {
		string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/TimeData.json");
		if (jsonString == "" || jsonString == "{}" || jsonString == null) { return; }
		SaveTimeData loadedTimeData = JsonUtility.FromJson<SaveTimeData>(jsonString);
		time = loadedTimeData.savedTime;

	}

}
