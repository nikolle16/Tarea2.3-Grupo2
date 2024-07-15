using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using System.Text;


namespace MauiAudioPluginRecordingSample
{
    public partial class MainPage : ContentPage
    {
        readonly IAudioManager _audioManager;
        readonly IAudioRecorder _audioRecorder;
        Controllers.AudioController controller;
        int _fileCounter;

        public MainPage(IAudioManager audioManager)
        {
            InitializeComponent();

            controller = new Controllers.AudioController();

            _audioManager = audioManager;
            _audioRecorder = audioManager.CreateRecorder();
        }

        private async void OnStartRecordingClicked(object sender, EventArgs e)
        {
            if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied", "Microphone permission is required to record audio.", "OK");
                return;
            }

            if (!_audioRecorder.IsRecording)
            {
                await _audioRecorder.StartAsync();
                StartRecordingButton.IsVisible = false;
                StopRecordingButton.IsVisible = true;
            }
        }

        private async void OnStopRecordingClicked(object sender, EventArgs e)
        {
            if (_audioRecorder.IsRecording)
            {
                var recordedAudio = await _audioRecorder.StopAsync();

                var audioStream = recordedAudio.GetAudioStream();
                string audiostring = await ConvertStreamToString(audioStream);

                if (string.IsNullOrEmpty(audiostring))
                {
                    await DisplayAlert("Error", "El audio no se pudo cargar.", "OK");
                    return;
                }

                var audio = new Models.Audio
                {
                    audio = audiostring,
                    fecha = DateTime.Now
                };

                try
                {
                    if (controller != null)
                    {
                        if (await Controllers.AudioController.Create(audio) > 0)
                        {
                            await DisplayAlert("Aviso", "Registro Ingresado con Exito!", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Error", "Ocurrio un Error", "OK");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrio un Error: {ex.Message}", "OK");
                }

                StartRecordingButton.IsVisible = true;
                StopRecordingButton.IsVisible = false;
            }
        }

        public async Task<string> ConvertStreamToString(Stream audioStream)
        {
            if (audioStream == null)
            {
                await DisplayAlert("Error", "El stream de audio es nulo.", "OK");
                return null;
            }

            if (!audioStream.CanRead)
            {
                await DisplayAlert("Error", "El stream de audio no se puede leer.", "OK");
                return null;
            }

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await audioStream.CopyToAsync(memoryStream);

                    byte[] byteArray = memoryStream.ToArray();
                    string audioString = Encoding.UTF8.GetString(byteArray);

                    if (string.IsNullOrEmpty(audioString))
                    {
                        await DisplayAlert("Advertencia", "El stream de audio convertido está vacío.", "OK");
                    }

                    return audioString;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al convertir el stream de audio: {ex.Message}", "OK");
                return null;
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.PageList());
        }
    }
}
