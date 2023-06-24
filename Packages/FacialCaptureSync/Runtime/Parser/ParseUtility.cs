using System;

namespace FacialCaptureSync
{
    public sealed class ParseUtility
    {
        public static int ParseFloatValues(ReadOnlySpan<char> src, char delimiter, Span<float> dst)
        {
            var processedValueCount = 0;

            int valueHead = 0;
            for (int k = 0; k < dst.Length - 1; k++)
            {
                var valueLength = src[valueHead..].IndexOf(delimiter);
                if (valueLength <= 0) { break; }

                var value = src.Slice(valueHead, valueLength);

#if UNITY_2021_2_OR_NEWER
                if (float.TryParse(value, out var floatValue))
#else
                if (float.TryParse(value.ToString(), out var floatValue))
#endif
                {
                    dst[k] = floatValue;
                    processedValueCount++;
                }

                valueHead += valueLength + 1;
            }

            var lastIndex = dst.Length - 1;
            var lastValueLength = src[valueHead..].IndexOf(delimiter);
            var lastValue = (lastValueLength <= 0) ? src.Slice(valueHead) : src.Slice(valueHead, lastValueLength);

#if UNITY_2021_2_OR_NEWER
            if (float.TryParse(lastValue, out var lastFloatValue))
#else
            if (float.TryParse(lastValue.ToString(), out var lastFloatValue))
#endif
            {
                dst[lastIndex] = lastFloatValue;
                processedValueCount++;
            }

            return processedValueCount;
        }
    }
}