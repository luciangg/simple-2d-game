using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLeaderboard : MonoBehaviour
{
    private StatisticUpdate score;
    private List<StatisticUpdate> playerScore;

    [SerializeField] private Text scoreText;

    [SerializeField] private InputField nameField;
    [SerializeField] private InputField pointsField;

    [SerializeField] private PlayFabLogin login;
	
    void Start()
    {
		Debug.Log("AQUI no START");
        score = new StatisticUpdate();
        playerScore = new List<StatisticUpdate>();
        playerScore.Add(score);
    }
	
	public void AddLeaderboardEntry()
	{
		if(!login.IsProcessingWebRequest())
		{
			playerScore[0].StatisticName = "Ranking";
			playerScore[0].Value = int.Parse(pointsField.text);

			UpdateUserTitleDisplayNameRequest requestDisplayName = new UpdateUserTitleDisplayNameRequest { DisplayName = nameField.text };
				PlayFabClientAPI.UpdateUserTitleDisplayName(requestDisplayName, OnDisplayNameChange, OnDisplayNameError);

			Debug.Log("Inserindo no placar...");

			login.SetProcessingWebRequestState(true);
		}
	}
	
	private void OnDisplayNameChange(UpdateUserTitleDisplayNameResult result)
	{
		UpdatePlayerStatisticsRequest requestStatistic = new UpdatePlayerStatisticsRequest { Statistics = playerScore };
		PlayFabClientAPI.UpdatePlayerStatistics(requestStatistic, OnLeaderBoardPost, OnLeaderboardError);
	}    

	private void OnDisplayNameError(PlayFabError error)
	{
		Debug.Log("Erro ao inserir nome");
		Debug.Log(error.GenerateErrorReport());
		login.SetProcessingWebRequestState(false);
	}

	private void OnLeaderBoardPost(UpdatePlayerStatisticsResult result)
	{
		Debug.Log("Inserido! Recarregue o placar para visualizar os resultados");

		login.SetProcessingWebRequestState(false);
		nameField.text = "";
		pointsField.text = "";

		// login.Login();
	}

	private void OnLeaderboardError(PlayFabError error)
	{
		Debug.Log("Erro ao inserir no placar");
		Debug.Log(error.GenerateErrorReport());
	}
	
	public void GetLeaderboard(string leaderboard = "Ranking")
	{
		if(!login.IsProcessingWebRequest())
		{
			GetLeaderboardRequest requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = leaderboard };
			PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnGetLeaderboardError);

			Debug.Log("Carregando placar...");

			login.SetProcessingWebRequestState(true);
		}
	}
	
	private void OnGetLeaderboard(GetLeaderboardResult result)
	{
		scoreText.text = "";
		foreach (PlayerLeaderboardEntry player in result.Leaderboard)
		{
			scoreText.text += player.DisplayName + ": " + player.StatValue + "\n";
		}

		Debug.Log("Placar carregado");

		login.SetProcessingWebRequestState(false);
	}

	private void OnGetLeaderboardError(PlayFabError error)
	{
		Debug.Log("Erro ao carregar o placar");
		Debug.Log(error.GenerateErrorReport());
	}

}
