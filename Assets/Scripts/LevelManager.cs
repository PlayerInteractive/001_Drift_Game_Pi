﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  SickscoreGames.HUDNavigationSystem;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    public GameObject[] Levels;
    public GameObject[] Players;
    public int[] Reward;
    public LevelProperties CurrentLevelProperties;
    public GameObject SelectedPlayer,FreeMode;
    public static LevelManager instace;
    public int CurrentLevel, coinsCounter;
    public HUDNavigationCanvas canvashud;
    public RCC_Camera vehicleCamera;
    public PlayerCamera_New Tpscamera;
    public OpenWorldManager OpenWorldManager;
    public GameObject TpsPlayer;
    
    
    [Header("WheatherArea")]
    
    public ParticleSystem snowParticleSystem;
    
    
   
    
    public Material daySkybox;
    public Material nightSkybox;

    public bool snowing = false;
    public bool isDay = true;
   
  
    public GameObject directionalLightGO;
    void Awake()
    {
        instace = this;
        if (PrefsManager.GetGameMode() != "free")
        {
            if (PrefsManager.GetLevelMode() == 0)
            {
                SelectedPlayer = Players[PrefsManager.GetSelectedPlayerValue()];
                CurrentLevel = PrefsManager.GetCurrentLevel() - 1;
                CurrentLevelProperties = Levels[CurrentLevel].GetComponent<LevelProperties>();
                SelectedPlayer.transform.position = CurrentLevelProperties.PlayerPosition.position;
                SelectedPlayer.transform.rotation = CurrentLevelProperties.PlayerPosition.rotation;
                CurrentLevelProperties.gameObject.SetActive(true);
            }
        }
        else
        {
            Time.timeScale = 1; 
            FreeMode.SetActive(true);
            SelectedPlayer = Players[PrefsManager.GetSelectedPlayerValue()];
            SetTransform(OpenWorldManager.TpsPosition, OpenWorldManager.CarPostiom);
        }
        SelectedPlayer.SetActive(true);
    }
    public void SetTransform(Transform playerposition, Transform defulcar)
    {
        TpsPlayer.transform.position = playerposition.position;
        TpsPlayer.transform.rotation = playerposition.rotation;

        Tpscamera.transform.position = playerposition.position;
        Tpscamera.transform.rotation = playerposition.rotation;
        
        SelectedPlayer.transform.position = defulcar.position;
        SelectedPlayer.transform.rotation = defulcar.rotation;
        
    }

    public IEnumerator Start()
    {
        if (PrefsManager.GetLevelMode() != 1)
        {
            yield return new WaitForSeconds(0.5f);
            UiManagerObject.instance.ShowObjective(CurrentLevelProperties.LevelStatment);
        }
        
        
       
    }

    public void DAy()
    {
        isDay = true;
        snowing = false;
    }
    
    public void Night()
    {
        isDay = false;
        snowing = true;
    }

    public void RestCar()
    {
        GameManager.Instance.CurrentCar.GetComponent<RCC_CarControllerV3>().RestCar();
    }
    void Update()
    {
        if (snowing)
        {
            RenderSettings.skybox = daySkybox;
            directionalLightGO.GetComponent<Light>().intensity = 0.9f;
            
            snowParticleSystem.Stop();
            snowParticleSystem.gameObject.SetActive(false);
        }
        else if (isDay)
        {
            snowParticleSystem.Play();
            RenderSettings.skybox = nightSkybox;
            directionalLightGO.GetComponent<Light>().intensity = 0.4f;
            snowParticleSystem.gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }
    
    
}
