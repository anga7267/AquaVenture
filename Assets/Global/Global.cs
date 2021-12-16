using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class Global
{
	public static Settings settings;

	static string settingsPath = "settings.json";

	public static void Init()
	{
		Reset();
	}

	public static void Save()
	{
		var path = Path.Combine(Application.persistentDataPath, settingsPath);
		File.WriteAllText(path, JsonUtility.ToJson(settings));
	}

	public static void Reset()
	{
		var path = Path.Combine(Application.persistentDataPath, settingsPath);
		if (File.Exists(path) == false)
		{
			settings = new Settings();
		}
		else
		{
			settings = JsonUtility.FromJson<Settings>(File.ReadAllText(path));
		}
	}
}

public struct Settings
{
	public bool invertUp;
}
