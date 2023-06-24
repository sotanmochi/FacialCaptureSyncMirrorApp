using System;
using NUnit.Framework;

namespace FacialCaptureSync.Tests
{
    public class FacialCaptureTest
    {
        [Test]
        public void BlendShapeNameTest()
        {
            var blendShapeNames = (BlendShapeName[])Enum.GetValues(typeof(BlendShapeName));
            foreach (var blendShapeName in blendShapeNames)
            {
                var index1 = FacialCaptureUtility.GetBlendShapeIndex(blendShapeName);
                var index2 = FacialCaptureUtility.GetBlendShapeIndex(blendShapeName.ToString());
                Assert.That(index1, Is.EqualTo(index2));
            }
        }

        [Test]
        public void ParseTest_iFacialMocap()
        {
            var dataSource = new iFacialMocap();

            var capture = new FacialCapture();
            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                capture.BlendShapeValues[i] = -1;
            }

            var success = dataSource.TryParse(_payload_iFacialMocap, ref capture);
            Assert.That(success, Is.EqualTo(true));

            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                Assert.That(capture.BlendShapeValues[i], Is.EqualTo(i));
            }

            var blendShapeNames = (BlendShapeName[])Enum.GetValues(typeof(BlendShapeName));
            for (int i = 0; i < (int)BlendShapeName.BlendShapeCount; i++)
            {
                var index = FacialCaptureUtility.GetBlendShapeIndex(blendShapeNames[i]);
                Assert.That(capture.BlendShapeValues[index], Is.EqualTo(index));
            }

            // Head
            Assert.That(capture.BoneEulerAngles[0], Is.EqualTo(-21.488958f));
            Assert.That(capture.BoneEulerAngles[1], Is.EqualTo(-6.038993f));
            Assert.That(capture.BoneEulerAngles[2], Is.EqualTo(-6.6019735f));
            // RightEye
            Assert.That(capture.BoneEulerAngles[3], Is.EqualTo(6.0297494f));
            Assert.That(capture.BoneEulerAngles[4], Is.EqualTo(2.4403017f));
            Assert.That(capture.BoneEulerAngles[5], Is.EqualTo(0.25649446f));
            // LeftEye
            Assert.That(capture.BoneEulerAngles[6], Is.EqualTo(6.034903f));
            Assert.That(capture.BoneEulerAngles[7], Is.EqualTo(-1.6660284f));
            Assert.That(capture.BoneEulerAngles[8], Is.EqualTo(-0.17520553f));
        }

        [Test]
        public void ParseTest_iFacialMocap_IndirectConnection()
        {
            var dataSource = new iFacialMocap(true);

            var capture = new FacialCapture();
            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                capture.BlendShapeValues[i] = -1;
            }

            var success = dataSource.TryParse(_payload_iFacialMocap_IndirectConnection, ref capture);
            Assert.That(success, Is.EqualTo(true));

            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                Assert.That(capture.BlendShapeValues[i], Is.EqualTo(i));
            }

            var blendShapeNames = (BlendShapeName[])Enum.GetValues(typeof(BlendShapeName));
            for (int i = 0; i < (int)BlendShapeName.BlendShapeCount; i++)
            {
                var index = FacialCaptureUtility.GetBlendShapeIndex(blendShapeNames[i]);
                Assert.That(capture.BlendShapeValues[index], Is.EqualTo(index));
            }

            // Head
            Assert.That(capture.BoneEulerAngles[0], Is.EqualTo(-6.567929f));
            Assert.That(capture.BoneEulerAngles[1], Is.EqualTo(-3.8523278f));
            Assert.That(capture.BoneEulerAngles[2], Is.EqualTo(-0.45081988f));
            // RightEye
            Assert.That(capture.BoneEulerAngles[3], Is.EqualTo(-5.039491f));
            Assert.That(capture.BoneEulerAngles[4], Is.EqualTo(1.0846136f));
            Assert.That(capture.BoneEulerAngles[5], Is.EqualTo(0.0f));
            // LeftEye
            Assert.That(capture.BoneEulerAngles[6], Is.EqualTo(-5.0845714f));
            Assert.That(capture.BoneEulerAngles[7], Is.EqualTo(0.75621915f));
            Assert.That(capture.BoneEulerAngles[8], Is.EqualTo(0.0f));
            // Neck
            Assert.That(capture.BoneEulerAngles[9], Is.EqualTo(-4.59755f));
            Assert.That(capture.BoneEulerAngles[10], Is.EqualTo(-2.6966295f));
            Assert.That(capture.BoneEulerAngles[11], Is.EqualTo(-0.3155739f));
            // Spine
            Assert.That(capture.BoneEulerAngles[12], Is.EqualTo(-1.9703788f));
            Assert.That(capture.BoneEulerAngles[13], Is.EqualTo(-1.1556984f));
            Assert.That(capture.BoneEulerAngles[14], Is.EqualTo(-0.13524596f));
        }

        [Test]
        public void ParseTest_Facemotion3d()
        {
            var dataSource = new Facemotion3d();

            var capture = new FacialCapture();
            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                capture.BlendShapeValues[i] = -1;
            }

            var success = dataSource.TryParse(_payload_Facemotion3d, ref capture);
            Assert.That(success, Is.EqualTo(true));

            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                Assert.That(capture.BlendShapeValues[i], Is.EqualTo(i));
            }

            var blendShapeNames = (BlendShapeName[])Enum.GetValues(typeof(BlendShapeName));
            for (int i = 0; i < (int)BlendShapeName.BlendShapeCount; i++)
            {
                var index = FacialCaptureUtility.GetBlendShapeIndex(blendShapeNames[i]);
                Assert.That(capture.BlendShapeValues[index], Is.EqualTo(index));
            }

            // Head
            Assert.That(capture.BoneEulerAngles[0], Is.EqualTo(-6.567929f));
            Assert.That(capture.BoneEulerAngles[1], Is.EqualTo(-3.8523278f));
            Assert.That(capture.BoneEulerAngles[2], Is.EqualTo(-0.45081988f));
            // RightEye
            Assert.That(capture.BoneEulerAngles[3], Is.EqualTo(-5.039491f));
            Assert.That(capture.BoneEulerAngles[4], Is.EqualTo(1.0846136f));
            Assert.That(capture.BoneEulerAngles[5], Is.EqualTo(0.0f));
            // LeftEye
            Assert.That(capture.BoneEulerAngles[6], Is.EqualTo(-5.0845714f));
            Assert.That(capture.BoneEulerAngles[7], Is.EqualTo(0.75621915f));
            Assert.That(capture.BoneEulerAngles[8], Is.EqualTo(0.0f));
            // Neck
            Assert.That(capture.BoneEulerAngles[9], Is.EqualTo(-4.59755f));
            Assert.That(capture.BoneEulerAngles[10], Is.EqualTo(-2.6966295f));
            Assert.That(capture.BoneEulerAngles[11], Is.EqualTo(-0.3155739f));
            // Spine
            Assert.That(capture.BoneEulerAngles[12], Is.EqualTo(-1.9703788f));
            Assert.That(capture.BoneEulerAngles[13], Is.EqualTo(-1.1556984f));
            Assert.That(capture.BoneEulerAngles[14], Is.EqualTo(-0.13524596f));
        }

        [Test]
        public void ParseTest_PartialData_iFacialMocap()
        {
            var dataSource = new iFacialMocap();
            var payload = _payload_partial_iFacialMocap;

            var index_browDown_L = FacialCaptureUtility.GetBlendShapeIndex(nameof(BlendShapeName.BrowDownLeft));
            var index_mouthClose = FacialCaptureUtility.GetBlendShapeIndex(nameof(BlendShapeName.MouthClose));
            var index_tongueOut = FacialCaptureUtility.GetBlendShapeIndex(nameof(BlendShapeName.TongueOut));

            Assert.That(index_browDown_L, Is.EqualTo(0));
            Assert.That(index_mouthClose, Is.EqualTo(26));
            Assert.That(index_tongueOut, Is.EqualTo(51));

            var capture = new FacialCapture();
            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                capture.BlendShapeValues[i] = -1;
            }

            var success = dataSource.TryParse(payload, ref capture);
            Assert.That(success, Is.EqualTo(true));

            Assert.That(capture.BlendShapeValues[index_browDown_L], Is.EqualTo(100));
            Assert.That(capture.BlendShapeValues[index_mouthClose], Is.EqualTo(50));
            Assert.That(capture.BlendShapeValues[index_tongueOut], Is.EqualTo(1));

            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                if (i != index_browDown_L
                &&  i != index_mouthClose
                &&  i != index_tongueOut)
                {
                    Assert.That(capture.BlendShapeValues[i], Is.EqualTo(-1));
                }
            }

            // Head
            Assert.That(capture.BoneEulerAngles[0], Is.EqualTo(-3.14159f));
            Assert.That(capture.BoneEulerAngles[1], Is.EqualTo(-2.71828f));
            Assert.That(capture.BoneEulerAngles[2], Is.EqualTo(-1.61803f));
        }

        [Test]
        public void ParseTest_PartialData_Facemotion3d()
        {
            var dataSource = new Facemotion3d();
            var payload = _payload_partial_Facemotion3d;

            var index_browDown_L = FacialCaptureUtility.GetBlendShapeIndex(nameof(BlendShapeName.BrowDownLeft));
            var index_mouthClose = FacialCaptureUtility.GetBlendShapeIndex(nameof(BlendShapeName.MouthClose));
            var index_tongueOut = FacialCaptureUtility.GetBlendShapeIndex(nameof(BlendShapeName.TongueOut));

            Assert.That(index_browDown_L, Is.EqualTo(0));
            Assert.That(index_mouthClose, Is.EqualTo(26));
            Assert.That(index_tongueOut, Is.EqualTo(51));

            var capture = new FacialCapture();
            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                capture.BlendShapeValues[i] = -1;
            }

            var success = dataSource.TryParse(payload, ref capture);
            Assert.That(success, Is.EqualTo(true));

            Assert.That(capture.BlendShapeValues[index_browDown_L], Is.EqualTo(100));
            Assert.That(capture.BlendShapeValues[index_mouthClose], Is.EqualTo(50));
            Assert.That(capture.BlendShapeValues[index_tongueOut], Is.EqualTo(1));

            for (int i = 0; i < capture.BlendShapeValues.Length; i++)
            {
                if (i != index_browDown_L
                &&  i != index_mouthClose
                &&  i != index_tongueOut)
                {
                    Assert.That(capture.BlendShapeValues[i], Is.EqualTo(-1));
                }
            }

            // Head
            Assert.That(capture.BoneEulerAngles[0], Is.EqualTo(-3.14159f));
            Assert.That(capture.BoneEulerAngles[1], Is.EqualTo(-2.71828f));
            Assert.That(capture.BoneEulerAngles[2], Is.EqualTo(-1.61803f));
        }

        [Test]
        public void ParseErrorTest_Empty()
        {
            var capture = new FacialCapture();

            var result1 = new iFacialMocap().TryParse(_payload_empty_error, ref capture);
            Assert.That(result1, Is.EqualTo(false));

            var result2 = new Facemotion3d().TryParse(_payload_empty_error, ref capture);
            Assert.That(result2, Is.EqualTo(false));
        }

        [Test]
        public void ParseErrorTest_FirstDelimiter()
        {
            var capture = new FacialCapture();

            var result1 = new iFacialMocap().TryParse(_payload_first_delimiter_error, ref capture);
            Assert.That(result1, Is.EqualTo(false));

            var result2 = new Facemotion3d().TryParse(_payload_first_delimiter_error, ref capture);
            Assert.That(result2, Is.EqualTo(false));
        }

        [Test]
        public void ParseErrorTest_ItemDelimiter()
        {
            var capture = new FacialCapture();

            var result1 = new iFacialMocap().TryParse(_payload_item_delimiter_error, ref capture);
            Assert.That(result1, Is.EqualTo(false));

            var result2 = new Facemotion3d().TryParse(_payload_item_delimiter_error, ref capture);
            Assert.That(result2, Is.EqualTo(false));
        }

        [Test]
        public void ParseErrorTest_ValueDelimiter()
        {
            var capture = new FacialCapture();

            var result1 = new iFacialMocap().TryParse(_payload_value_delimiter_error, ref capture);
            Assert.That(result1, Is.EqualTo(false));

            var result2 = new Facemotion3d().TryParse(_payload_value_delimiter_error, ref capture);
            Assert.That(result2, Is.EqualTo(false));
        }

        private readonly string _payload_empty_error = "";
        private readonly string _payload_first_delimiter_error = "browDown_L-100|mouthClose-50|tongueOut-1|$head#-3.14159,-2.71828,-1.61803,1.61803,2.71828,3.14159|";
        private readonly string _payload_item_delimiter_error = "browDown_L-100:mouthClose-50:tongueOut-1:=head#-3.14159,-2.71828,-1.61803,1.61803,2.71828,3.14159:";
        private readonly string _payload_value_delimiter_error = "browDown_L~100|mouthClose~50|tongueOut~1|=head#-3.14159;-2.71828;-1.61803;1.61803;2.71828;3.14159|";

        private readonly string _payload_partial_iFacialMocap = "browDown_L-100|mouthClose-50|tongueOut-1|=head#-3.14159,-2.71828,-1.61803,1.61803,2.71828,3.14159|";
        private readonly string _payload_partial_Facemotion3d = "browDownLeft&100|mouthClose&50|tongueOut&1|=head#-3.14159,-2.71828,-1.61803,head|";

        private readonly string _payload_iFacialMocap = "mouthSmile_R-44|"
                                                        + "eyeLookOut_L-14|"
                                                        + "mouthUpperUp_L-47|"
                                                        + "eyeWide_R-21|"
                                                        + "mouthClose-26|"
                                                        + "mouthPucker-37|"
                                                        + "mouthRollLower-39|"
                                                        + "eyeBlink_R-9|"
                                                        + "eyeLookDown_L-10|"
                                                        + "cheekSquint_R-7|"
                                                        + "eyeBlink_L-8|"
                                                        + "tongueOut-51|"
                                                        + "jawRight-25|"
                                                        + "eyeLookIn_R-13|"
                                                        + "cheekSquint_L-6|"
                                                        + "mouthDimple_L-27|"
                                                        + "mouthPress_L-35|"
                                                        + "eyeSquint_L-18|"
                                                        + "mouthRight-38|"
                                                        + "mouthShrugLower-41|"
                                                        + "eyeLookUp_R-17|"
                                                        + "eyeLookOut_R-15|"
                                                        + "mouthPress_R-36|"
                                                        + "cheekPuff-5|"
                                                        + "jawForward-22|"
                                                        + "mouthLowerDown_L-33|"
                                                        + "mouthFrown_L-29|"
                                                        + "mouthShrugUpper-42|"
                                                        + "browOuterUp_L-3|"
                                                        + "browInnerUp-2|"
                                                        + "mouthDimple_R-28|"
                                                        + "browDown_R-1|"
                                                        + "mouthUpperUp_R-48|"
                                                        + "mouthRollUpper-40|"
                                                        + "mouthFunnel-31|"
                                                        + "mouthStretch_R-46|"
                                                        + "mouthFrown_R-30|"
                                                        + "eyeLookDown_R-11|"
                                                        + "jawOpen-24|"
                                                        + "jawLeft-23|"
                                                        + "browDown_L-0|"
                                                        + "mouthSmile_L-43|"
                                                        + "noseSneer_R-50|"
                                                        + "mouthLowerDown_R-34|"
                                                        + "noseSneer_L-49|"
                                                        + "eyeWide_L-20|"
                                                        + "mouthStretch_L-45|"
                                                        + "browOuterUp_R-4|"
                                                        + "eyeLookIn_L-12|"
                                                        + "eyeSquint_R-19|"
                                                        + "eyeLookUp_L-16|"
                                                        + "mouthLeft-32|"
                                                        + "="
                                                        + "head#-21.488958,-6.038993,-6.6019735,-0.030653415,-0.10287084,-0.6584072|"
                                                        + "rightEye#6.0297494,2.4403017,0.25649446|"
                                                        + "leftEye#6.034903,-1.6660284,-0.17520553|"
                                                        + "";

        private readonly string _payload_iFacialMocap_IndirectConnection
                                                        = "faceObjGrp!SampleHead|"
                                                        + "mouthSmileRight-44|"
                                                        + "eyeLookOutLeft-14|"
                                                        + "mouthUpperUpLeft-47|"
                                                        + "eyeWideRight-21|"
                                                        + "mouthClose-26|"
                                                        + "mouthPucker-37|"
                                                        + "mouthRollLower-39|"
                                                        + "eyeBlinkRight-9|"
                                                        + "eyeLookDownLeft-10|"
                                                        + "cheekSquintRight-7|"
                                                        + "eyeBlinkLeft-8|"
                                                        + "tongueOut-51|"
                                                        + "jawRight-25|"
                                                        + "eyeLookInRight-13|"
                                                        + "cheekSquintLeft-6|"
                                                        + "mouthDimpleLeft-27|"
                                                        + "mouthPressLeft-35|"
                                                        + "eyeSquintLeft-18|"
                                                        + "mouthRight-38|"
                                                        + "mouthShrugLower-41|"
                                                        + "eyeLookUpRight-17|"
                                                        + "eyeLookOutRight-15|"
                                                        + "mouthPressRight-36|"
                                                        + "cheekPuff-5|"
                                                        + "jawForward-22|"
                                                        + "mouthLowerDownLeft-33|"
                                                        + "mouthFrownLeft-29|"
                                                        + "mouthShrugUpper-42|"
                                                        + "browOuterUpLeft-3|"
                                                        + "browInnerUp-2|"
                                                        + "mouthDimpleRight-28|"
                                                        + "browDownRight-1|"
                                                        + "mouthUpperUpRight-48|"
                                                        + "mouthRollUpper-40|"
                                                        + "mouthFunnel-31|"
                                                        + "mouthStretchRight-46|"
                                                        + "mouthFrownRight-30|"
                                                        + "eyeLookDownRight-11|"
                                                        + "jawOpen-24|"
                                                        + "jawLeft-23|"
                                                        + "browDownLeft-0|"
                                                        + "mouthSmileLeft-43|"
                                                        + "noseSneerRight-50|"
                                                        + "mouthLowerDownRight-34|"
                                                        + "noseSneerLeft-49|"
                                                        + "eyeWideLeft-20|"
                                                        + "mouthStretchLeft-45|"
                                                        + "browOuterUpRight-4|"
                                                        + "eyeLookInLeft-12|"
                                                        + "eyeSquintRight-19|"
                                                        + "eyeLookUpLeft-16|"
                                                        + "mouthLeft-32|"
                                                        + "="
                                                        + "headPosition#0.006986115,-0.043920115,0.0021677106,objectName_is_not_specified|"
                                                        + "head#-6.567929,-3.8523278,-0.45081988,head|"
                                                        + "rightEye#-5.039491,1.0846136,0.0,eyeBall_R|"
                                                        + "leftEye#-5.0845714,0.75621915,0.0,eyeBall_L|"
                                                        + "neck#-4.59755,-2.6966295,-0.3155739,neckLower|"
                                                        + "spine#-1.9703788,-1.1556984,-0.13524596,objectName_is_not_specified|"
                                                        + "";
        private readonly string _payload_Facemotion3d = "faceObjGrp!SampleHead|"
                                                        + "blendShape1.mouthSmileRight&44|"
                                                        + "blendShape1.eyeLookOutLeft&14|"
                                                        + "blendShape1.mouthUpperUpLeft&47|"
                                                        + "blendShape1.eyeWideRight&21|"
                                                        + "blendShape1.mouthClose&26|"
                                                        + "blendShape1.mouthPucker&37|"
                                                        + "blendShape1.mouthRollLower&39|"
                                                        + "blendShape1.eyeBlinkRight&9|"
                                                        + "blendShape1.eyeLookDownLeft&10|"
                                                        + "blendShape1.cheekSquintRight&7|"
                                                        + "blendShape1.eyeBlinkLeft&8|"
                                                        + "blendShape1.tongueOut&51|"
                                                        + "blendShape1.jawRight&25|"
                                                        + "blendShape1.eyeLookInRight&13|"
                                                        + "blendShape1.cheekSquintLeft&6|"
                                                        + "blendShape1.mouthDimpleLeft&27|"
                                                        + "blendShape1.mouthPressLeft&35|"
                                                        + "blendShape1.eyeSquintLeft&18|"
                                                        + "blendShape1.mouthRight&38|"
                                                        + "blendShape1.mouthShrugLower&41|"
                                                        + "blendShape1.eyeLookUpRight&17|"
                                                        + "blendShape1.eyeLookOutRight&15|"
                                                        + "blendShape1.mouthPressRight&36|"
                                                        + "blendShape1.cheekPuff&5|"
                                                        + "blendShape1.jawForward&22|"
                                                        + "blendShape1.mouthLowerDownLeft&33|"
                                                        + "blendShape1.mouthFrownLeft&29|"
                                                        + "blendShape1.mouthShrugUpper&42|"
                                                        + "blendShape1.browOuterUpLeft&3|"
                                                        + "blendShape1.browInnerUp&2|"
                                                        + "blendShape1.mouthDimpleRight&28|"
                                                        + "blendShape1.browDownRight&1|"
                                                        + "blendShape1.mouthUpperUpRight&48|"
                                                        + "blendShape1.mouthRollUpper&40|"
                                                        + "blendShape1.mouthFunnel&31|"
                                                        + "blendShape1.mouthStretchRight&46|"
                                                        + "blendShape1.mouthFrownRight&30|"
                                                        + "blendShape1.eyeLookDownRight&11|"
                                                        + "blendShape1.jawOpen&24|"
                                                        + "blendShape1.jawLeft&23|"
                                                        + "blendShape1.browDownLeft&0|"
                                                        + "blendShape1.mouthSmileLeft&43|"
                                                        + "blendShape1.noseSneerRight&50|"
                                                        + "blendShape1.mouthLowerDownRight&34|"
                                                        + "blendShape1.noseSneerLeft&49|"
                                                        + "blendShape1.eyeWideLeft&20|"
                                                        + "blendShape1.mouthStretchLeft&45|"
                                                        + "blendShape1.browOuterUpRight&4|"
                                                        + "blendShape1.eyeLookInLeft&12|"
                                                        + "blendShape1.eyeSquintRight&19|"
                                                        + "blendShape1.eyeLookUpLeft&16|"
                                                        + "blendShape1.mouthLeft&32|"
                                                        + "="
                                                        + "headPosition#0.006986115,-0.043920115,0.0021677106,objectName_is_not_specified|"
                                                        + "head#-6.567929,-3.8523278,-0.45081988,head|"
                                                        + "rightEye#-5.039491,1.0846136,0.0,eyeBall_R|"
                                                        + "leftEye#-5.0845714,0.75621915,0.0,eyeBall_L|"
                                                        + "neck#-4.59755,-2.6966295,-0.3155739,neckLower|"
                                                        + "spine#-1.9703788,-1.1556984,-0.13524596,objectName_is_not_specified|"
                                                        + "";
    }
}