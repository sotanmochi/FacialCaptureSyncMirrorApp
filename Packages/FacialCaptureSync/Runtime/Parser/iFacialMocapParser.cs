using System;

#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
using Unity.Profiling;
#endif

namespace FacialCaptureSync
{
    public sealed partial class iFacialMocap : IFacialCaptureSource
    {
#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
        private static readonly ProfilerMarker _TryParseProfilerMarker = new ProfilerMarker($"{nameof(iFacialMocap)}.TryParse");
#endif

        public bool TryParse(string payload, ref FacialCapture output)
        {
#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
            _TryParseProfilerMarker.Begin();
#endif
            var delimiters = _delimiters;
            if (delimiters is null || delimiters.Length != 8) { return false; }

            var span = payload.AsSpan();

            var separationIndex = span.IndexOf(delimiters[0]);
            if (separationIndex < 0) { return false; }

            var blendShapes = span.Slice(0, separationIndex);
            var bonePoses = span.Slice(separationIndex + 1);

            int processedBlendShapeCount = 0;
            int processedPoseElementCount = 0;

            // ObjectGroup + BlendShape
            {
                int head = 0;
                for (int i = 0; i < (1 + BlendShapeCount); i++)
                {
                    //
                    // The range operator (..) is available in C# 8.0 and later.
                    // https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-
                    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-
                    //
                    var itemLength = blendShapes[head..].IndexOf(delimiters[1]);
                    if (itemLength <= 0) { break; }

                    var blendShape = blendShapes.Slice(head, itemLength);
                    head += itemLength + 1;

                    var objectGroupSeparationIndex = blendShape.IndexOf(delimiters[2]);
                    if (objectGroupSeparationIndex >= 0){ continue; } // Skip the face object group

                    var blendShapePrefixSeparationIndex = blendShape.IndexOf(delimiters[3]);
                    if (blendShapePrefixSeparationIndex >= 0)
                    {
                        blendShape = blendShape.Slice(blendShapePrefixSeparationIndex + 1); // Remove a prefix
                    }

                    var valueSeparationIndex = blendShape.IndexOf(delimiters[4]);
                    if (valueSeparationIndex <= 0) { continue; }

                    var blendShapeName = blendShape.Slice(0, valueSeparationIndex);
    #if UNITY_2021_2_OR_NEWER
                    if (short.TryParse(blendShape.Slice(valueSeparationIndex + 1), out var blendShapeValue))
    #else
                    if (short.TryParse(blendShape.Slice(valueSeparationIndex + 1).ToString(), out var blendShapeValue))
    #endif
                    {
                        var index = GetBlendShapeIndex(blendShapeName.ToString());
                        if (index < 0) { continue; }
                        output._blendShapeValues[index] = blendShapeValue;
                        processedBlendShapeCount++;
                    }
                }
            }

            // HeadPosition + BoneEulerAngles
            {
                var boneEulerAngles = output._boneEulerAngles.AsSpan<float>();
                int head = 0;
                for (int i = 0; i < (1 + BoneCount); i++)
                {
                    //
                    // The range operator (..) is available in C# 8.0 and later.
                    // https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-
                    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-
                    //
                    var itemLength = bonePoses[head..].IndexOf(delimiters[5]);
                    if (itemLength <= 0) { break; }

                    var bonePose = bonePoses.Slice(head, itemLength);
                    head += itemLength + 1;

                    var valueSeparationIndex = bonePose.IndexOf(delimiters[6]);
                    if (valueSeparationIndex <= 0) { continue; }

                    var boneName = bonePose.Slice(0, valueSeparationIndex);
                    var floatValues = bonePose.Slice(valueSeparationIndex + 1);

                    var index = FacialCaptureUtility.GetBoneIndex(boneName.ToString());
                    if (index < 0) { continue; }

                    var processedValueCount = ParseUtility.ParseFloatValues(floatValues, delimiters[7], boneEulerAngles.Slice(3*index, 3));
                    processedPoseElementCount += processedValueCount;
                }
            }

            if (processedBlendShapeCount == 0 && processedPoseElementCount == 0)
            {
                return false;
            }

#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
            _TryParseProfilerMarker.End();
#endif
            return true;
        }

        private static int GetBlendShapeIndex(string blendShapeName)
        {
            var index = blendShapeName switch
            {
                // For direct receiver
                nameof(InternalBlendShapeName.browDown_L)       => (int)InternalBlendShapeName.browDown_L,
                nameof(InternalBlendShapeName.browDown_R)       => (int)InternalBlendShapeName.browDown_R,
                nameof(InternalBlendShapeName.browInnerUp)      => (int)InternalBlendShapeName.browInnerUp,
                nameof(InternalBlendShapeName.browOuterUp_L)    => (int)InternalBlendShapeName.browOuterUp_L,
                nameof(InternalBlendShapeName.browOuterUp_R)    => (int)InternalBlendShapeName.browOuterUp_R,
                nameof(InternalBlendShapeName.cheekPuff)        => (int)InternalBlendShapeName.cheekPuff,
                nameof(InternalBlendShapeName.cheekSquint_L)    => (int)InternalBlendShapeName.cheekSquint_L,
                nameof(InternalBlendShapeName.cheekSquint_R)    => (int)InternalBlendShapeName.cheekSquint_R,
                nameof(InternalBlendShapeName.eyeBlink_L)       => (int)InternalBlendShapeName.eyeBlink_L,
                nameof(InternalBlendShapeName.eyeBlink_R)       => (int)InternalBlendShapeName.eyeBlink_R,
                nameof(InternalBlendShapeName.eyeLookDown_L)    => (int)InternalBlendShapeName.eyeLookDown_L,
                nameof(InternalBlendShapeName.eyeLookDown_R)    => (int)InternalBlendShapeName.eyeLookDown_R,
                nameof(InternalBlendShapeName.eyeLookIn_L)      => (int)InternalBlendShapeName.eyeLookIn_L,
                nameof(InternalBlendShapeName.eyeLookIn_R)      => (int)InternalBlendShapeName.eyeLookIn_R,
                nameof(InternalBlendShapeName.eyeLookOut_L)     => (int)InternalBlendShapeName.eyeLookOut_L,
                nameof(InternalBlendShapeName.eyeLookOut_R)     => (int)InternalBlendShapeName.eyeLookOut_R,
                nameof(InternalBlendShapeName.eyeLookUp_L)      => (int)InternalBlendShapeName.eyeLookUp_L,
                nameof(InternalBlendShapeName.eyeLookUp_R)      => (int)InternalBlendShapeName.eyeLookUp_R,
                nameof(InternalBlendShapeName.eyeSquint_L)      => (int)InternalBlendShapeName.eyeSquint_L,
                nameof(InternalBlendShapeName.eyeSquint_R)      => (int)InternalBlendShapeName.eyeSquint_R,
                nameof(InternalBlendShapeName.eyeWide_L)        => (int)InternalBlendShapeName.eyeWide_L,
                nameof(InternalBlendShapeName.eyeWide_R)        => (int)InternalBlendShapeName.eyeWide_R,
                nameof(InternalBlendShapeName.jawForward)       => (int)InternalBlendShapeName.jawForward,
                nameof(InternalBlendShapeName.jawLeft)          => (int)InternalBlendShapeName.jawLeft,
                nameof(InternalBlendShapeName.jawOpen)          => (int)InternalBlendShapeName.jawOpen,
                nameof(InternalBlendShapeName.jawRight)         => (int)InternalBlendShapeName.jawRight,
                nameof(InternalBlendShapeName.mouthClose)       => (int)InternalBlendShapeName.mouthClose,
                nameof(InternalBlendShapeName.mouthDimple_L)    => (int)InternalBlendShapeName.mouthDimple_L,
                nameof(InternalBlendShapeName.mouthDimple_R)    => (int)InternalBlendShapeName.mouthDimple_R,
                nameof(InternalBlendShapeName.mouthFrown_L)     => (int)InternalBlendShapeName.mouthFrown_L,
                nameof(InternalBlendShapeName.mouthFrown_R)     => (int)InternalBlendShapeName.mouthFrown_R,
                nameof(InternalBlendShapeName.mouthFunnel)      => (int)InternalBlendShapeName.mouthFunnel,
                nameof(InternalBlendShapeName.mouthLeft)        => (int)InternalBlendShapeName.mouthLeft,
                nameof(InternalBlendShapeName.mouthLowerDown_L) => (int)InternalBlendShapeName.mouthLowerDown_L,
                nameof(InternalBlendShapeName.mouthLowerDown_R) => (int)InternalBlendShapeName.mouthLowerDown_R,
                nameof(InternalBlendShapeName.mouthPress_L)     => (int)InternalBlendShapeName.mouthPress_L,
                nameof(InternalBlendShapeName.mouthPress_R)     => (int)InternalBlendShapeName.mouthPress_R,
                nameof(InternalBlendShapeName.mouthPucker)      => (int)InternalBlendShapeName.mouthPucker,
                nameof(InternalBlendShapeName.mouthRight)       => (int)InternalBlendShapeName.mouthRight,
                nameof(InternalBlendShapeName.mouthRollLower)   => (int)InternalBlendShapeName.mouthRollLower,
                nameof(InternalBlendShapeName.mouthRollUpper)   => (int)InternalBlendShapeName.mouthRollUpper,
                nameof(InternalBlendShapeName.mouthShrugLower)  => (int)InternalBlendShapeName.mouthShrugLower,
                nameof(InternalBlendShapeName.mouthShrugUpper)  => (int)InternalBlendShapeName.mouthShrugUpper,
                nameof(InternalBlendShapeName.mouthSmile_L)     => (int)InternalBlendShapeName.mouthSmile_L,
                nameof(InternalBlendShapeName.mouthSmile_R)     => (int)InternalBlendShapeName.mouthSmile_R,
                nameof(InternalBlendShapeName.mouthStretch_L)   => (int)InternalBlendShapeName.mouthStretch_L,
                nameof(InternalBlendShapeName.mouthStretch_R)   => (int)InternalBlendShapeName.mouthStretch_R,
                nameof(InternalBlendShapeName.mouthUpperUp_L)   => (int)InternalBlendShapeName.mouthUpperUp_L,
                nameof(InternalBlendShapeName.mouthUpperUp_R)   => (int)InternalBlendShapeName.mouthUpperUp_R,
                nameof(InternalBlendShapeName.noseSneer_L)      => (int)InternalBlendShapeName.noseSneer_L,
                nameof(InternalBlendShapeName.noseSneer_R)      => (int)InternalBlendShapeName.noseSneer_R,
                nameof(InternalBlendShapeName.tongueOut)        => (int)InternalBlendShapeName.tongueOut,

                // For indirect receiver
                nameof(BlendShapeNameInCamelCase.browDownLeft)        => (int)BlendShapeNameInCamelCase.browDownLeft,
                nameof(BlendShapeNameInCamelCase.browDownRight)       => (int)BlendShapeNameInCamelCase.browDownRight,
                // nameof(BlendShapeNameInCamelCase.browInnerUp)         => (int)BlendShapeNameInCamelCase.browInnerUp,
                nameof(BlendShapeNameInCamelCase.browOuterUpLeft)     => (int)BlendShapeNameInCamelCase.browOuterUpLeft,
                nameof(BlendShapeNameInCamelCase.browOuterUpRight)    => (int)BlendShapeNameInCamelCase.browOuterUpRight,
                // nameof(BlendShapeNameInCamelCase.cheekPuff)           => (int)BlendShapeNameInCamelCase.cheekPuff,
                nameof(BlendShapeNameInCamelCase.cheekSquintLeft)     => (int)BlendShapeNameInCamelCase.cheekSquintLeft,
                nameof(BlendShapeNameInCamelCase.cheekSquintRight)    => (int)BlendShapeNameInCamelCase.cheekSquintRight,
                nameof(BlendShapeNameInCamelCase.eyeBlinkLeft)        => (int)BlendShapeNameInCamelCase.eyeBlinkLeft,
                nameof(BlendShapeNameInCamelCase.eyeBlinkRight)       => (int)BlendShapeNameInCamelCase.eyeBlinkRight,
                nameof(BlendShapeNameInCamelCase.eyeLookDownLeft)     => (int)BlendShapeNameInCamelCase.eyeLookDownLeft,
                nameof(BlendShapeNameInCamelCase.eyeLookDownRight)    => (int)BlendShapeNameInCamelCase.eyeLookDownRight,
                nameof(BlendShapeNameInCamelCase.eyeLookInLeft)       => (int)BlendShapeNameInCamelCase.eyeLookInLeft,
                nameof(BlendShapeNameInCamelCase.eyeLookInRight)      => (int)BlendShapeNameInCamelCase.eyeLookInRight,
                nameof(BlendShapeNameInCamelCase.eyeLookOutLeft)      => (int)BlendShapeNameInCamelCase.eyeLookOutLeft,
                nameof(BlendShapeNameInCamelCase.eyeLookOutRight)     => (int)BlendShapeNameInCamelCase.eyeLookOutRight,
                nameof(BlendShapeNameInCamelCase.eyeLookUpLeft)       => (int)BlendShapeNameInCamelCase.eyeLookUpLeft,
                nameof(BlendShapeNameInCamelCase.eyeLookUpRight)      => (int)BlendShapeNameInCamelCase.eyeLookUpRight,
                nameof(BlendShapeNameInCamelCase.eyeSquintLeft)       => (int)BlendShapeNameInCamelCase.eyeSquintLeft,
                nameof(BlendShapeNameInCamelCase.eyeSquintRight)      => (int)BlendShapeNameInCamelCase.eyeSquintRight,
                nameof(BlendShapeNameInCamelCase.eyeWideLeft)         => (int)BlendShapeNameInCamelCase.eyeWideLeft,
                nameof(BlendShapeNameInCamelCase.eyeWideRight)        => (int)BlendShapeNameInCamelCase.eyeWideRight,
                // nameof(BlendShapeNameInCamelCase.jawForward)          => (int)BlendShapeNameInCamelCase.jawForward,
                // nameof(BlendShapeNameInCamelCase.jawLeft)             => (int)BlendShapeNameInCamelCase.jawLeft,
                // nameof(BlendShapeNameInCamelCase.jawOpen)             => (int)BlendShapeNameInCamelCase.jawOpen,
                // nameof(BlendShapeNameInCamelCase.jawRight)            => (int)BlendShapeNameInCamelCase.jawRight,
                // nameof(BlendShapeNameInCamelCase.mouthClose)          => (int)BlendShapeNameInCamelCase.mouthClose,
                nameof(BlendShapeNameInCamelCase.mouthDimpleLeft)     => (int)BlendShapeNameInCamelCase.mouthDimpleLeft,
                nameof(BlendShapeNameInCamelCase.mouthDimpleRight)    => (int)BlendShapeNameInCamelCase.mouthDimpleRight,
                nameof(BlendShapeNameInCamelCase.mouthFrownLeft)      => (int)BlendShapeNameInCamelCase.mouthFrownLeft,
                nameof(BlendShapeNameInCamelCase.mouthFrownRight)     => (int)BlendShapeNameInCamelCase.mouthFrownRight,
                // nameof(BlendShapeNameInCamelCase.mouthFunnel)         => (int)BlendShapeNameInCamelCase.mouthFunnel,
                // nameof(BlendShapeNameInCamelCase.mouthLeft)           => (int)BlendShapeNameInCamelCase.mouthLeft,
                nameof(BlendShapeNameInCamelCase.mouthLowerDownLeft)  => (int)BlendShapeNameInCamelCase.mouthLowerDownLeft,
                nameof(BlendShapeNameInCamelCase.mouthLowerDownRight) => (int)BlendShapeNameInCamelCase.mouthLowerDownRight,
                nameof(BlendShapeNameInCamelCase.mouthPressLeft)      => (int)BlendShapeNameInCamelCase.mouthPressLeft,
                nameof(BlendShapeNameInCamelCase.mouthPressRight)     => (int)BlendShapeNameInCamelCase.mouthPressRight,
                // nameof(BlendShapeNameInCamelCase.mouthPucker)         => (int)BlendShapeNameInCamelCase.mouthPucker,
                // nameof(BlendShapeNameInCamelCase.mouthRight)          => (int)BlendShapeNameInCamelCase.mouthRight,
                // nameof(BlendShapeNameInCamelCase.mouthRollLower)      => (int)BlendShapeNameInCamelCase.mouthRollLower,
                // nameof(BlendShapeNameInCamelCase.mouthRollUpper)      => (int)BlendShapeNameInCamelCase.mouthRollUpper,
                // nameof(BlendShapeNameInCamelCase.mouthShrugLower)     => (int)BlendShapeNameInCamelCase.mouthShrugLower,
                // nameof(BlendShapeNameInCamelCase.mouthShrugUpper)     => (int)BlendShapeNameInCamelCase.mouthShrugUpper,
                nameof(BlendShapeNameInCamelCase.mouthSmileLeft)      => (int)BlendShapeNameInCamelCase.mouthSmileLeft,
                nameof(BlendShapeNameInCamelCase.mouthSmileRight)     => (int)BlendShapeNameInCamelCase.mouthSmileRight,
                nameof(BlendShapeNameInCamelCase.mouthStretchLeft)    => (int)BlendShapeNameInCamelCase.mouthStretchLeft,
                nameof(BlendShapeNameInCamelCase.mouthStretchRight)   => (int)BlendShapeNameInCamelCase.mouthStretchRight,
                nameof(BlendShapeNameInCamelCase.mouthUpperUpLeft)    => (int)BlendShapeNameInCamelCase.mouthUpperUpLeft,
                nameof(BlendShapeNameInCamelCase.mouthUpperUpRight)   => (int)BlendShapeNameInCamelCase.mouthUpperUpRight,
                nameof(BlendShapeNameInCamelCase.noseSneerLeft)       => (int)BlendShapeNameInCamelCase.noseSneerLeft,
                nameof(BlendShapeNameInCamelCase.noseSneerRight)      => (int)BlendShapeNameInCamelCase.noseSneerRight,
                // nameof(BlendShapeNameInCamelCase.tongueOut)           => (int)BlendShapeNameInCamelCase.tongueOut,

                _ => -1,
            };
            return index;
        }
    }
}