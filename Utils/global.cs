using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
namespace App;

public static class G
{
    public static IEnumerable<T> EnumIterate<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    [DoesNotReturn]
    public static void Todo(string? message = null)
    {
        throw new NotImplementedException($"TODO: {message ?? "no message"}");
    }

    [DoesNotReturn]
    public static T Todo<T>(string? message = null)
    {
        throw new NotImplementedException($"TODO: {message ?? "no message"}");
    }

    [DoesNotReturn]
    public static void Unreachable(string? message = null)
    {
        throw new UnreachableException($"UNREACHABLE: {message ?? "no message"}");
    }

    [DoesNotReturn]
    public static T Unreachable<T>(string? message = null)
    {
        throw new UnreachableException($"UNREACHABLE: {message ?? "no message"}");
    }
}

public static class Log
{
    public enum Level {
        Debug,
        Info,
        Warning,
        Error,
        Fatal,
    }

    public static void Write(Level level, string message)
    {
        var writer = level switch {
            Level.Debug or Level.Info or Level.Warning => Console.Out,
            Level.Error or Level.Fatal => Console.Error,
            _ => G.Unreachable<TextWriter>(nameof(Level)),
        };
        var level_str = level switch {
            Level.Debug   => "DEBUG",
            Level.Info    => "INFO",
            Level.Warning => "WARNING",
            Level.Error   => "ERROR",
            Level.Fatal   => "FATAL",
            _ => G.Unreachable<string>(nameof(Level)),
        };
        writer.WriteLine($"{level_str}: {message}");
    }

    public static void Debug(string message)   => Write(Level.Debug, message);
    public static void Info(string message)    => Write(Level.Info, message);
    public static void Warning(string message) => Write(Level.Warning, message);
    public static void Error(string message)   => Write(Level.Error, message);
    public static void Fatal(string message)   => Write(Level.Fatal, message);
}
