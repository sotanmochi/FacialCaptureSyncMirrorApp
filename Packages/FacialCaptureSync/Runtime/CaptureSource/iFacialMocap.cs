namespace FacialCaptureSync
{
    public sealed partial class iFacialMocap : IFacialCaptureSource
    {
        public bool UseIndirectConnection => _useIndirectConnection;

        public string HandshakeMessage => "iFacialMocap_sahuasouryya9218sauhuiayeta91555dy3719";
        public string StopStreamingMessage => "";

        public int HandshakePort => 49983;
        public int ReceiverPort => _receiverPort;

        public char[] Delimiters => _delimiters;

        public int BlendShapeCount => (int)BlendShapeName.BlendShapeCount;
        public int BoneCount => (int)BoneName.boneCount;
        public int BoneEulerAngleCount => BoneCount * 3;

        private readonly bool _useIndirectConnection;
        private readonly int _receiverPort;
        private readonly char[] _delimiters;

        private static readonly int _directReceiverPort = 49983;

        public iFacialMocap(bool useIndirectConnection = false, int indirectReceiverPort = 50003, char blendShapeNamePrefixDelimiter = '.')
        {
            _receiverPort = useIndirectConnection ? indirectReceiverPort : _directReceiverPort;
            _delimiters = new char[]{'=', '|', '!', blendShapeNamePrefixDelimiter, '-', '|', '#', ','};
            _useIndirectConnection = useIndirectConnection;
        }
    }
}