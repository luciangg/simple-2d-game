using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLeaderboard : MonoBehaviour
{
	[SerializeField] private GameObject panelRanking;
    [SerializeField] private Text rankingText;
	
	
    private PlayFabLogin login;
	
    void Start()
    {
		GetLogin();
    }
			
	public void GetLeaderboard(string leaderboard = "Ranking")
	{
		if(!GetLogin().IsProcessingWebRequest())
		{
			GetLeaderboardRequest requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = leaderboard };
			PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnGetLeaderboardError);

			// Debug.Log("Carregando placar...");

			GetLogin().SetProcessingWebRequestState(true);
		}
	}
	
	private void OnGetLeaderboard(GetLeaderboardResult result)
	{
		rankingText.text = "";
		int index = 1;
		foreach (PlayerLeaderboardEntry player in result.Leaderboard)
		{
			rankingText.text += index + " - " + player.DisplayName + ": " + player.StatValue + "\n";
			index++;
		}
		GetLogin().SetProcessingWebRequestState(false);
	}

	private void OnGetLeaderboardError(PlayFabError error)
	{
		Debug.Log("Erro ao carregar o placar");
		Debug.Log(error.GenerateErrorReport());
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
		panelRanking.SetActive(active);
		GetLeaderboard("BestScore");
	}	
}
