using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class TextureImport : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            var textureImporter = (TextureImporter) assetImporter;
            //UI目录下导入的图，自动变成Sprite
            if (textureImporter.assetPath.StartsWith("Assets/Ui/Sprites"))
            {
                textureImporter.isReadable = false;
                textureImporter.alphaIsTransparency = true;
                textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
                textureImporter.mipmapEnabled = false;
                textureImporter.textureType = TextureImporterType.Sprite;

                var textureImporterSetting = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(textureImporterSetting);
                textureImporterSetting.spriteMeshType = SpriteMeshType.FullRect;
                textureImporterSetting.spriteGenerateFallbackPhysicsShape = false;
                textureImporter.SetTextureSettings(textureImporterSetting);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                textureImporter.SaveAndReimport();
            }
        }
    }
}
