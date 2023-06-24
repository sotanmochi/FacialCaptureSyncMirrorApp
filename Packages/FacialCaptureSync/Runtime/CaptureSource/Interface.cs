namespace FacialCaptureSync
{
    public interface IFacialCaptureSource
    {
        public bool UseIndirectConnection { get; }

        public string HandshakeMessage { get; }
        public string StopStreamingMessage { get; }

        public int HandshakePort { get; }
        public int ReceiverPort { get; }

        public char[] Delimiters { get; }

        public int BlendShapeCount { get; }
        public int BoneCount { get; }
        public int BoneEulerAngleCount { get; }

        bool TryParse(string payload, ref FacialCapture output);
    }
}