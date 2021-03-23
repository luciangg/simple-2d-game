using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
	public void Load(string levelName)
	{
		SceneManager.LoadScene(levelName, LoadSceneMode.Single);
	}
}
