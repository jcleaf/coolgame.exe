using System;
using UnityEngine;

/// <summary>
/// Class used to allow use of either a reference value or a custom value.
/// </summary>
[Serializable]
public abstract class ValueReference<T, TValue> where TValue : Value<T>
{
#pragma warning disable 0649
    [SerializeField] private bool tryUseReferenceValue = true;
    [SerializeField] private TValue referenceValue;
    [SerializeField] private T customValue;
#pragma warning restore 0649

    private bool useReferenceValue { get { return tryUseReferenceValue && referenceValue.value != null; } }

    public T value
    {
        get
        {
            return useReferenceValue ? referenceValue.value : customValue;
        }

        set
        {
            if (useReferenceValue)
            {
                referenceValue.value = value;
            }
            else
            {
                customValue = value;
            }
        }
    }

    public static implicit operator T(ValueReference<T, TValue> r) { return r.value; }
}

//For Unity serialization
[Serializable] public class FloatReference : ValueReference<float, FloatValue> { }
[Serializable] public class IntReference : ValueReference<int, IntValue> { }
[Serializable] public class AnimationCurveReference : ValueReference<AnimationCurve, AnimationCurveValue> { }




/// <summary>
/// This class allows the reference to have a reference to any value in CameraTuning.
/// </summary>
[Serializable]
public abstract class Value<T> : ScriptableObject
{
    public T value;
}