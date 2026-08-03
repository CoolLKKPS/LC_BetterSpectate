namespace BetterSpectate.Patches
{
    internal static class SpectateUtils
    {
        public static float ZoomClamp(this float value)
        {
            bool flag = SpectateUtils.maxZoomDistance > SpectateUtils.minZoomDistance;
            float num;
            if (flag)
            {
                bool flag2 = value > SpectateUtils.maxZoomDistance;
                if (flag2)
                {
                    num = SpectateUtils.maxZoomDistance;
                }
                else
                {
                    bool flag3 = value < SpectateUtils.minZoomDistance;
                    if (flag3)
                    {
                        num = SpectateUtils.minZoomDistance;
                    }
                    else
                    {
                        num = value;
                    }
                }
            }
            else
            {
                num = PlayerControllerB_Patch.GetZoomDistance();
            }
            return num;
        }

        public static float GetMaxZoom()
        {
            return SpectateUtils.maxZoomDistance;
        }

        public static void SetMaxZoom(float value)
        {
            SpectateUtils.maxZoomDistance = value;
        }

        public static float GetMinZoom()
        {
            return SpectateUtils.minZoomDistance;
        }

        public static void SetMinZoom(float value)
        {
            SpectateUtils.minZoomDistance = value;
        }

        private static float maxZoomDistance;

        private static float minZoomDistance;
    }
}
