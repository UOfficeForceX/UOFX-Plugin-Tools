
using Newtonsoft.Json;

namespace SchedulerSample
{
    public class SyncService
    {
        // 設定檔
        private SettingModel _sampleSetting;

        public async void Run()
        {
            string projectPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // 取得設定檔
            await GetSetting(projectPath);
            foreach (var setting in _sampleSetting.SchedulerSampleSetting)
            {
                Console.WriteLine($"排程發出的訊息: {setting.Version}");
            }
        }

        /// <summary>
        ///  取得設定檔
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task GetSetting(string path)
        {
            string jsonString = await System.IO.File.ReadAllTextAsync(Path.Combine(path, "appsettings.json"));
            if (!string.IsNullOrEmpty(jsonString))
            {
                _sampleSetting = JsonConvert.DeserializeObject<SettingModel>(jsonString);
            }
        }
    }

    public class SettingModel
    {
        public ContentModel[] SchedulerSampleSetting { get; set; }
    }

    public class ContentModel
    {
        public string Version { get; set; }
    }
}
