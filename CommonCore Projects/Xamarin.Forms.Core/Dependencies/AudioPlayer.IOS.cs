#if __IOS__
using System;
using System.IO;
using AVFoundation;
using Foundation;
using MediaPlayer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(AudioPlayer))]
namespace Xamarin.Forms.Core
{
    public class AudioPlayer : IAudioPlayer
    {
        private AVAudioPlayer _audioPlayer = null;
        public Action OnFinishedPlaying { get; set; }

        public AudioPlayer()
        {
            var avSession = AVAudioSession.SharedInstance();
            avSession.SetCategory(AVAudioSessionCategory.Playback);

            NSError activationError = null;
            avSession.SetActive(true, out activationError);

            if (activationError != null)
                Console.WriteLine(
                    "Could not activate audio session {0}",
                    activationError.LocalizedDescription);
        }

        public void Play(string pathToAudioFile)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.FinishedPlaying -= Player_FinishedPlaying;
                _audioPlayer.Stop();
            }

            string localUrl = pathToAudioFile;
            _audioPlayer = AVAudioPlayer.FromUrl(NSUrl.FromFilename(localUrl));
            _audioPlayer.FinishedPlaying += Player_FinishedPlaying;
            _audioPlayer.Play();
        }

        private void Player_FinishedPlaying(object sender, AVStatusEventArgs e)
        {
            OnFinishedPlaying?.Invoke();
        }

        public void Pause()
        {
            _audioPlayer?.Pause();
        }

        public void Play()
        {
            _audioPlayer?.Play();
        }
    }
}
#endif
