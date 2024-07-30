using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAudioManege : MonoBehaviour
{
    public AudioClip[] BGMNyuryoku;
    public AudioClip[] SENyuryoku;

    private static AudioClip[] BGMs;
    private static AudioClip[] SEs;

    public static AudioSource BGMAudio;
    public static AudioSource SEAudio;

    private static bool BGMInFlag, BGMOutFlag;

    public float Speed;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        BGMAudio = this.GetComponents<AudioSource>()[0];
        SEAudio = this.GetComponents<AudioSource>()[1];

        BGMs = BGMNyuryoku;
        SEs = SENyuryoku;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BGMInFlag)
        {
            BGMAudio.volume = BGMAudio.volume + (Speed * Time.deltaTime);

            BGMAudio.volume = Mathf.Clamp(BGMAudio.volume,0,0.6f);

            if (BGMAudio.volume>=0.6f)
            {
                BGMInFlag = false;
            }
        }

        if (BGMOutFlag)
        {
            BGMAudio.volume = BGMAudio.volume - (Speed * Time.deltaTime);

            BGMAudio.volume = Mathf.Clamp01(BGMAudio.volume);

            if (BGMAudio.volume <=0)
            {
                BGMOutFlag = false;
                BGMAudio.Stop();
            }
        }
    }

    public static void PlayBGM(int value)
    {
        BGMInFlag = true;
        BGMOutFlag = false;
        BGMAudio.volume = 0;

        BGMAudio.clip = BGMs[value];
        BGMAudio.Play();
    }

    public static void StopBGM()
    {
        BGMOutFlag = true;
    }

    public static void MajiStopBGM()
    {
        BGMAudio.Stop();
    }

    public static void PlaySE(int value)
    {
        SEAudio.PlayOneShot(SEs[value]);
    }

    public static void PauseBGM()
    {
        BGMAudio.Pause();
    }

    public static void UnPauseBGM()
    {
        BGMAudio.UnPause();
    }
}
