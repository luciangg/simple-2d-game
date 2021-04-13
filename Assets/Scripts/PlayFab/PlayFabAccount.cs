using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	
	IEnumerator Start()
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
			UpdateUserTitleDisplayNameRequest requestDisplayName = new UpdateUserTitleDisplayNameRequest { DisplayName = nameField.text };
			
			PlayFabClientAPI.UpdateUserTitleDisplayName(requestDisplayName, OnDisplayNameChange, OnDisplayNameError);			
			GetLogin().SetProcessingWebRequestState(true);
		}
	}
	
	private void OnDisplayNameChange(UpdateUserTitleDisplayNameResult result)
	{
		PlayerPrefs.SetString("Name", result.DisplayName);		
		SetLocale();
		GetLogin().SetProcessingWebRequestState(false);
		
		panelAccount.SetActive(false);
	} 

	private void SetLocale()
	{
		PlayerPrefs.SetInt("LocaleIndex", selectedLocaleIndex);
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLocaleIndex];
	}

	private void OnDisplayNameError(PlayFabError error)
	{
		errorField.text = "Erro ao atualizar o nome. Verifique a conexão e tente novamente";
		Debug.Log("Erro ao atualizar o nome");
		Debug.Log(error.GenerateErrorReport());		
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
}
