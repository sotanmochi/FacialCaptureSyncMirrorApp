namespace FacialCaptureSync
{
    public sealed partial class Facemotion3d : IFacialCaptureSource
    {
        public bool UseIndirectConnection => false;

        public string HandshakeMessage => "UnityDirectConnect_FACEMOTION3D";
        public string StopStreamingMessage => "StopStreaming_FACEMOTION3D";

        public int HandshakePort => 49993;
        public int ReceiverPort => _receiverPort;

        public char[] Delimiters => _delimiters;

        public int BlendShapeCount => (int)BlendShapeName.BlendShapeCount;
        public int BoneCount => (int)BoneName.boneCount;
        public int BoneEulerAngleCount => BoneCount * 3;

        private readonly int _receiverPort;
        private readonly char[] _delimiters;

        public Facemotion3d(int directReceiverPort = 50003, char blendShapeNamePrefixDelimiter = '.')
        {
            _receiverPort = directReceiverPort;
            _delimiters = new char[]{'=', '|', '!', blendShapeNamePrefixDelimiter, '&', '|', '#', ','};
        }
    }
}