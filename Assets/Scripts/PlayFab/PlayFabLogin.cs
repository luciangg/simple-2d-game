using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private string customID = "64f59e32a23b4d55b4d76d63e17f05ef";
    private bool waitWebRequest;

    // [SerializeField] private PlayFabLog log;
	
    void Start()
    {		
        Login();
    }
	
	public void Login()
    {        
        CreateCustomID();

        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest { CustomId = customID, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

        // Debug.Log("Aguarde a conexão...");
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
        // Debug.Log("Conectado");

        waitWebRequest = false;
    }

    private void OnLoginFailure(PlayFabError error)
    {
        // Debug.Log("Erro na conexão");
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
}
