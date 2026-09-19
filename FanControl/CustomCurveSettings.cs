using CommonHelpers;
using System.Globalization;

namespace FanControl
{
    public sealed class CustomCurveSettings : BaseSettings
    {
        public static readonly CustomCurveSettings Default = new CustomCurveSettings();

        public const string DefaultCurve = "50:2200, 55:3200, 60:4200, 65:5200, 70:6200, 75:6800, 80:7300";

        public struct CurvePoint
        {
            public float Temp;
            public ushort RPM;

            public CurvePoint(float temp, ushort rpm)
            {
                Temp = temp;
                RPM = rpm;
            }
        }

        private DateTime lastConfigFileTime = DateTime.MinValue;
        private List<CurvePoint> points = new List<CurvePoint>();
        private readonly object lockObj = new object();

        public CustomCurveSettings() : base("CustomCurve")
        {
            TouchSettings = true;
            ResolveConfigPath();
            Reload();
        }

        private void ResolveConfigPath()
        {
            if (!File.Exists(ConfigFile))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var dllIni = Path.Combine(baseDir, "FanControl.dll.ini");
                if (File.Exists(dllIni))
                {
                    ConfigFile = dllIni;
                    return;
                }

                var exeIni = Path.Combine(baseDir, "FanControl.exe.ini");
                if (File.Exists(exeIni))
                {
                    ConfigFile = exeIni;
                    return;
                }
            }
        }

        public string Curve
        {
            get { return Get<string>("Curve", DefaultCurve, touchSettings: true); }
            set { Set("Curve", value); }
        }

        public void ReloadIfNeeded()
        {
            try
            {
                ResolveConfigPath();
                if (File.Exists(ConfigFile))
                {
                    var writeTime = File.GetLastWriteTimeUtc(ConfigFile);
                    if (writeTime != lastConfigFileTime)
                    {
                        Reload();
                    }
                }
            }
            catch
            {
                // Fallback / ignore transient I/O exceptions
            }
        }

        public void Reload()
        {
            lock (lockObj)
            {
                try
                {
                    if (File.Exists(ConfigFile))
                    {
                        lastConfigFileTime = File.GetLastWriteTimeUtc(ConfigFile);
                    }
                    ClearCache();
                    string curveStr = Curve;
                    var parsed = ParseCurveString(curveStr);
                    if (parsed.Count > 0)
                    {
                        points = parsed;
                    }
                    else
                    {
                        points = ParseCurveString(DefaultCurve);
                    }
                }
                catch
                {
                    points = ParseCurveString(DefaultCurve);
                }
            }
        }

        public static List<CurvePoint> ParseCurveString(string str)
        {
            var result = new List<CurvePoint>();
            if (string.IsNullOrWhiteSpace(str))
                return result;

            var parts = str.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                var kv = part.Split(':');
                if (kv.Length == 2)
                {
                    if (float.TryParse(kv[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float temp) &&
                        ushort.TryParse(kv[1].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out ushort rpm))
                    {
                        rpm = Math.Min(rpm, Vlv0100.MAX_FAN_RPM);
                        result.Add(new CurvePoint(temp, rpm));
                    }
                }
            }

            result.Sort((a, b) => a.Temp.CompareTo(b.Temp));
            return result;
        }

        public ushort CalculateRPM(float input)
        {
            ReloadIfNeeded();

            List<CurvePoint> pts;
            lock (lockObj)
            {
                pts = points;
            }

            if (pts == null || pts.Count == 0)
                return 2200;

            if (input <= pts[0].Temp)
                return pts[0].RPM;

            if (input >= pts[pts.Count - 1].Temp)
                return pts[pts.Count - 1].RPM;

            for (int i = 0; i < pts.Count - 1; i++)
            {
                if (input >= pts[i].Temp && input <= pts[i + 1].Temp)
                {
                    float range = pts[i + 1].Temp - pts[i].Temp;
                    if (range <= 0.001f)
                        return pts[i + 1].RPM;

                    float factor = (input - pts[i].Temp) / range;
                    float rpm = pts[i].RPM + factor * (pts[i + 1].RPM - pts[i].RPM);
                    return (ushort)Math.Clamp((int)Math.Round(rpm), 0, (int)Vlv0100.MAX_FAN_RPM);
                }
            }

            return pts[pts.Count - 1].RPM;
        }
    }
}
