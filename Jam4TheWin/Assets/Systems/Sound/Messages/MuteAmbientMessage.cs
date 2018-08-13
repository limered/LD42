
namespace System.Sound.Messages
{
    public class MuteAmbientMessage
    {
        public MuteAmbientMessage(bool mute)
        {
            _mute = mute;
        }
        private readonly bool _mute;
        public bool Mute { get { return _mute; } }
    }
}