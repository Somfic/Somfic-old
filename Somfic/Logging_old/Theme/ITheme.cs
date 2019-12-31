﻿using System;

namespace Somfic.Logging_old.Theme
{
    public interface ITheme
    {
        ConsoleColor BackGround { get; }
        ConsoleColor Main { get; }
        ConsoleColor Warning { get; }
        ConsoleColor Error { get; }
        ConsoleColor Success { get; }
        ConsoleColor Debug { get; }
        ConsoleColor Special { get; }
    }
}