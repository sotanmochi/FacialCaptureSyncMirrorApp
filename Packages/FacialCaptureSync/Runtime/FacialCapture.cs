using System;

#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
using Unity.Profiling;
#endif

namespace FacialCaptureSync
{
    /// <summary>
    /// Facial capture data for iFacialMocap and Facemotion3d.<br/>
    /// https://www.ifacialmocap.com/<br/>
    /// https://www.facemotion3d.info/
    /// </summary>
    public sealed class FacialCapture
    {
        public static readonly int BlendShapeCount = (int)BlendShapeName.BlendShapeCount;
        public static readonly int BoneCount = (int)BoneName.boneCount;
        public static readonly int BoneEulerAngleCount = BoneCount * 3;

        /// <summary>
        /// The value of a BlendShape is expressed as an integer value in the range of 0 to 100.
        /// </summary>
        public short[] BlendShapeValues => _blendShapeValues;
        internal readonly short[] _blendShapeValues = new short[BlendShapeCount];

        /// <summary>
        /// BoneEulerAngles[3*k + 0]: EulerAngles.X<br/>
        /// BoneEulerAngles[3*k + 1]: EulerAngles.Y<br/>
        /// BoneEulerAngles[3*k + 2]: EulerAngles.Z<br/>
        /// where k = 0, 1, ..., BoneCount.<br/>
        /// </summary>
        public float[] BoneEulerAngles => _boneEulerAngles; // BoneOrientation
        internal readonly float[] _boneEulerAngles = new float[BoneEulerAngleCount];

#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
        private static readonly ProfilerMarker _CopyToProfilerMarker = new ProfilerMarker($"{nameof(FacialCapture)}.CopyTo");
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capture"></param>
        public void CopyTo(FacialCapture capture)
        {
#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
            _CopyToProfilerMarker.Begin();
#endif
            Array.Copy(_blendShapeValues, capture._blendShapeValues, BlendShapeCount);
            Array.Copy(_boneEulerAngles, capture._boneEulerAngles, BoneEulerAngleCount);

#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
            _CopyToProfilerMarker.End();
#endif
        }
    }
}