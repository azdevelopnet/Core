#if __ANDROID__
using System;
using Android.Media;
using Xamarin.Forms;
using Content = Android.Content;
using Xamarin.Forms.Core;
using Plugin.CurrentActivity;
using Android.Content;

[assembly: Dependency(typeof(AudioPlayer))]
namespace Xamarin.Forms.Core
{
	public class AudioPlayer : IAudioPlayer
	{
		private MediaPlayer _mediaPlayer;

		public Action OnFinishedPlaying { get; set; }

        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

		public void Play(string pathToSoundName)
		{
			if (_mediaPlayer != null)
			{
				_mediaPlayer.Completion -= MediaPlayer_Completion;
				_mediaPlayer.Stop();
			}

			var fullPath = pathToSoundName;

			Content.Res.AssetFileDescriptor afd = null;

			try
			{
				afd = Ctx.Assets.OpenFd(fullPath);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error openfd: " + ex);
			}
			if (afd != null)
			{
				System.Diagnostics.Debug.WriteLine("Length " + afd.Length);
				if (_mediaPlayer == null)
				{
					_mediaPlayer = new MediaPlayer();
					_mediaPlayer.Prepared += (sender, args) =>
					{
						_mediaPlayer.Start();
						_mediaPlayer.Completion += MediaPlayer_Completion;
					};
				}

				_mediaPlayer.Reset();
				_mediaPlayer.SetVolume(1.0f, 1.0f);

				_mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
				_mediaPlayer.PrepareAsync();
			}
		}

		void MediaPlayer_Completion(object sender, EventArgs e)
		{
			OnFinishedPlaying?.Invoke();
		}

		public void Pause()
		{
			_mediaPlayer?.Pause();
		}

		public void Play()
		{
			_mediaPlayer?.Start();
		}
	}
}
#endif
