using MediaManager;
using MediaManager.Playback;

namespace MauiAudioPluginRecordingSample.Views;

public partial class PageList : ContentPage
{
    public PageList()
    {
        InitializeComponent();
    }

    private void OnPlayButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var audioPath = button?.CommandParameter as string;
        if (!string.IsNullOrEmpty(audioPath))
        {
            CrossMediaManager.Current.Play(audioPath);
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        List<Models.Audio> audiolist = new List<Models.Audio>();
        audiolist = await Controllers.AudioController.Get();
        listaudio.ItemsSource = audiolist;
    }
}

