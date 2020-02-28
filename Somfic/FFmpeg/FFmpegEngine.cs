using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Somfic.Logging;

namespace Somfic.FFmpeg
{
    public class FFmpegEngine
    {
        public FFmpegEngine(string path)
        {
            Logger.Verbose($"Confirming FFmpeg path '{path}'.");
            if (!File.Exists(path))
            {
                Exception ex = new FileNotFoundException("FFmpeg path could not be found.", path);
                Logger.Error($"FFmpeg path '{path}' is invalid.", ex);
            }
        }
    }
}
