using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [Header("Ui tings")]
    public UnityEngine.UI.Button settingsButton;
    public UnityEngine.UI.Button backToMainMenuButton;
    public UnityEngine.UI.Button backToStartMenuButton;
    public UnityEngine.UI.Button backToGameButton;
    public UnityEngine.UI.Slider fovSlider;
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Slider sfxSlider;
    public Camera fpsCamera;
    public FirstPersonLook cam;

    [Header("Audio")]
    public AudioSource[] musicSources;
    public AudioSource[] sfxSources;

    [Header("Canvas")]
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public GameObject InfoPanel;

    private bool inMenu = false;
    private bool onSettings = false;



    private int currentMusicIndex = 0;
    private int currentSFXIndex = 0;

    void Start()
    {
        


        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(false);

        if (backToStartMenuButton != null)
            backToStartMenuButton.onClick.AddListener(BackToStartMenu);
        
        if (backToMainMenuButton != null)
            backToMainMenuButton.onClick.AddListener(BackToMainMenu);

        if (backToGameButton != null)
            backToGameButton.onClick.AddListener(BackToGame);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettings);

        if (fovSlider != null)
            fovSlider.onValueChanged.AddListener(UpdateFOV);

        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    public void OnSettings() { onSettings = true; }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void BackToMainMenu()
    {
        onSettings = false;
    }

    public void UpdateFOV(float value)
    {
        if (fpsCamera != null)
            fpsCamera.fieldOfView = value;
    }

    public void UpdateMusicVolume(float value)
    {
        foreach (var music in musicSources)
        {
            music.volume = value;
        }
    }

    public void UpdateSFXVolume(float value)
    {
        foreach (var sfx in sfxSources)
        {
            sfx.volume = value;
        }
    }

    public void NextMusic()
    {
        if (musicSources.Length > 0)
        {
            musicSources[currentMusicIndex].Stop();
            currentMusicIndex = (currentMusicIndex + 1) % musicSources.Length;
            musicSources[currentMusicIndex].Play();
        }
    }

    public void NextSFX()
    {
        if (sfxSources.Length > 0)
        {
            sfxSources[currentSFXIndex].Stop();
            currentSFXIndex = (currentSFXIndex + 1) % sfxSources.Length;
            sfxSources[currentSFXIndex].Play();
        }
    }

    public void BackToGame()
    {
        inMenu = false;
        InfoPanel.SetActive(true);
        SettingsMenu.SetActive(false);
        PauseMenu.SetActive(false);
    }

    public void Update() 
    {
        InfoPanel.SetActive(true);
        if (Input.GetKey(KeyCode.Tab))
        {
            inMenu = true;

            InfoPanel.SetActive(false);
            PauseMenu.SetActive(true);
            SettingsMenu.SetActive(false);
        }
        else { }

        if (onSettings == true)
        {
            SettingsMenu.SetActive(true);
            PauseMenu.SetActive(false);
        }
        else { }

        if (onSettings == false && inMenu == true)
        {
            SettingsMenu.SetActive(false);
            PauseMenu.SetActive(true);
        }
        else { }

        cam.isInMenu = inMenu!;
        



    }
}
