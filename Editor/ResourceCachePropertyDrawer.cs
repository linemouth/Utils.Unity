using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utils;
using Utils.Unity;
using Color = UnityEngine.Color;

[CustomPropertyDrawer(typeof(ResourceCache))]
public class ResourceCacheDrawer : PropertyDrawer
{
	/*public ResourceCacheDrawer()
    {
		EditorApplication.update += Update;
	}
	~ResourceCacheDrawer()
	{
		EditorApplication.update -= Update;
		EditorApplication.update -= Update;
	}*/
	private static readonly Dictionary<string, (Color color, Texture2D icon)> resourceTypes = new Dictionary<string, (Color color, Texture2D icon)>
	{
		{ "energy", (new Color(0, 0.5f, 1), Resources.Load<Texture2D>("Graphics/Materials/Icons/energy")) }
    };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		ResourceCache resourceCache = (ResourceCache)fieldInfo.GetValue(property.serializedObject.targetObject);
		(Color color, Texture2D icon) info = default;
		if(!resourceTypes.TryGetValue(label.text.ToLowerInvariant(), out info))
        {
			info = (new Color(0.5f, 0.5f, 0.5f), null);
		}

		// Calculate geometry
		Rect contentRect = EditorGUI.PrefixLabel(position, label);
		Rect[] rowRects;
		Rect percentRect;
		if(EditorGUIUtility.wideMode)
        {
			rowRects = EditorGUIExtentions.GetRowRects(contentRect, "+1 10 +1 30+1", 4);
			percentRect = rowRects[3];
		}
		else
		{
			percentRect = contentRect;
			percentRect.height = 16;
			contentRect = new Rect(position.x + 16, position.y + 20, position.width - 16, 16);
			rowRects = EditorGUIExtentions.GetRowRects(contentRect, "+1 10 +1", 3);
		}

		// Show read-only fields
		//EditorGUI.BeginProperty(position, label, property);
		if(info.icon != null)
        {
			Rect iconPosition = new Rect(contentRect.x - 20, contentRect.y, 16, 16);
			EditorGUI.LabelField(iconPosition, new GUIContent(info.icon));
        }
		resourceCache.Stored = Math.Clamp(EditorGUI.DoubleField(rowRects[0], resourceCache.Stored), 0, resourceCache.ConnectedCapacity);
		EditorGUI.LabelField(rowRects[1], "/");
		resourceCache.Capacity = Math.Max(EditorGUI.DoubleField(rowRects[2], resourceCache.Capacity), 0);
		double fraction = resourceCache.Fraction;
		Rect barRect = percentRect;
		barRect.height = 2;
		EditorGUI.DrawRect(barRect, new Color(0, 0, 0));
		barRect.width *= (float)fraction;
		EditorGUI.DrawRect(barRect, info.color);
		EditorGUI.LabelField(percentRect, new GUIContent($"({100 * fraction:0.0}% of {resourceCache.ConnectedCapacity})", "Includes connected capacities."));
		//EditorGUI.EndProperty();
	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.wideMode ? 16f : 36f;
	}

	/*private void Update()
	{
		EditorWindow.focusedWindow.Repaint();
	}*/
}
