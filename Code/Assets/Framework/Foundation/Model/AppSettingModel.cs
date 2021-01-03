using System;

namespace Assets.Framework.Foundation.Model
{
    [Serializable]
    public class AppSettingModel
    {
        public string Url;
        public string[] Scenes;

        public AppSettingModel()
        {
            Url = "";
        }
    }
}
