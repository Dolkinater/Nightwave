using UnityEngine;
using System.Collections.Generic;

public class ManualAudioCall : MonoBehaviour
{
    [SerializeField] List<CallableSFX> audioClips;

    [System.Serializable]
    public struct CallableSFX 
    {
        public AudioClip[] audio;
        public string nameRef;
    }

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void CallSFXByName(string name)
    {
        int savedIndex = -1;

        for (int i = 0; i < audioClips.Count; i++)
        {
            if (audioClips[i].nameRef.Equals(name))
            {
                savedIndex = i;
                break;
            }
        }

        if (savedIndex == -1) { Debug.Log(name + " not found"); return; }

        int random = Random.Range(0, audioClips[savedIndex].audio.Length); // if multiple audio clips are attached to a single name, clips will be randomly selected

        audioSource.PlayOneShot(audioClips[savedIndex].audio[random]);
    }

    public void PlayLoopingAudio(bool value)
    {
        if (value) { audioSource.Play(); }
        else { audioSource.Stop(); }
    }
}
