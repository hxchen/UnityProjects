using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioSource[] destroyNoise;
    public AudioSource backgroundMusic;

    private void Start() {
        if (PlayerPrefs.HasKey("Sound")) {
            if (PlayerPrefs.GetInt("Sound") == 0) {
                backgroundMusic.Play();
                backgroundMusic.volume = 0;
            } else {
                backgroundMusic.Play();
                backgroundMusic.volume = 1;
            }
        } else {
            backgroundMusic.Play();
            backgroundMusic.volume = 1;
        }
    }

    public void adjustVolumn() {
        if (PlayerPrefs.HasKey("Sound")) {
            if (PlayerPrefs.GetInt("Sound") == 0) {
                backgroundMusic.volume = 0;
            } else {
                backgroundMusic.volume = 1;
            }
        }
    }

    public void PlayRandomDestroyNoise() {
        if (PlayerPrefs.HasKey("Sound")) {
            if (PlayerPrefs.GetInt("Sound") == 1) {
                int clipToPlay = Random.Range(0, destroyNoise.Length);
                destroyNoise[clipToPlay].Play();
            }
        } else {
            int clipToPlay = Random.Range(0, destroyNoise.Length);
            destroyNoise[clipToPlay].Play();
        }
    }
}
