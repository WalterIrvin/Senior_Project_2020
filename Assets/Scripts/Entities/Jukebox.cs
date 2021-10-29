using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public List<AudioClip> music_tracks;
    public AudioSource speaker;
    private int index = 0;
    private System.Random randomizer;
    void Start()
    {
        randomizer = new System.Random();
        Shuffle();
    }

    void Update()
    {
        if (speaker.isPlaying == false)
        {
            //Is done playing a clip -- or hasn't yet started
            Shuffle();
            speaker.clip = music_tracks[index];
            speaker.Play();
        }
    }

    void Shuffle()
    {
        int new_selection = randomizer.Next(0, music_tracks.Count);
        if (new_selection == index)
            Shuffle();
        index = new_selection;
    }

}
