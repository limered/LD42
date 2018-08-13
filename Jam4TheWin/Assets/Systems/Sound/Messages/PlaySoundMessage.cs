
namespace System.Sound.Messages
{
    public class PlaySoundMessage
    {
        public PlaySoundMessage(string name, string tag = null)
        {
            _name = name;
        }
        private readonly string _name;
        private readonly string _tag;
        public string Name { get { return _name; } }
        public string Tag { get { return _tag; } }
    }
}