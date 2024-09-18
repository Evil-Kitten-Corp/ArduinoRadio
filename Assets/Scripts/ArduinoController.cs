using System;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.Audio;

public class ArduinoController : MonoBehaviour
{
    private readonly SerialPort _serialPort = new("COM3", 9600); 

    private Thread _serialThread;
    private bool _keepReading = true;
    private string _serialMessage;

    public AudioSource audioSourceRickRoll; 
    public AudioSource audioSourceWhatIsLove; 

    private AudioSource _currentAudioSource;
    

    void Start()
    {
        if (!_serialPort.IsOpen)
        {
            _serialPort.Open();
        }

        _serialThread = new Thread(ReadFromSerial);
        _serialThread.Start();
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(_serialMessage))
        {
            if (_serialMessage.Contains("Play"))
            {
                PlaySong(audioSourceRickRoll);
            }
            else if (_serialMessage.Contains("Pause"))
            {
                PauseSong(audioSourceRickRoll);
            }
            else if (_serialMessage.Contains("Fastf"))
            {
                FastForward(.5f);
            }
            else if (_serialMessage.Contains("Backf"))
            {
                FastBackward();
            }
            else if (_serialMessage.Contains("Rewind ON"))
            {
                Rewind(true);
            }
            else if (_serialMessage.Contains("Rewind OFF"))
            {
                Rewind(false);
            }
            else if (_serialMessage.Contains("VolumePercentage:")) 
            {
                AdjustVolume(_serialMessage);
            }

            _serialMessage = null;
        }
        
        /*if (serialPort.IsOpen)
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
        }*/
    }
    
    void AdjustVolume(string message)
    {
        // Extract the volume value from the message
        string[] parts = message.Split(' ');
        
        if (parts.Length == 2)
        {
            if (float.TryParse(parts[1], out var volume))
            {
                // Adjust the volume of the currently playing song
                if (_currentAudioSource != null)
                {
                    _currentAudioSource.volume = volume / 100f; // Convert 0-100 to 0.0-1.0
                }
            }
        }
    }

    void ReadFromSerial()
    {
        while (_keepReading && _serialPort.IsOpen)
        {
            try
            {
                string msg = _serialPort.ReadLine();

                lock (this)
                {
                    _serialMessage = msg;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
    }

    public void PlaySong(AudioSource newSong)
    {
        // If there is a song playing, pause it
        if (_currentAudioSource != null && _currentAudioSource.isPlaying)
        {
            _currentAudioSource.Pause();
        }

        // Set the new song to be the current one and play it
        _currentAudioSource = newSong;
        _currentAudioSource.Play();
    }

    public void PauseSong(AudioSource song)
    {
        // If the current song is paused, it stays paused
        if (_currentAudioSource == song && _currentAudioSource.isPlaying)
        {
            _currentAudioSource.Pause();
        }
    }

    public void FastForward(float howMuchMore)
    {
        if (_currentAudioSource == null)
        {
            _currentAudioSource = audioSourceRickRoll;
        }
        
        _currentAudioSource.pitch = 1 + howMuchMore; 
        _currentAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("pitchBend", 1f / _currentAudioSource.pitch);
    }
    
    public void FastBackward()
    {
        if (_currentAudioSource == null)
        {
            _currentAudioSource = audioSourceRickRoll;
        }
        
        _currentAudioSource.pitch = 1; 
        _currentAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("pitchBend", 1f);
    }

    public void Rewind(bool t)
    {
        if (_currentAudioSource == null)
        {
            _currentAudioSource = audioSourceRickRoll; 
        }
        
        if (t)
        {
            _currentAudioSource.pitch = -1;
            _currentAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("pitchBend", 1f);
        }
        else
        {
            _currentAudioSource.pitch = 1;
            _currentAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("pitchBend", 1f);
        }
    }

    void OnApplicationQuit()
    {
        _keepReading = false;

        if (_serialThread.IsAlive)
        {
            _serialThread.Join();
        }
    
        if (_serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }
}
