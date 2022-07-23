using NOSTALE.CONFIG.ApplyConfig;
using OpenNos.GameObject.Networking;
using NOSTALE.CONFIG.Config;

namespace OpenNos.GameObject.ConfigEXT
{
    public static class ConfigurationExtension
    {
        public static RateItem RateItem(this ServerManager e)
        {
            if (e.ServerGroup == "S3-Nosmonster")
            {
                return ServerConfigurationS3.Instance.RateItem;
            }

            if (e.ServerGroup == "S2-Nosmonster")
            {
                return ServerConfigurationS2.Instance.RateItem;
            }

            return ServerConfigurationS1.Instance.RateItem;
        }
    }
}
