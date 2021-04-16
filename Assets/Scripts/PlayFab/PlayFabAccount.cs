using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabAccount : MonoBehaviour
{
    [SerializeField] private InputField nameField;
	[SerializeField] private Text errorField;
	[SerializeField] private Dropdown dropdownLanguage;
	[SerializeField] private GameObject panelAccount;
	
	private PlayFabLogin login;
	private static int selectedLocaleIndex;
	
	public void Initialize()
	{
		StartCoroutine(InitializePanel());
	}
	
	private IEnumerator InitializePanel()
    {
		GetLogin();
		string name = PlayerPrefs.GetString("Name");
		nameField.text = name;	
        
        yield return LocalizationSettings.InitializationOperation;
        
        var options = new List<Dropdown.OptionData>();
        selectedLocaleIndex = 0;
        for(int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selectedLocaleIndex = i;
            options.Add(new Dropdown.OptionData(locale.name));
        }
		selectedLocaleIndex = PlayerPrefs.GetInt("LocaleIndex", selectedLocaleIndex);
        dropdownLanguage.options = options;		
        dropdownLanguage.value = selectedLocaleIndex;
        dropdownLanguage.onValueChanged.AddListener(LocaleSelected);
		SetLocale();
    }
	
	static void LocaleSelected(int index)
    {
		selectedLocaleIndex = index;
    }
	   
	public void UpdateUserName()
	{
		if(!GetLogin().IsProcessingWebRequest())
		{
			SetLocale();
			UpdateUserTitleDisplayNameRequest requestDisplayName = new UpdateUserTitleDisplayNameRequest { DisplayName = nameField.text };
			
			PlayFabClientAPI.UpdateUserTitleDisplayName(requestDisplayName, OnDisplayNameChange, OnDisplayNameError);			
			GetLogin().SetProcessingWebRequestState(true);
		}
	}
	
	private void OnDisplayNameChange(UpdateUserTitleDisplayNameResult result)
	{
		GetLogin().SetDisplayName(result.DisplayName);
		GetLogin().SetProcessingWebRequestState(false);		
		SetActive(false);
	} 

	private void SetLocale()
	{
		PlayerPrefs.SetInt("LocaleIndex", selectedLocaleIndex);
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLocaleIndex];
	}

	private void OnDisplayNameError(PlayFabError error)
	{
		string localizationKey = "Error Changing Name";
		var label = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", localizationKey);
		errorField.text = label.Result;
		localizationKey = "Check Connection Try Again";
		if(error.Error == PlayFabErrorCode.NameNotAvailable || error.Error == PlayFabErrorCode.InvalidParams)
		{
			localizationKey = "Invalid Unavailable Name";
		}
		label = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", localizationKey);
		errorField.text += "\n" + label.Result;
		Debug.Log("Erro ao atualizar o nome");
		Debug.Log(error.GenerateErrorReport());		
		Debug.Log("Details: " + error.Error.ToString() + "==>" + error.ErrorDetails + " ===> " + error.ErrorMessage);	
		GetLogin().SetProcessingWebRequestState(false);
	}
	
	private PlayFabLogin GetLogin()
	{
		if(!login)
		{
			login = GameObject.FindGameObjectWithTag("PlayFabControl").GetComponent<PlayFabLogin>();
		}
		return login;
	}
	
	public void SetActive(bool active)
	{
		if(!active && string.IsNullOrEmpty(GetLogin().GetDisplayName()))
		{
			var label = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Insert Name");
			errorField.text = label.Result;
			return;
		}
		panelAccount.SetActive(active);
	}
}
