using UnityEngine;
using UnityEditor;
using System.IO;

public class PageComposer : EditorWindow
{
    public Texture2D backgroundFrame; // Ảnh khung nền (customBackground)
    public Texture2D[] turretImages;  // Danh sách ảnh Turret
    public float turretScale = 0.8f;  // Tỉ lệ ảnh Turret so với khung (0.8 = 80%)

    [MenuItem("Tools/Cyber Defense/Create Book Pages")]
    public static void ShowWindow()
    {
        GetWindow<PageComposer>("Page Composer");
    }

    void OnGUI()
    {
        GUILayout.Label("Cấu hình tạo trang sách", EditorStyles.boldLabel);

        backgroundFrame = (Texture2D)EditorGUILayout.ObjectField("Ảnh nền (Khung)", backgroundFrame, typeof(Texture2D), false);
        
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("turretImages");

        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();

        turretScale = EditorGUILayout.Slider("Tỉ lệ thu phóng Turret", turretScale, 0.1f, 1.5f);

        if (GUILayout.Button("Tạo Trang Sách"))
        {
            if (backgroundFrame == null || turretImages == null || turretImages.Length == 0)
            {
                Debug.LogError("Vui lòng chọn ảnh nền và ít nhất một ảnh Turret!");
                return;
            }
            CreatePages();
        }
    }

    void CreatePages()
    {
        string path = "Assets/Textures/GeneratedPages";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        // Đảm bảo ảnh nền đọc được
        string bgPath = AssetDatabase.GetAssetPath(backgroundFrame);
        SetTextureReadable(bgPath);

        int width = backgroundFrame.width;
        int height = backgroundFrame.height;

        for (int i = 0; i < turretImages.Length; i++)
        {
            if (turretImages[i] == null) continue;

            // Đảm bảo ảnh turret đọc được
            string tPath = AssetDatabase.GetAssetPath(turretImages[i]);
            SetTextureReadable(tPath);

            // Tạo texture mới
            Texture2D newPage = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] bgPixels = backgroundFrame.GetPixels();
            newPage.SetPixels(bgPixels);

            // Tính toán vị trí vẽ đè turret
            Texture2D turret = turretImages[i];
            int tW = (int)(turret.width * turretScale);
            int tH = (int)(turret.height * turretScale);
            
            // Vẽ turret vào giữa
            int startX = (width - tW) / 2;
            int startY = (height - tH) / 2;

            for (int y = 0; y < tH; y++)
            {
                for (int x = 0; x < tW; x++)
                {
                    // Lấy màu từ ảnh gốc (dùng bilinear filtering để scale mượt hơn)
                    float u = x / (float)tW;
                    float v = y / (float)tH;
                    Color tColor = turret.GetPixelBilinear(u, v);

                    if (tColor.a > 0.1f) // Nếu pixel không trong suốt
                    {
                        // Pha màu (Alpha Blending)
                        int targetX = startX + x;
                        int targetY = startY + y;
                        if (targetX >= 0 && targetX < width && targetY >= 0 && targetY < height)
                        {
                            Color bgColor = newPage.GetPixel(targetX, targetY);
                            Color finalColor = Color.Lerp(bgColor, tColor, tColor.a);
                            newPage.SetPixel(targetX, targetY, finalColor);
                        }
                    }
                }
            }

            newPage.Apply();

            // Lưu thành file
            byte[] bytes = newPage.EncodeToPNG();
            string fileName = $"{path}/Page_{turretImages[i].name}.png";
            File.WriteAllBytes(fileName, bytes);
        }

        AssetDatabase.Refresh();
        Debug.Log("Đã tạo xong các trang sách tại: " + path);
    }

    void SetTextureReadable(string path)
    {
        TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(path);
        if (ti != null && !ti.isReadable)
        {
            ti.isReadable = true;
            ti.SaveAndReimport();
        }
    }
}