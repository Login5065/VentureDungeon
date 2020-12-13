using Dungeon.Variables;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon.Audio
{
    public static class SoundLibrary
    {
        public static Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>()
        {
            {"Day", Resources.Load("ost/01_LOOP_ROLLING_SLEEVES") as AudioClip},
            {"Night", Resources.Load("ost/01_LOOP_NOIR") as AudioClip},
            {"Move", Resources.Load("sfx/Collect/collect-5") as AudioClip}
        };
    }

    public static class MusicManager
    {
        static string playing = "";
        static readonly AudioSource musicSource = Camera.main.gameObject.GetComponent<AudioSource>();
        public static bool Play(string song)
        {
            if (song == playing) return false;
            else
            {
                if (SoundLibrary.sounds.TryGetValue(song, out var s))
                {
                    musicSource.Stop();
                    playing = song;
                    musicSource.loop = true;
                    musicSource.clip = s;
                    musicSource.Play();
                    return true;
                }
                return false;
            }
        }
        public static void Stop()
        {
            musicSource.Stop();
            playing = "";
        }
    }
}