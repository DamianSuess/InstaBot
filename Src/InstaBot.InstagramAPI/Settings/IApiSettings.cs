namespace InstaBot.InstagramAPI.Settings
{
    public interface IApiSettings
    {
        string Url { get; set; }
        string Version { get; set; }
        string SignKey { get; set; }
        string SignKeyVersion { get; set; }
        string AndroidVersion { get; set; }
        string AndroidRelease { get; set; }
        string Experiments { get; set; }
        string LoginExperiments { get; set; }
    }
}
