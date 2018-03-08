using UnityEngine;

public static class DebugUtil
{

    public static void DrawRect(float x0, float x1, float y0, float y1)
    {
        DrawRect(x0, x1, y0, y1, Color.green);
    }

    public static void DrawRect(float x0, float x1, float y0, float y1, Color color, float duration = 0.0f)
    {
        Vector2 tl = new Vector2(x0, y0);
        Vector2 tr = new Vector2(x1, y0);
        Vector2 br = new Vector2(x1, y1);
        Vector2 bl = new Vector2(x0, y1);
        Debug.DrawLine(tl, tr, color, duration);
        Debug.DrawLine(tr, br, color, duration);
        Debug.DrawLine(br, bl, color, duration);
        Debug.DrawLine(bl, tl, color, duration);
    }

    // DrawString: From https://gist.github.com/Arakade/9dd844c2f9c10e97e3d0
    static public void DrawString(string text, Vector3 worldPos, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();

        var restoreColor = GUI.color;

        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
        {
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
            return;
        }

        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        GUI.color = restoreColor;
        UnityEditor.Handles.EndGUI();
    }

    /*
    *   a b
    *  cdefg
    *   h i
    *  jklmn
    *   o p
    *   
    *   bits: abcd efgh ijkl mnop
    */
    // VectorText: From http://dinodini.weebly.com/debugdrawtext.html
    public static void VectorText(Vector3 p, string v, float size = 1.0f)
    {
        Color col = Color.white;
        float dur = 0.0f;

        Vector4[] lines = new Vector4[] {
            new Vector4( -1,1,0,1 ),    // a
            new Vector4( 0,1,1,1),      // b
            new Vector4( -1,1,-1,0),    // c
            new Vector4( -1,1,0,0),     // d
            new Vector4( 0,1,0,0),       // e
            new Vector4( 1,1,0,0),       // f
            new Vector4( 1,1,1,0),       // g
            new Vector4( -1,0,0,0),      // h
            new Vector4(0,0,1,0),       // i
            new Vector4(-1,0,-1,-1),       // j
            new Vector4(0,0,-1,0-1),       // k
            new Vector4(0,0,0,-1),       // l
            new Vector4(0,0,1,-1),       // m
            new Vector4(1,0,1,-1),       // n
            new Vector4(-1,-1,0,-1),       // o
            new Vector4(0,-1,1,-1),       // p
        };

        int[] chars = {
            0xe667, // 0
            0x0604, // 1
            0xc3c3, // 2
            0xc287, // 3
            0x2990, // 4
            0xe187, // 5
            0xe1c7, // 6
            0xc410, // 7
            0xe3c7, // 8
            0xe384, // 9

            0xe3c4, // A
            0xca97, // B
            0xe043, // C
            0xca17, // D
            0xe1c3, // E
            0xe140, // F
            0xe0c7, // G
            0x23c4, // H
            0xc813, // I
            0xc852, // J
            0x2548, // K
            0x2043, // L
            0x3644, // M
            0x324c, // N
            0xe247, // O
            0xe3c0, // P
            0xe24f, // Q
            0xe3c8, // R
            0xd087, // S
            0xc810, // T
            0x2247, // U
            0x2460, // V
            0x226c, // W
            0x1428, // X
            0x1410, // Y
            0xc423, // Z

            0x0000, // space
            0x0002, // .
            0x0100, // -
        };

        for (int m = 0; m < v.Length; m++)
        {
            int n = v[m];
            int c = -1;
            if (n >= '0' && n <= '9')
            {
                n = n - '0';
                c = chars[n];
            }
            else if (n >= 'A' && n <= 'Z')
            {
                n = n - 'A' + 10;
                c = chars[n];
            }
            else if (n >= 'a' && n <= 'z')
            {
                n = n - 'a' + 10;
                c = chars[n];
            }
            else if (n == ' ')
            {
                c = chars[26 + 10];
            }
            else if (n == '.')
            {
                c = chars[26 + 11];
            }
            else if (n == '-')
            {
                c = chars[26 + 11];
            }
            for (int i = 0; i < 16; i++)
            {
                if ((c & (1 << (15 - i))) != 0)
                {
                    Debug.DrawLine(
                        new Vector3(m * 2.0f * size + p.x + lines[i].x / 2 * size, p.y + lines[i].y * size, 0.0f),
                        new Vector3(m * 2.0f * size + p.x + lines[i].z / 2 * size, p.y + lines[i].w * size, 0.0f),
                        col,
                        dur);
                }
            }
        }
    }
}
