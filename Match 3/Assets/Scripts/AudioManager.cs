using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] destroyNoise;

    public void PlayRandomDestroyNoise() {
        int clipToPlay = Random.Range(0, destroyNoise.Length);
        destroyNoise[clipToPlay].Play();
    }
}
