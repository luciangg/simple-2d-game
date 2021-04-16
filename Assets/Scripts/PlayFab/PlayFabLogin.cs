using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private string customID;
	private string playFabId;
	private string displayName;
    private bool waitWebRequest;
	
	private PlayFabAccount playFabAccount;

    // [SerializeField] private PlayFabLog log;
	
    void Start()
    {
		playFabAccount = gameObject.GetComponent<PlayFabAccount>();
        Login();
    }
	
	public void Login()
    {        
        CreateCustomID();		
        PlayFabClientAPI.LoginWithCustomID(
			new LoginWithCustomIDRequest { 
				CustomId = customID,
				CreateAccount = true,
				InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
					GetPlayerProfile = true
				}
			},
			OnLoginSuccess,
			OnLoginFailure
		);

        Debug.Log("Conectando...");
        waitWebRequest = true;
    }
	
	public void CreateCustomID()
    {
		// Debug.Log("SystemInfo.deviceUniqueIdentifier" + SystemInfo.deviceUniqueIdentifier);
		customID = SystemInfo.deviceUniqueIdentifier;
        // customID = System.Guid.NewGuid().ToString("N");
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("OK");
		playFabId = result.PlayFabId;
		waitWebRequest = false;
		
		if(result.InfoResultPayload != null && result.InfoResultPayload.PlayerProfile != null)
		{
			displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
			PlayerPrefs.SetString("Name", displayName);
		}
		
		if(playFabAccount != null)
		{
			playFabAccount.Initialize();
			
			if(string.IsNullOrEmpty(displayName))
			{
				playFabAccount.SetActive(true);
			}
		}
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        waitWebRequest = false;

        Login();
    }
    
    public bool IsProcessingWebRequest()
    {
        return waitWebRequest;
    }

    public void SetProcessingWebRequestState(bool _currentState)
    {
        waitWebRequest = _currentState;
    }
	
	public string GetPlayFabId()
	{
		return playFabId;
	}
	public string GetDisplayName()
	{
		return displayName;		
	}
	public void SetDisplayName(string name)
	{
		displayName = name;
		PlayerPrefs.SetString("Name", name);
	}
}
