using UnityEngine;
using UnityEditor;

public abstract class ValueReferenceInspector<T> : PropertyDrawer
{
    private readonly string[] popupOptions = { "Use Reference", "Use Custom" };
    private GUIStyle popupStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Initialize();

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        SerializedProperty tryUseDefaultValue = property.FindPropertyRelative("tryUseReferenceValue");
        SerializedProperty defaultValue = property.FindPropertyRelative("referenceValue");
        SerializedProperty customValue = property.FindPropertyRelative("customValue");

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //Popup
        int result = EditorGUI.Popup(GetSwitchButtonRect(ref position), tryUseDefaultValue.boolValue ? 0 : 1, popupOptions, popupStyle);
        tryUseDefaultValue.boolValue = result == 0;

        //Property
        EditorGUI.PropertyField(position, tryUseDefaultValue.boolValue ? defaultValue : customValue, GUIContent.none);

        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
        }
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    //Get an appropriate Rect for the default/custom switch button
    private Rect GetSwitchButtonRect(ref Rect position)
    {
        Rect buttonRect = new Rect(position);
        buttonRect.yMin += popupStyle.margin.top;
        buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
        position.xMin = buttonRect.xMax;

        return buttonRect;
    }

    private void Initialize()
    {
        if (popupStyle == null)
        {
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }
    }
}

//For Unity serialization
[CustomPropertyDrawer(typeof(FloatReference))]
public class FloatReferenceInspector : ValueReferenceInspector<float> { }

[CustomPropertyDrawer(typeof(IntReference))]
public class IntReferenceInspector : ValueReferenceInspector<int> { }

[CustomPropertyDrawer(typeof(BoolReference))]
public class BoolReferenceInspector : ValueReferenceInspector<bool> { }

[CustomPropertyDrawer(typeof(AnimationCurveReference))]
public class AnimationCurveReferenceInspector : ValueReferenceInspector<AnimationCurve> { }



public abstract class ValueInspector<T> : PropertyDrawer
{
    private const float spaceWidth = 5f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (property.objectReferenceValue == null) //this property hasn't been assigned yet
        {
            //asset select
            EditorGUI.PropertyField(position, property, label);
        }
        else
        {
            SerializedObject asset = new SerializedObject(property.objectReferenceValue);
            SerializedProperty value = asset.FindProperty("value");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            float propertyWidth = (position.width - spaceWidth) * 0.5f;

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            //allow selecting of a new asset
            EditorGUI.PropertyField(GetPropertyRect(ref position, propertyWidth), property, GUIContent.none);

            //show disabled value of selected asset
            bool guiWasEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(GetPropertyRect(ref position, propertyWidth), value, GUIContent.none);
            GUI.enabled = guiWasEnabled;
            EditorGUI.indentLevel = indent;
        }

        EditorGUI.EndProperty();
    }

    //Get an appropriate Rect for the property field rects
    private Rect GetPropertyRect(ref Rect position, float propertyWidth)
    {
        Rect propertyRect = new Rect(position);
        propertyRect.width = propertyWidth;
        position.xMin = propertyRect.xMax + spaceWidth;

        return propertyRect;
    }
}

//For Unity serialization
[CustomPropertyDrawer(typeof(FloatValue))]
public class FloatValueInspector : ValueInspector<float> { }

[CustomPropertyDrawer(typeof(FloatValue))]
public class IntValueInspector : ValueInspector<int> { }

[CustomPropertyDrawer(typeof(BoolValue))]
public class BoolValueInspector : ValueInspector<bool> { }

[CustomPropertyDrawer(typeof(AnimationCurveValue))]
public class AnimationCurveValueInspector : ValueInspector<AnimationCurve> { }