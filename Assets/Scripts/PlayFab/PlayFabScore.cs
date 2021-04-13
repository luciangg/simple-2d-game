using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabScore : MonoBehaviour
{
	private PlayFabLogin login;
	
    void Start()
    {		
		GetLogin();
    }
	
	public void AddScore(int score)
	{
		AddToRanking(score);
		
		GetLogin().SetProcessingWebRequestState(true);
	}
	
	private void AddToRanking(int score)
	{
		PlayFabClientAPI.UpdatePlayerStatistics(
			GetRequest("Ranking", score),
			response => {
				AddToBestScote(score);
			}, 
			OnError
		);
	}
	private void AddToBestScote(int score)
	{
		PlayFabClientAPI.UpdatePlayerStatistics(
			GetRequest("BestScore", score),
			OnSuccess, 
			OnError
		);
	}
	
	private UpdatePlayerStatisticsRequest GetRequest(string name, int value)
	{
		return new UpdatePlayerStatisticsRequest {
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate {StatisticName = name, Value = value}
			}
		};
	}

	private void OnSuccess(UpdatePlayerStatisticsResult result)
	{
		// Debug.Log("Inserido! Recarregue o placar para visualizar os resultados");

		GetLogin().SetProcessingWebRequestState(false);
	}

	private void OnError(PlayFabError error)
	{
		Debug.Log("Erro ao inserir no placar");
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
}
