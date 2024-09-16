using UnityEngine;
using System.IO.Ports;
using UnityEngine.Audio;

public class ArduinoController : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM3", 9600); // Set to the correct COM port

    public AudioSource audioSourceRickRoll;  // Audio source for RickRoll song
    public AudioSource audioSourceWhatIsLove;  // Audio source for What is Love song

    public AudioMixerGroup audioMixer;
    private AudioSource currentAudioSource; // Reference to the current playing song
    
    

    void Start()
    {
        audioSourceRickRoll.outputAudioMixerGroup = audioMixer;
        audioSourceWhatIsLove.outputAudioMixerGroup = audioMixer;
        
        if (!serialPort.IsOpen)
        {
            serialPort.Open();  // Open the serial port
        }
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string message = serialPort.ReadLine(); // Read the incoming message

                if (message.Contains("Play RickRoll"))
                {
                    //PlaySong(audioSourceRickRoll);
                    FastForward(.5f);
                }
                else if (message.Contains("Pause RickRoll"))
                {
                    //PauseSong(audioSourceRickRoll);
                    FastBackward();
                }
                else if (message.Contains("Play WhatIsLove"))
                {
                    PlaySong(audioSourceWhatIsLove);
                }
                else if (message.Contains("Pause WhatIsLove"))
                {
                    PauseSong(audioSourceWhatIsLove);
                }
            }
            catch (System.Exception)
            {
                // Handle any serial port errors here
            }
        }
    }

    void PlaySong(AudioSource newSong)
    {
        // If there is a song playing, pause it
        if (currentAudioSource != null && currentAudioSource.isPlaying)
        {
            currentAudioSource.Pause();
        }

        // Set the new song to be the current one and play it
        currentAudioSource = newSong;
        currentAudioSource.Play();
    }

    void PauseSong(AudioSource song)
    {
        // If the current song is paused, it stays paused
        if (currentAudioSource == song && currentAudioSource.isPlaying)
        {
            currentAudioSource.Pause();
        }
    }

    void FastForward(float howMuchMore)
    {
        if (currentAudioSource == null)
        {
            currentAudioSource = audioSourceRickRoll;
        }
        
        currentAudioSource.pitch = 1.5f; 
        audioMixer.audioMixer.SetFloat("pitchBend", 1f / 1.5f);
    }
    
    void FastBackward()
    {
        if (currentAudioSource == null)
        {
            currentAudioSource = audioSourceRickRoll;
        }
        
        currentAudioSource.pitch = 1; 
        audioMixer.audioMixer.SetFloat("pitchBend", 1f);
    }

    void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close(); // Close the serial port when the application quits
        }
    }
}
