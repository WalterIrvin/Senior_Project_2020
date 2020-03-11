﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour
{
    public List<AudioClip> m_musicTracks;
    public AudioSource m_speaker;
    private int m_selection = 0;
    private System.Random m_randomizer;
    void Start()
    {
        m_randomizer = new System.Random();
    }
    void Shuffle()
    {
        //Picks a random int
        m_selection = m_randomizer.Next(0, m_musicTracks.Count);
        Debug.Log(m_selection);
    }
    void Update()
    {
        if (m_speaker.isPlaying == false)
        {
            //Is done playing a clip -- or hasn't yet started
            Shuffle();
            m_speaker.clip = m_musicTracks[m_selection];
            m_speaker.Play();
        }
    }
}