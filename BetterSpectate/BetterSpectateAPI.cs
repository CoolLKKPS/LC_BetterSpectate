using BetterSpectate.Patches;

namespace BetterSpectate
{
    public class BetterSpectateAPI
    {
        public bool isModLoaded
        {
            get
            {
                return BetterSpectateBase.instance != null;
            }
        }

        public bool isFirstPersonSpectateEnabled
        {
            get
            {
                return PlayerControllerB_Patch.GetFirstPersonEnabled();
            }
        }

        public bool isZoomEnabled
        {
            get
            {
                return PlayerControllerB_Patch.GetZoomEnabled();
            }
        }

        public bool isFirstPersonSpectating
        {
            get
            {
                return PlayerControllerB_Patch.IsPlayerInFirstPerson();
            }
        }

        public float zoomDistance
        {
            get
            {
                return PlayerControllerB_Patch.GetZoomDistance();
            }
        }

        public float zoomSpeed
        {
            get
            {
                return PlayerControllerB_Patch.GetZoomSpeed();
            }
        }

        public float maxZoom
        {
            get
            {
                return SpectateUtils.GetMaxZoom();
            }
        }

        public float minZoom
        {
            get
            {
                return SpectateUtils.GetMinZoom();
            }
        }
    }
}
