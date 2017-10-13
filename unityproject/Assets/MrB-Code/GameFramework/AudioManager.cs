using System.Collections;
using System.Linq;
using Prime31.ZestKit;
using UnityEngine;

namespace Assets.GameFramework
{
    public class AudioManager : SingletonPrefab<AudioManager>
    {
        public enum FxType {Click, Swipe, Armonica, Snoring, Bip, Pop}
        public enum TrackType {None, Crickets, DeltaAgent, Lounge, Driving}

        public AudioSource FxClick;
        public AudioSource FxSwipe;
        public AudioSource FxArmonica;
        public AudioSource FxSnoring;
        public AudioSource FxBip;
        public AudioSource FxPop;
        public AudioSource TrackCrickets;
        public AudioSource TrackDeltaAgent;
        public AudioSource TrackLounge;
        public AudioSource TrackDriving;

        private AudioSource _currentBackgroundTrack;

        public void Awake()
        {
            ZestKit.instance.EnsureZestKitIsProperlyConfigured();
        }

        public void PlayBackgroundMusic(TrackType track)
        {
            const float fadeOut = 1;

            var audioToPlay = ToSource(track);
            var audioToFadeOut = _currentBackgroundTrack;

            if (audioToFadeOut == audioToPlay && audioToPlay != null)
            {
                Debug.Log("requested audiosource " + audioToPlay.clip.name + " is already playing");
                return;
            }

            if (audioToFadeOut == null)
            {
                StartAudioSourceInBackground(audioToPlay);
            }
            else
            {                
                audioToFadeOut
                    .ZKvolumeTo(0, fadeOut)
                    .setCompletionHandler(tween =>
                    {
                        audioToFadeOut.clip.UnloadAudioData();
                        StartAudioSourceInBackground(audioToPlay);
                    })
                    .start();
            }            
        }

        private void StartAudioSourceInBackground(AudioSource audioSource)
        {
            if (audioSource == null)
            {
                _currentBackgroundTrack = null;
                return;
            }

            Debug.Log("playing background music " + audioSource.clip.name);
            audioSource.volume = 1;
            audioSource.Play();
            _currentBackgroundTrack = audioSource;
        }

        private AudioSource ToSource(TrackType track)
        {
            if (track == TrackType.None) return null;
            if (track == TrackType.Crickets) return TrackCrickets;
            if (track == TrackType.DeltaAgent) return TrackDeltaAgent;
            if (track == TrackType.Lounge) return TrackLounge;
            if (track == TrackType.Driving) return TrackDriving;
            return null;
        }

        public void PlayFx(FxType fx)
        {
            if (fx == FxType.Click) FxClick.Play();
            if (fx == FxType.Swipe) FxSwipe.Play();
            if (fx == FxType.Armonica) FxArmonica.Play();
            if (fx == FxType.Snoring) FxSnoring.Play();
            if (fx == FxType.Bip) FxBip.Play();
            if (fx == FxType.Pop) FxPop.Play();
        }

        public void FadeBackgroundMusicToZero()
        {            
            FadeOutAndUnloadAudioSource(_currentBackgroundTrack, 2f);
        }

        private static void FadeOutAndUnloadAudioSource(AudioSource audioSourceToFade, float fadeDurationInSeconds)
        {
            if (audioSourceToFade == null)
                return;

            audioSourceToFade
                .ZKvolumeTo(0, fadeDurationInSeconds)
                .setCompletionHandler(tween =>
                {
                    audioSourceToFade.clip.UnloadAudioData();
                })
                .start();
        }        
    }
}