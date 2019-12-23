using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Somfic.Logging_old;

namespace Somfic.FFmpeg
{
    public class FFmpegEngine
    {
        private Logger Logger;

        public FFmpegEngine(string path, Logger logger)
        {
            Logger = logger;
            Logger.Debug($"Confirming FFmpeg path '{path}'.");
            if (!File.Exists(path))
            {
                Exception ex = new FileNotFoundException("FFmpeg path could not be found.", path);
                Logger.Error($"FFmpeg path '{path}' is invalid.", ex);
            }
        }
    }
}
