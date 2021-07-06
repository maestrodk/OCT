using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadClientTool
{
    public class DisplaySettings
    {
        public enum Aspect { Format4x3, Format16x9, Format16x10, Format21x9, Format21x10 };

        public class Resolution
        {
            public Aspect Aspect { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            public Resolution(int w, int h, Aspect a)
            {
                Width = w;
                Height = h;
                Aspect = a;
            }

            public override string ToString()
            {
                return $"{Width}x{Height}";
            }
        }

        private List<Resolution> resolutions = new List<Resolution>();
        private List<Resolution> resolutions4x3 = new List<Resolution>();
        private List<Resolution> resolutions16x9 = new List<Resolution>();
        private List<Resolution> resolutions16x10 = new List<Resolution>();
        private List<Resolution> resolutions21x9 = new List<Resolution>();
        private List<Resolution> resolutions21x10 = new List<Resolution>();

        public DisplaySettings()
        {
            AddFormats(Aspect.Format4x3);
            AddFormats(Aspect.Format16x9);
            AddFormats(Aspect.Format16x10);
            AddFormats(Aspect.Format21x9);
            AddFormats(Aspect.Format21x10);
        }

        public string DisplaySettingInfo(Resolution resolution)
        {
            return resolution.ToString();
        }

        private void AddFormats(Aspect aspect)
        {
            AddResolutions(640, aspect);
            AddResolutions(720, aspect);
            AddResolutions(800, aspect);
            AddResolutions(960, aspect);
            AddResolutions(1024, aspect);
            AddResolutions(1280, aspect);
            AddResolutions(1366, aspect);
            AddResolutions(1440, aspect);
            AddResolutions(1600, aspect);
            AddResolutions(1680, aspect);
            AddResolutions(1920, aspect);
            AddResolutions(2048, aspect);
            AddResolutions(2560, aspect);
            AddResolutions(2880, aspect);
            AddResolutions(3440, aspect);
            AddResolutions(3840, aspect);
            AddResolutions(4096, aspect);
            AddResolutions(5120, aspect);
        }

        private void AddResolutions(int w, Aspect aspect)
        {
            double a = 1.0;
            int test;

            switch (aspect)
            {
                case Aspect.Format21x9:
                    a = 21.0 / 9.0;
                    test = (int)(w / a);
                    if ((test % 2) == 1) a = 3440.0 / 1440.0;
                    if (w == 3440) a = 3440.0 / 1440.0;
                    break;

                case Aspect.Format21x10:
                    a = 21.0 / 10.0;
                    break;

                case Aspect.Format16x9:
                    a = 16.0 / 9.0;
                    break;

                case Aspect.Format16x10:
                    a = 16.0 / 10.0;
                    break;

                default:
                    a = 4.0 / 3.0;
                    break;
            }

            test = (int)(w / a);

            if ((test % 2) == 0)
            {
                resolutions.Add(new Resolution(w, (int)(w / a), aspect));

                switch (aspect)
                {
                    case Aspect.Format21x9:
                        resolutions21x9.Add(new Resolution(w, (int)(w / a), aspect));
                        break;

                    case Aspect.Format21x10:
                        resolutions21x10.Add(new Resolution(w, (int)(w / a), aspect));
                        break;

                    case Aspect.Format16x9:
                        resolutions16x9.Add(new Resolution(w, (int)(w / a), aspect));
                        break;

                    case Aspect.Format16x10:
                        resolutions16x10.Add(new Resolution(w, (int)(w / a), aspect));
                        break;

                    default:
                        resolutions4x3.Add(new Resolution(w, (int)(w / a), aspect));
                        break;
                }
            }
        }
    }
}
