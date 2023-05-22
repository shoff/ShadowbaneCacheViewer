namespace Arcane.Unity;

using System.Collections;
using UnityEngine;

// This code assumes that each group of three values in the motion_smoothing
// arrays represent rotation in x, y, and z respectively over time.
// You may need to adjust this code based on what exactly the motion_smoothing
// data represents. Also, please note that the motion_smoothed_count and
// motion_smoothed_value properties are not used here, as it's unclear how
// they should be applied without more specific details about the data.
public class ApplyMotionData : MonoBehaviour
{
    public Arcane.Cache.Json.Motion.Motion data;

    private void Start()
    {
        ApplyMotionSmoothing();
    }

    void ApplyMotionSmoothing()
    {
        float keyframeDuration = 1f / this.data.MotionTargetFrames[0]; // Calculate the duration of each keyframe

        for (int i = 0; i < this.data.MotionParts.Count; i++)
        {
            var boneName = this.data.MotionParts[i];
            var boneTransform = this.transform.Find(boneName); // Assumes that the bone names match the names of the GameObjects in the model

            if (boneTransform == null)
            {
                continue;
            }

            var smoothingData = this.data.MotionSmoothing[i];

            AnimationCurve curvePosX = new AnimationCurve();
            AnimationCurve curvePosY = new AnimationCurve();
            AnimationCurve curvePosZ = new AnimationCurve();

            AnimationCurve curveRotX = new AnimationCurve();
            AnimationCurve curveRotY = new AnimationCurve();
            AnimationCurve curveRotZ = new AnimationCurve();
            AnimationCurve curveRotW = new AnimationCurve();

            AnimationCurve curveScaleX = new AnimationCurve();
            AnimationCurve curveScaleY = new AnimationCurve();
            AnimationCurve curveScaleZ = new AnimationCurve();

            for (int j = 0; j < smoothingData.Count; j += 10)
            {
                float time = j * keyframeDuration;

                // decode position
                Keyframe keyPosX = new Keyframe(time, smoothingData[j]);
                Keyframe keyPosY = new Keyframe(time, smoothingData[j + 1]);
                Keyframe keyPosZ = new Keyframe(time, smoothingData[j + 2]);

                // decode rotation
                Keyframe keyRotX = new Keyframe(time, smoothingData[j + 3]);
                Keyframe keyRotY = new Keyframe(time, smoothingData[j + 4]);
                Keyframe keyRotZ = new Keyframe(time, smoothingData[j + 5]);
                Keyframe keyRotW = new Keyframe(time, smoothingData[j + 6]);  // Added this line

                // decode scale
                Keyframe keyScaleX = new Keyframe(time, smoothingData[j + 7]);
                Keyframe keyScaleY = new Keyframe(time, smoothingData[j + 8]);
                Keyframe keyScaleZ = new Keyframe(time, smoothingData[j + 9]);

                // add keys to curves
                curvePosX.AddKey(keyPosX);
                curvePosY.AddKey(keyPosY);
                curvePosZ.AddKey(keyPosZ);

                curveRotX.AddKey(keyRotX);
                curveRotY.AddKey(keyRotY);
                curveRotZ.AddKey(keyRotZ);
                curveRotW.AddKey(keyRotW);

                curveScaleX.AddKey(keyScaleX);
                curveScaleY.AddKey(keyScaleY);
                curveScaleZ.AddKey(keyScaleZ);
            }

            // Apply the curves to the bone's transform
            StartCoroutine(ApplyAnimation(boneTransform, curvePosX, curvePosY, curvePosZ, curveRotX, curveRotY, curveRotZ, curveRotW, curveScaleX, curveScaleY, curveScaleZ));
        }
    }

    IEnumerator ApplyAnimation(Transform transform, AnimationCurve curvePosX, AnimationCurve curvePosY, AnimationCurve curvePosZ, AnimationCurve curveRotX, AnimationCurve curveRotY, AnimationCurve curveRotZ, AnimationCurve curveRotW, AnimationCurve curveScaleX, AnimationCurve curveScaleY, AnimationCurve curveScaleZ)
    {
        for (float t = 0; t < curvePosX.keys[curvePosX.length - 1].time; t += Time.deltaTime)
        {
            // Interpolate position, rotation and scale
            Vector3 pos = new Vector3(curvePosX.Evaluate(t), curvePosY.Evaluate(t), curvePosZ.Evaluate(t));
            Quaternion rot = new Quaternion(curveRotX.Evaluate(t), curveRotY.Evaluate(t), curveRotZ.Evaluate(t), curveRotW.Evaluate(t));
            Vector3 scale = new Vector3(curveScaleX.Evaluate(t), curveScaleY.Evaluate(t), curveScaleZ.Evaluate(t));

            transform.localPosition = pos;
            transform.localRotation = rot;
            transform.localScale = scale;

            yield return null;
        }
    }
}


public class ApplyMotionData1 : MonoBehaviour
{
    public Arcane.Cache.Json.Motion.Motion motionData;

    void Start()
    {
        ApplyMotionSmoothing();
    }

    void ApplyMotionSmoothing()
    {
        foreach (var smoothingData in this.motionData.MotionSmoothing)
        {
            AnimationCurve curveX = new AnimationCurve();
            AnimationCurve curveY = new AnimationCurve();
            AnimationCurve curveZ = new AnimationCurve();

            for (int i = 0; i < smoothingData.Count; i += 3)
            {
                Keyframe keyX = new Keyframe(i, smoothingData[i]);
                Keyframe keyY = new Keyframe(i, smoothingData[i + 1]);
                Keyframe keyZ = new Keyframe(i, smoothingData[i + 2]);

                curveX.AddKey(keyX);
                curveY.AddKey(keyY);
                curveZ.AddKey(keyZ);
            }

            // Apply the curves to the GameObject's rotation
            StartCoroutine(ApplyAnimation(curveX, curveY, curveZ));
        }
    }

    IEnumerator ApplyAnimation(AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveZ)
    {
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            float x = curveX.Evaluate(t);
            float y = curveY.Evaluate(t);
            float z = curveZ.Evaluate(t);

            this.transform.rotation = Quaternion.Euler(x, y, z);

            yield return null;
        }
    }
}

public class MotionData
{
    public float[][] motion_smoothing;
    // rest of your fields
}

public class ApplyMotionData2 : MonoBehaviour
{
    public MotionData data;

    void Start()
    {
        ApplyMotionSmoothing();
    }

    void ApplyMotionSmoothing()
    {
        foreach (var smoothingData in this.data.motion_smoothing)
        {
            AnimationCurve curvePosX = new AnimationCurve();
            AnimationCurve curvePosY = new AnimationCurve();
            AnimationCurve curvePosZ = new AnimationCurve();

            AnimationCurve curveRotX = new AnimationCurve();
            AnimationCurve curveRotY = new AnimationCurve();
            AnimationCurve curveRotZ = new AnimationCurve();

            AnimationCurve curveScaleX = new AnimationCurve();
            AnimationCurve curveScaleY = new AnimationCurve();
            AnimationCurve curveScaleZ = new AnimationCurve();

            for (int i = 0; i < smoothingData.Length; i += 10)
            {
                // decode position
                Keyframe keyPosX = new Keyframe(i, smoothingData[i]);
                Keyframe keyPosY = new Keyframe(i, smoothingData[i + 1]);
                Keyframe keyPosZ = new Keyframe(i, smoothingData[i + 2]);

                // decode rotation
                Keyframe keyRotX = new Keyframe(i, smoothingData[i + 3]);
                Keyframe keyRotY = new Keyframe(i, smoothingData[i + 4]);
                Keyframe keyRotZ = new Keyframe(i, smoothingData[i + 5]);

                // decode scale
                Keyframe keyScaleX = new Keyframe(i, smoothingData[i + 6]);
                Keyframe keyScaleY = new Keyframe(i, smoothingData[i + 7]);
                Keyframe keyScaleZ = new Keyframe(i, smoothingData[i + 8]);

                // add keys to curves
                curvePosX.AddKey(keyPosX);
                curvePosY.AddKey(keyPosY);
                curvePosZ.AddKey(keyPosZ);

                curveRotX.AddKey(keyRotX);
                curveRotY.AddKey(keyRotY);
                curveRotZ.AddKey(keyRotZ);

                curveScaleX.AddKey(keyScaleX);
                curveScaleY.AddKey(keyScaleY);
                curveScaleZ.AddKey(keyScaleZ);
            }

            // Apply the curves to the GameObject's transform
            StartCoroutine(ApplyAnimation(curvePosX, curvePosY, curvePosZ, curveRotX, curveRotY, curveRotZ, curveScaleX, curveScaleY, curveScaleZ));
        }
    }

    IEnumerator ApplyAnimation(AnimationCurve curvePosX, AnimationCurve curvePosY, AnimationCurve curvePosZ,
        AnimationCurve curveRotX, AnimationCurve curveRotY, AnimationCurve curveRotZ,
        AnimationCurve curveScaleX, AnimationCurve curveScaleY, AnimationCurve curveScaleZ)
    {
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            // Set the position
            Vector3 position = new Vector3(
                curvePosX.Evaluate(t),
                curvePosY.Evaluate(t),
                curvePosZ.Evaluate(t));

            // Set the rotation
            Quaternion rotation = Quaternion.Euler(
                curveRotX.Evaluate(t),
                curveRotY.Evaluate(t),
                curveRotZ.Evaluate(t));

            // Set the scale
            Vector3 scale = new Vector3(
                curveScaleX.Evaluate(t),
                curveScaleY.Evaluate(t),
                curveScaleZ.Evaluate(t));

            this.transform.localPosition = position;
            this.transform.localRotation = rotation;
            this.transform.localScale = scale;

            yield return null;
        }
    }
}
