using Spectre.Console;

namespace ImageGenerationApp.Utils;

internal class ConsoleHelper
{
    public static void CreateHeader()
    {
        AnsiConsole.Clear();

        Grid grid = new();
        grid.AddColumn();
        grid.AddRow(new FigletText("Image Generation").Centered().Color(Color.Red));
        grid.AddRow(Align.Center(new Panel("[red]Sample by Thomas Sebastian Jensen ([link]https://www.tsjdev-apps.de[/])[/]")));

        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }

    public static string SelectFromOptions(List<string> options)
    {
        CreateHeader();

        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select from the following [yellow]options[/]?")
            .AddChoices(options));
    }

    public static string GetUrl(string prompt)
    {
        CreateHeader();

        return AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
            .PromptStyle("white")
            .ValidationErrorMessage("[red]Invalid prompt[/]")
            .Validate(prompt =>
            {
                if (prompt.Length < 3)
                {
                    return ValidationResult.Error("[red]URL too short[/]");
                }

                if (prompt.Length > 250)
                {
                    return ValidationResult.Error("[red]URL too long[/]");
                }

                if (Uri.TryCreate(prompt, UriKind.Absolute, out Uri? uri)
                    && uri.Scheme == Uri.UriSchemeHttps)
                {
                    return ValidationResult.Success();
                }

                return ValidationResult.Error("[red]No valid URL[/]");
            }));
    }

    public static string GetString(string prompt)
    {
        CreateHeader();

        return AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
            .PromptStyle("white")
            .ValidationErrorMessage("[red]Invalid prompt[/]")
            .Validate(prompt =>
            {
                if (prompt.Length < 3)
                {
                    return ValidationResult.Error("[red]Value too short[/]");
                }

                if (prompt.Length > 200)
                {
                    return ValidationResult.Error("[red]Value too long[/]");
                }

                return ValidationResult.Success();
            }));
    }

    public static void WriteString(string input)
    {
        CreateHeader();

        AnsiConsole.MarkupLine(input);
    }
}
