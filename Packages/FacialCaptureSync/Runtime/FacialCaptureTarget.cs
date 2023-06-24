#if UNITY_2020_3_OR_NEWER

using System;
using System.Linq;
using UnityEngine;

namespace FacialCaptureSync
{
    public class FacialCaptureTarget : MonoBehaviour
    {
        [SerializeField] bool _initializeOnAwake;

        [SerializeField] Transform _headTransform;
        [SerializeField] Transform _leftEyeTransform;
        [SerializeField] Transform _rightEyeTransform;
        [SerializeField] Transform _neckTransform;
        [SerializeField] Transform _spineTransform;

        [SerializeField] string _blendShapePrefix = "";
        [SerializeField] char _blendShapePrefixDelimiter = '.';

        public float EyeMoveScaleFactor = 0.5f;

        public bool Initialized => _initialized;
        private bool _initialized = false;

        private SkinnedMeshRenderer[] _faceMeshRenderers;
        private (int MeshIndex, int BlendShapeIndex)[] _blendShapeIndexMap = new (int MeshIndex, int BlendShapeIndex)[FacialCapture.BlendShapeCount];

        void Awake()
        {
            if (_initializeOnAwake) { Initialize();}
        }

        public void Initialize()
        {
            if (_initialized) { return; }

            _faceMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

#if UNITY_EDITOR
            Debug.Log($"FaceMeshRenderer.Length: {_faceMeshRenderers.Length}");
#endif

            for (int i = 0; i < _blendShapeIndexMap.Length; i++)
            {
                _blendShapeIndexMap[i] = (-1, -1);
            }

            var blendShapeNames = ((BlendShapeName[])Enum.GetValues(typeof(BlendShapeName))).Select(value => value.ToString());
            var blendShapeNamesInCamelCase = ((BlendShapeNameInCamelCase[])Enum.GetValues(typeof(BlendShapeNameInCamelCase))).Select(value => value.ToString());
            blendShapeNames = blendShapeNames.Concat(blendShapeNamesInCamelCase);

            for (int meshIndex = 0; meshIndex < _faceMeshRenderers.Length; meshIndex++)
            {
                var mesh = _faceMeshRenderers[meshIndex].sharedMesh;
                if (mesh != null && mesh.blendShapeCount > 0)
                {
                    foreach (var blendShapeName in blendShapeNames)
                    {
                        var blendShapeNameInMesh = blendShapeName;
                        if (!string.IsNullOrEmpty(_blendShapePrefix))
                        {
                            blendShapeNameInMesh = $"{_blendShapePrefix}{_blendShapePrefixDelimiter}{blendShapeName}";
                        }

                        var index = FacialCaptureUtility.GetBlendShapeIndex(blendShapeName);
                        var blendShapeIndexInMesh = mesh.GetBlendShapeIndex(blendShapeNameInMesh);

                        if (index >= 0 && blendShapeIndexInMesh >= 0)
                        {
                            _blendShapeIndexMap[index] = (meshIndex, blendShapeIndexInMesh);
                        }
                    }
                }
            }

            var animator = GetComponentInChildren<Animator>();
            if (animator != null)
            {
                if (_headTransform is null)     { _headTransform = animator.GetBoneTransform(HumanBodyBones.Head); }
                if (_leftEyeTransform is null)  { _leftEyeTransform = animator.GetBoneTransform(HumanBodyBones.LeftEye); }
                if (_rightEyeTransform is null) { _rightEyeTransform = animator.GetBoneTransform(HumanBodyBones.RightEye); }
                if (_neckTransform is null)     { _neckTransform = animator.GetBoneTransform(HumanBodyBones.Neck); }
                if (_spineTransform is null)    { _spineTransform = animator.GetBoneTransform(HumanBodyBones.Spine); }
            }

            _initialized = true;
        }

        public void SetBlendShapes(FacialCapture capture)
        {
            for (int i = 0; i < FacialCapture.BlendShapeCount; i++)
            {
                var meshIndex = _blendShapeIndexMap[i].MeshIndex;
                var blendShapeIndex = _blendShapeIndexMap[i].BlendShapeIndex;

                if (meshIndex >= 0 && blendShapeIndex >= 0)
                {
                    _faceMeshRenderers[meshIndex].SetBlendShapeWeight(blendShapeIndex, (float)capture.BlendShapeValues[i]);
                }
            }
        }

        public void SetBonePoses(FacialCapture capture)
        {
            if (_headTransform != null)
            {
                var boneIndex = (int)BoneName.head;
                var x = capture.BoneEulerAngles[3 * boneIndex + 0];
                var y = capture.BoneEulerAngles[3 * boneIndex + 1];
                var z = capture.BoneEulerAngles[3 * boneIndex + 2];
                _headTransform.localEulerAngles = new Vector3(x, y, z);
            }
            if (_rightEyeTransform != null)
            {
                var boneIndex = (int)BoneName.rightEye;
                var x = capture.BoneEulerAngles[3 * boneIndex + 0];
                var y = capture.BoneEulerAngles[3 * boneIndex + 1];
                var z = capture.BoneEulerAngles[3 * boneIndex + 2];
                _rightEyeTransform.localEulerAngles = new Vector3(x, y, z) * EyeMoveScaleFactor;
            }
            if (_leftEyeTransform != null)
            {
                var boneIndex = (int)BoneName.leftEye;
                var x = capture.BoneEulerAngles[3 * boneIndex + 0];
                var y = capture.BoneEulerAngles[3 * boneIndex + 1];
                var z = capture.BoneEulerAngles[3 * boneIndex + 2];
                _leftEyeTransform.localEulerAngles = new Vector3(x, y, z) * EyeMoveScaleFactor;
            }
            if (_neckTransform != null)
            {
                var boneIndex = (int)BoneName.neck;
                var x = capture.BoneEulerAngles[3 * boneIndex + 0];
                var y = capture.BoneEulerAngles[3 * boneIndex + 1];
                var z = capture.BoneEulerAngles[3 * boneIndex + 2];
                _neckTransform.localEulerAngles = new Vector3(x, y, z);
            }
            if (_spineTransform != null)
            {
                var boneIndex = (int)BoneName.spine;
                var x = capture.BoneEulerAngles[3 * boneIndex + 0];
                var y = capture.BoneEulerAngles[3 * boneIndex + 1];
                var z = capture.BoneEulerAngles[3 * boneIndex + 2];
                _spineTransform.localEulerAngles = new Vector3(x, y, z);
            }
        }
    }
}

#endif