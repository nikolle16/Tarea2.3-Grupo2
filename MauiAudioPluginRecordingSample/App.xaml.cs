using MediaManager;

namespace MauiAudioPluginRecordingSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            base.OnStart();
            CrossMediaManager.Current.Init();
        }
    }
}
