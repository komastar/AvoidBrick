using System;

namespace Assets.Framework.Foundation.Model
{
    public class AssetBundleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Project { get; set; }
        public string Branch { get; set; }
        public string BundleVersionCode { get; set; }
        public string BundleVersion { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}