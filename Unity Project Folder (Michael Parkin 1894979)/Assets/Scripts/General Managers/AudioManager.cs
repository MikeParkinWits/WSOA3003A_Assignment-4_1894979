using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioSource hitAudio, backgroundAudio, deathAudio, loseAudio, winAudio, specialHitAudio, winOtherAudio, otherAudio, thrusterLoudAudio;

    // Start is called before the first frame update
    void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        hitAudio = audioSources[0];
        backgroundAudio = audioSources[1];
        deathAudio = audioSources[2];
        loseAudio = audioSources[3];
        winAudio = audioSources[4];
        specialHitAudio = audioSources[4];
    }
}
