﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public class Specification
{
    public int[] Values;
}

public class PlayerSelectionEG : MonoBehaviour
{

    public Specification[] SpecificationValue;
    public GameObject[] sp_bar;
    public int[] Prices;
    int selectedPlayerValue = 0;
    public GameObject[] selectedDogArray;
    public GameObject nextBtn, backBtn, dogSelectionCanvas, menuCanvas, 
        levelSelectionCanvas, lockSprite, LOADING, Play, notCash, successPannel, unlockPlayerButton,TestDriveButton
      ,MainNextBack;
    public Text coinText,coinText2, ls_cointext, PriceText;
    bool isReadyForPurchase;
    int ActivePlayervalue = 1;
    int coinValue;
    public static PlayerSelectionEG instance;
    


    void Start()
    {
        instance = this;
        coinText.text = "" + PrefsManager.GetCoinsValue();
        coinText2.text = "" + PrefsManager.GetCoinsValue();
        Time.timeScale = 1;

    } 
    private void Update()
    {
        coinText.text = "" + PrefsManager.GetCoinsValue();
        coinText2.text = "" + PrefsManager.GetCoinsValue();
        ls_cointext.text = "" + PrefsManager.GetCoinsValue();
    }
    public void OnNextPressed()
    {
        SoundManager.Instance.PlayOneShotSounds(SoundManager.Instance.click);
        if (selectedPlayerValue < selectedDogArray.Length - 1)
        {
            selectedPlayerValue++;
            ShowPlayerNow(selectedPlayerValue);
        }

    }

    public void OnBackPressed()
    {
        SoundManager.Instance.PlayOneShotSounds(SoundManager.Instance.click);
        if (selectedPlayerValue > 0)
        {
            selectedPlayerValue--;
            ShowPlayerNow(selectedPlayerValue);
        }

    }


    GameObject CurrentPlayer = null;
    public void ShowPlayerNow(int val)
    {
        
        ActivePlayervalue = val;
        PriceText.text = Prices[val] + "";
        if (PrefsManager.GetPlayerState(val) == 0 && val != 0)
        {
            PriceText.transform.parent.gameObject.SetActive(true);
            lockSprite.SetActive(true);
            Play.SetActive(false);
            unlockPlayerButton.SetActive(true);
            TestDriveButton.SetActive(true);
        }
        else
        {
            lockSprite.SetActive(false);
            PriceText.transform.parent.gameObject.SetActive(false);
            Play.SetActive(true);
            unlockPlayerButton.SetActive(false);
            TestDriveButton.SetActive(false);

        }

        for (int i = 0; i < selectedDogArray.Length; i++)
        {
            selectedDogArray[i].SetActive(false);

        }
        PrefsManager.SetSelectedPlayerValue(val);
        CurrentPlayer = selectedDogArray[val];
        CurrentPlayer.SetActive(true);
        for (int i = 0; i < sp_bar.Length; i++)
        {
            sp_bar[i].GetComponent<filled>().SetEndpoint(SpecificationValue[val].Values[i]);
        }

        if (ActivePlayervalue == selectedDogArray.Length - 1)
            {
                nextBtn.SetActive(false);
                backBtn.SetActive(true);
            }
            else if (ActivePlayervalue == 0)
            {
                nextBtn.SetActive(true);
                backBtn.SetActive(false);
            }
            else
            {
                nextBtn.SetActive(true);
                backBtn.SetActive(true);
            }
        selectedPlayerValue = ActivePlayervalue;
    }

    public void TestDrive()
    {
        lockSprite.SetActive(false);
        PriceText.transform.parent.gameObject.SetActive(false);
        Play.SetActive(true);
        unlockPlayerButton.SetActive(false);
        TestDriveButton.SetActive(false);
        PrefsManager.SetSelectedPlayerValue(selectedPlayerValue);
        SelectDogPlay();
    }
    public void Ps_PlayEvent()
    {
        SoundManager.Instance.PlayOneShotSounds(SoundManager.Instance.click);
        //AdmobAdsManager.Instance.ShowInt(GoToLevelSelect, false);
        GoToLevelSelect();
    }
    public void GoToLevelSelect()
    {
        // CurrentEnv.SetActive(false);
        for (int i = 0; i < selectedDogArray.Length; i++)
        {
            selectedDogArray[i].SetActive(false);
        }

        ls_cointext.text = PrefsManager.GetCoinsValue().ToString();
        SelectDogPlay();
    }


    public void SelectDogPlay()
    {
        SoundManager.Instance.PlayOneShotSounds(SoundManager.Instance.click);
        levelSelectionCanvas.SetActive(true);
    }

    public void ForTutorialPlay()
    {
        levelSelectionCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        Debug.Log("Enable Here");
        LOADING.SetActive(true);
        PrefsManager.SetGameMode("challange");
        PrefsManager.SetCurrentLevel(1);
        PrefsManager.SetLevelMode(0);

    }

    public void RedirectToDogSelection()
    {
        //  AdmobAdsManager.Instance.LoadInterstitialAd();
        SoundManager.Instance.PlayOneShotSounds(SoundManager.Instance.click);
        dogSelectionCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        ShowPlayerNow(PrefsManager.GetLastJeepUnlock());
        MainNextBack.SetActive(true);
        selectedPlayerValue = PrefsManager.GetLastJeepUnlock();
    }
    
    public void BackToMainCanvas()
    {
      //  AdmobAdsManager.Instance.LoadInterstitialAd();
        SoundManager.Instance.PlayOneShotSounds(SoundManager.Instance.click);
        dogSelectionCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        Debug.Log("Enable Here");
    }

    public void BackFromLevelScreen()
    {
        SoundManager.Instance.PlayOneShotSounds(SoundManager.Instance.click);
        levelSelectionCanvas.SetActive(false);
        dogSelectionCanvas.SetActive(true);
        ShowPlayerNow(PrefsManager.GetSelectedPlayerValue());
        MainNextBack.SetActive(true);
        selectedPlayerValue = PrefsManager.GetSelectedPlayerValue();
    }

    public void SetLevelValue(int lValue)
    {
        //AdmobAdsManager.Instance.ShowInt(LoadGamePlay,false);
        LoadGamePlay();
        SoundManager.Instance.PlayOneShotSounds(SoundManager.Instance.click);
        PrefsManager.SetCurrentLevel(lValue);
    }

    public void LoadGamePlay()
    {
        LOADING.SetActive(true);
        LOADING.GetComponentInChildren<bl_SceneLoader>().LoadLevel("GamePlay_EG");
        Debug.Log("Called it");
    }

    public void UnlockSelectedDog()
    {
        UnlockDog(Prices[ActivePlayervalue]);
    }
    public void UnlockDog(int dogVal)
    {
        SoundManager.Instance.PlayOneShotSounds(SoundManager.Instance.click);
        if (PrefsManager.GetCoinsValue() >= dogVal)
        {
            PrefsManager.SetPlayerState(ActivePlayervalue, 1);
            PrefsManager.SetCoinsValue(PrefsManager.GetCoinsValue() - dogVal);
            Success_purchase();
            PrefsManager.SetLastJeepUnlock(ActivePlayervalue);
            Play.SetActive(true);
            unlockPlayerButton.SetActive(false);
            TestDriveButton.SetActive(false);
        }
        else
        {
            notCash.SetActive(true);
        }
    }
    public void OffNoCash()
    {
        notCash.SetActive(false);
    }
    public void Offsuccess()
    {
        successPannel.SetActive(false);
    }
    public void Success_purchase()
    {
        isReadyForPurchase = true;
        successPannel.SetActive(true);
        lockSprite.SetActive(false);
        unlockPlayerButton.SetActive(false);
        Play.SetActive(true);
        Invoke("Offsuccess", 3f);
        coinText.text = "" + PrefsManager.GetCoinsValue();
        coinText2.text = "" + PrefsManager.GetCoinsValue();
        PriceText.transform.parent.gameObject.SetActive(false);
    }

}
