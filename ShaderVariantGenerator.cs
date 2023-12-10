using UnityEditor;
using UnityEngine;
using System.IO;

public class ShaderVariantGenerator : EditorWindow
{
    private Material targetMaterial;
    private Vector2 minMaxRange;
    private int variantCount;
    private bool isHDR;
    private bool isTexture;
    private string textureFolderPath =null;
    private string[] textureFiles;

    [MenuItem("Tools/Shader Variant Generator")]
    public static void ShowWindow()
    {
        GetWindow<ShaderVariantGenerator>("Shader Variant Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Drag and drop a material to generate variants.", EditorStyles.boldLabel);

        targetMaterial = EditorGUILayout.ObjectField("Target Material", targetMaterial, typeof(Material), true) as Material;
        minMaxRange = EditorGUILayout.Vector2Field("Min-Max Range", minMaxRange);
        variantCount = EditorGUILayout.IntField("Variant Count", variantCount);
        isHDR = EditorGUILayout.Toggle("HDR Variants", isHDR);
        isTexture = EditorGUILayout.Toggle("Texture Variants", isTexture);
        
        if (isTexture)
        {
            Repaint();
            textureFolderPath = EditorGUILayout.TextField("Texture Folder Path", textureFolderPath);
        }

        if (GUILayout.Button("Generate Variants"))
        {
            GenerateMaterialVariants();
        }
    }

    private void GenerateMaterialVariants()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target Material is not set.");
            return;
        }

        if (!(Directory.Exists("Assets/Variants")))
        {
            Directory.CreateDirectory("Assets/Variants");
        }

        if (textureFolderPath != null && isTexture)
        {
            textureFiles = Directory.GetFiles(textureFolderPath, "*.png");
        }
        int propertyCount = ShaderUtil.GetPropertyCount(targetMaterial.shader);

        for (int i = 0; i < variantCount; i++)
        {
            Material variantMaterial = new Material(targetMaterial);

            for (int j = 0; j < propertyCount; j++)
            {
                string propertyName = ShaderUtil.GetPropertyName(targetMaterial.shader, j);

                // Rastgele deðer atama örneði, siz kendi mantýðýnýza göre ayarlayabilirsiniz.
                if (ShaderUtil.GetPropertyType(targetMaterial.shader, j) == ShaderUtil.ShaderPropertyType.Float)
                {
                    variantMaterial.SetFloat(propertyName, Random.Range(minMaxRange.x, minMaxRange.y));
                }
                else if (ShaderUtil.GetPropertyType(targetMaterial.shader, j) == ShaderUtil.ShaderPropertyType.Vector)
                {
                    variantMaterial.SetVector(propertyName, Random.onUnitSphere * Random.Range(minMaxRange.x, minMaxRange.y));
                }
                else if (ShaderUtil.GetPropertyType(targetMaterial.shader, j) == ShaderUtil.ShaderPropertyType.Color)
                {
                    variantMaterial.SetColor(propertyName, new Color(Random.value, Random.value, Random.value, 1f));
                    if (isHDR)
                    {
                        Color hdrColor = variantMaterial.GetColor(propertyName);
                        hdrColor *= Random.value * 10;
                        variantMaterial.SetColor(propertyName, hdrColor);
                    }
                }
                else if (ShaderUtil.GetPropertyType(targetMaterial.shader, j) == ShaderUtil.ShaderPropertyType.TexEnv && isTexture)
                {
                    // Texture propertisine rastgele bir texture atama
                    if (textureFiles.Length > 0)
                    {
                        string randomTexturePath = textureFiles[Random.Range(0, textureFiles.Length)];
                        Texture2D randomTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(randomTexturePath);
                        variantMaterial.SetTexture(propertyName, randomTexture);
                    }
                    else
                    {
                        Debug.LogWarning("No textures found in the selected folder.");
                    }
                }
            }

            string variantMaterialPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Variants/" + targetMaterial.name + "_Variant_" + i + ".mat");
            AssetDatabase.CreateAsset(variantMaterial, variantMaterialPath);

            Debug.Log("Material variant " + i + " created: " + variantMaterialPath);
        }

        Debug.Log("Material variants generated successfully.");
    }
}
