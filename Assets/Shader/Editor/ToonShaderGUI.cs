using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class ToonShaderGUI : ShaderGUI 
{
    private enum ShadingStyle
    {
        SkinAndTextiles,
        HairAndMetal,
    }
    private ShadingStyle _shadingStyle;

    private bool _toonPropsVisible = true;
    private bool _emission;

    private const string SkinKeyword = "_SHADING_STYLE_SKIN_AND_TEXTILES";
    private const string HairKeyword = "_SHADING_STYLE_HAIR_AND_METAL";

    public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        Material targetMaterial = materialEditor.target as Material;

        //======= Base Properties
        
        GUILayout.Label("Base properties", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        
        materialEditor.TextureProperty(FindProperty("_BaseMap", properties), "Base map");
        materialEditor.ColorProperty(FindProperty("_BaseColor", properties), "Tint");
        materialEditor.TextureProperty(FindProperty("_NormalMap", properties), "Normal map");
        
        //Emission property
        bool emissionPreviousValue = _emission;
        _emission = DrawEmissionToggle(materialEditor, FindProperty("_UseEmission", properties));
        if (_emission)
        {
            EditorGUI.indentLevel++;
            materialEditor.TextureProperty(FindProperty("_EmissionMap", properties), "Emission map");
            materialEditor.ColorProperty(FindProperty("_EmissionColor", properties), "Emission tint");
            EditorGUI.indentLevel--;
            if (emissionPreviousValue != _emission)
            {
                targetMaterial.EnableKeyword("_EMISSION");
                targetMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
            }
        }
        else
        {
            if (emissionPreviousValue != _emission)
            {
                targetMaterial.DisableKeyword("_EMISSION");
                targetMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            }
        }

        materialEditor.RangeProperty(FindProperty("_Smoothness", properties), "Smoothness");
        materialEditor.ColorProperty(FindProperty("_SpecularColor", properties), "Specular color");
        materialEditor.RangeProperty(FindProperty("_Hue", properties), "Hue");
        materialEditor.RangeProperty(FindProperty("_Saturation", properties), "Saturation");
        EditorGUI.indentLevel--;
    }

    private bool DrawEmissionToggle(MaterialEditor materialEditor, MaterialProperty prop)
    {
        bool value = (prop.floatValue != 0.0f);

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = prop.hasMixedValue;

        // Show the toggle control
        value = EditorGUILayout.Toggle("Emission", value);

        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            prop.floatValue = value ? 1.0f : 0.0f;
        }

        return value && !prop.hasMixedValue;
    }
}