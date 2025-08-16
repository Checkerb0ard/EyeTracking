using UnityEngine;
using Object = UnityEngine.Object;

namespace EyeTracking.EyeGaze;

internal static class EyeGazeDebugOverlay
{
#if DEBUG
    private static bool _init;
    private static GUIStyle _panel;
    private static GUIStyle _header;
    private static GUIStyle _line;
    private static readonly Vector2 PanelPos = new(16f, 16f);
    private const float PanelWidth = 360f;
    private const float LinePadY = 2f;
    private const float SectionGap = 6f;

    private static readonly Rect LeftGazeRect  = new Rect(16f, 350f, 160f, 160f);
    private static readonly Rect RightGazeRect = new Rect(16f + 160f + 16f, 350f, 160f, 160f);
    private static Texture2D _dotTex;
#endif

    public static void Draw()
    {
#if DEBUG
        if (!_init) Init();

        var impl = ImplementationManager.CurrentImplementation;
        var eye = Tracking.Data?.Eye;

        if (impl == null || eye == null)
        {
            DrawNoData();
            return;
        }

        var left = eye.Left;
        var right = eye.Right;
        var combined = eye.Combined();
        if (float.IsNaN(combined.PupilDiameterMm)) combined.PupilDiameterMm = 0f;

        var lines = new List<(string, GUIStyle)>(32);

        Header(lines, "Eye Tracking Debug");
        Line(lines, F("Implementation", impl.Name));
        Line(lines, F("DeviceId", impl.DeviceId));
        Gap(lines);

        Header(lines, "Left Eye");
        Line(lines, F("Gaze", V2(left.Gaze)) + "  " + F("Open", left.Openness.ToString("F2")));
        Line(lines, F("Pupil mm", left.PupilDiameterMm.ToString("F2")));
        Gap(lines);

        Header(lines, "Right Eye");
        Line(lines, F("Gaze", V2(right.Gaze)) + "  " + F("Open", right.Openness.ToString("F2")));
        Line(lines, F("Pupil mm", right.PupilDiameterMm.ToString("F2")));
        Gap(lines);

        Header(lines, "Combined");
        Line(lines, F("Gaze", V2(combined.Gaze)) + "  " + F("Open", combined.Openness.ToString("F2")));
        Gap(lines);

        float contentHeight = 0f;
        foreach (var (t, s) in lines)
        {
            if (t == "")
            {
                contentHeight += SectionGap;
                continue;
            }
            contentHeight += s.CalcHeight(GC(t), PanelWidth - 24f) + LinePadY;
        }

        var panelRect = new Rect(PanelPos.x, PanelPos.y, PanelWidth, contentHeight + 20f);
        GUI.Box(panelRect, GUIContent.none, _panel);

        float y = PanelPos.y + 10f;
        float insideX = PanelPos.x + 12f;
        float innerW = PanelWidth - 24f;
        foreach (var (t, s) in lines)
        {
            if (t == "")
            {
                y += SectionGap;
                continue;
            }
            var gc = GC(t);
            float h = s.CalcHeight(gc, innerW);
            GUI.Label(new Rect(insideX, y, innerW, h), gc, s);
            y += h + LinePadY;
        }

        DrawEyeBox(LeftGazeRect, left.Gaze, left.Openness);
        DrawEyeBox(RightGazeRect, right.Gaze, right.Openness);
#endif
    }

#if DEBUG
    
    private static void DrawEyeBox(Rect rect, Vector2 gaze, float openness)
    {
        GUI.Box(rect, GUIContent.none, _panel);
        GUI.Label(new Rect(rect.x, rect.y - 18f, rect.width, 16f),"", _header);

        float gx = Mathf.Clamp(gaze.x, -1f, 1f);
        float gy = Mathf.Clamp(gaze.y, -1f, 1f);

        float px = rect.x + (gx * 0.5f + 0.5f) * rect.width;
        float py = rect.y + (1f - (gy * 0.5f + 0.5f)) * rect.height;

        Color prev = GUI.color;
        GUI.color = new Color(1f,1f,1f,0.15f);
        GUI.DrawTexture(new Rect(rect.x + rect.width * 0.5f - 0.5f, rect.y, 1f, rect.height), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height * 0.5f - 0.5f, rect.width, 1f), Texture2D.whiteTexture);
        GUI.color = prev;

        if (_dotTex == null) _dotTex = MakeTex(8,8,new Color(1f,1f,1f,1f));

        const float closedThreshold = 0.2f;
        Color dotColor = openness < closedThreshold ? new Color(1f, 0.15f, 0.4f, 1f) : Color.white;
        GUI.color = dotColor;
        const float d = 10f;
        var dotRect = new Rect(px - d * 0.5f, py - d * 0.5f, d, d);
        GUI.DrawTexture(dotRect, _dotTex);
        GUI.color = Color.white;
        
        GUI.color = new Color(0.65f, 0.3f, 0.9f, 0.6f);
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, 1f), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y, 1f, rect.height), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMax - 1f, rect.y, 1f, rect.height), Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    private static void DrawNoData()
    {
        if (!_init) Init();
        const float w = 260f;
        string msg = "Eye Tracking: (no data)";
        float h = _header.CalcHeight(GC(msg), w - 24f) + 20f;
        var rect = new Rect(PanelPos.x, PanelPos.y, w, h);
        GUI.Box(rect, GUIContent.none, _panel);
        GUI.Label(new Rect(rect.x + 12f, rect.y + 10f, w - 24f, h - 20f), msg, _header);
    }

    private static string F(string name, string value) => $"{name}: {value}";
    private static string V2(Vector2 v) => $"{v.x:+0.000;-0.000;0.000},{v.y:+0.000;-0.000;0.000}";
    private static void Header(List<(string, GUIStyle)> list, string t) => list.Add((t, _header));
    private static void Line(List<(string, GUIStyle)> list, string t) => list.Add((t, _line));
    private static void Gap(List<(string, GUIStyle)> list) => list.Add(("", _line));

    private static readonly GUIContent _gc = new();
    private static GUIContent GC(string t) { _gc.text = t; return _gc; }

    private static void Init()
    {
        _init = true;

        _panel = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(0,0,0,0),
            normal = { background = MakeTex(8,8, new Color(0.08f,0.08f,0.08f,0.80f)) },
            border = new RectOffset(6,6,6,6)
        };

        var mono = new GUIStyle(GUI.skin.label)
        {
            font = Resources.GetBuiltinResource<Font>("Lucida Console.ttf"),
            fontSize = 12,
            richText = false,
            normal = { textColor = Color.white }
        };
        
        _header = new GUIStyle(mono)
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            normal = { textColor = new Color(0.8f, 0.6f, 1f, 1f) }
        };
        _line = new GUIStyle(mono);
    }

    private static Texture2D MakeTex(int w, int h, Color c)
    {
        var tex = new Texture2D(w,h, TextureFormat.ARGB32, false){filterMode = FilterMode.Bilinear};
        var arr = new Color[w*h];
        for (int i=0;i<arr.Length;i++) arr[i]=c;
        tex.SetPixels(arr);
        tex.Apply();
        Object.DontDestroyOnLoad(tex);
        return tex;
    }
    
#endif
}