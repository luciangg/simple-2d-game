using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylistController : MonoBehaviour
{
	public List<AudioClip> playlist = new List<AudioClip>();
	public int current = 0;
	private AudioSource audio;
    
    void Start()
    {
		audio = GetComponent<AudioSource>();
        // audio.clip = playlist[current];
		// audio.Play();
    }

    void Update()
    {
		if(!audio.isPlaying)
		{
			int next = Random.Range(0, playlist.Capacity);
			current = (current != next) ? next : current++;
			current = (current >= playlist.Capacity) ? 0 : current;
			audio.clip = playlist[current];
			audio.Play();
		}
    }
}
