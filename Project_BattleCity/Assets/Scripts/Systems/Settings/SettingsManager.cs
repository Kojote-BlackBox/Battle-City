using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/*
public class SettingsManager : MonoBehaviour {


    // https://www.youtube.com/watch?v=YOaYQrN1oYQ
    // https://www.youtube.com/watch?v=qUYpQ8ySkLU

Resolution[] resolutions;

public Dropdown resulutionDropdown;


private void Start() {
resolutions = Screen.resolutions;
resulutionDropdown.ClearOptions();
List<string> options = new List<string>();

int currectResolutionsIndex = 0;

for (int i = 0; i < resolutions.Length; i++) {
    Resolution resolution = resolutions[i];
    string option = resolution.width + " x " + resolution.height;
    options.Add(option);

    if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height) {
        currectResolutionsIndex = i;
    }
}

resulutionDropdown.AddOptions(options);
resulutionDropdown.value = currectResolutionsIndex;
resulutionDropdown.RefreshShownValue();
}

public void SetResolution(int resolutionIndex) {
Resolution resolution = resolutions[resolutionIndex];
Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
}

public void SetDifficulty(int index) {
Debug.Log(index);
}

public void SetValume(float volume) {
Debug.Log(volume);
}

public void SetFullscreen (bool isFullscreen) {
Screen.fullScreen = isFullscreen;
}
}
//void SaveVolume(float volume) {
//PlayerPrefs.SetFloat("volume", volume);
//}

*/