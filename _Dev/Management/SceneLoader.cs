using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadNextLevel()
    {
        int level = PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1);
        PlayerPrefs.SetInt(PlayerPrefsStrings.Level, level + 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadPastLevel()
    {
        int level = PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1);
        if (level - 1 > 0)
        {
            PlayerPrefs.SetInt(PlayerPrefsStrings.Level, level - 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
