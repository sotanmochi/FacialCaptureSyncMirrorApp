using System;

#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
using Unity.Profiling;
#endif

namespace FacialCaptureSync
{
    public sealed partial class Facemotion3d : IFacialCaptureSource
    {
#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
        private static readonly ProfilerMarker _tryParseProfilerMarker = new ProfilerMarker($"{nameof(Facemotion3d)}.TryParse");
#endif

        public bool TryParse(string payload, ref FacialCapture output)
        {
#if UNITY_2020_3_OR_NEWER && DEVELOPMENT_BUILD || UNITY_EDITOR
            _tryParseProfilerMarker.Begin();
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
                        var index = FacialCaptureUtility.GetBlendShapeIndex(blendShapeName.ToString());
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
            _tryParseProfilerMarker.End();
#endif
            return true;
        }
    }
}