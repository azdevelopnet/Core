using System;
namespace Xamarin.Forms.Core
{
    public interface IAudioPlayer
    {
        /// <summary>
        /// Play the specified AudioFile. Android Assests folder / IOS Resources folder
        /// </summary>
        /// <returns>The play.</returns>
        /// <param name="pathToAudioFile">Path to audio file.</param>
        void Play(string pathToAudioFile);
        void Play();
        void Pause();
        Action OnFinishedPlaying { get; set; }
    }
}
