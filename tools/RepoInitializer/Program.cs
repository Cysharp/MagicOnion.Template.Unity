const string ProjectTemplateBaseName = "AppNamePlaceholder";

if (!args.Any())
{
    Console.Error.WriteLine("Usage: RepoInitializer <ProjectName>");
    Environment.Exit(1);
    return;
}

var projectName = args[0];
var repoRootDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", ".."));

Console.WriteLine($"Initializing the project...");
Console.WriteLine($"Project Name: {projectName}");
Console.WriteLine($"Working Directory: {Environment.CurrentDirectory}");
Console.WriteLine($"Repository Directory: {repoRootDir}");

// Verify that the project template exists
EnsureSuccess(File.Exists(Path.Combine(repoRootDir, $"{ProjectTemplateBaseName}.sln")), $"'{ProjectTemplateBaseName}.sln' is not found.  The project may have already been initialized.");
EnsureSuccess(Directory.Exists(Path.Combine(repoRootDir, $"src")), "'src' directory is not found.");
EnsureSuccess(Directory.Exists(Path.Combine(repoRootDir, $"src", $"{ProjectTemplateBaseName}.Server")), $"'{ProjectTemplateBaseName}.Server' directory is not found. The project may have already been initialized.");
EnsureSuccess(Directory.Exists(Path.Combine(repoRootDir, $"src", $"{ProjectTemplateBaseName}.Shared")), $"'{ProjectTemplateBaseName}.Shared' directory is not found. The project may have already been initialized.");
EnsureSuccess(Directory.Exists(Path.Combine(repoRootDir, $"src", $"{ProjectTemplateBaseName}.Unity")), $"'{ProjectTemplateBaseName}.Unity' directory is not found. The project may have already been initialized.");
EnsureSuccess(!Directory.Exists(Path.Combine(repoRootDir, $"src", $"{projectName}.Server")), $"'{projectName}.Server' directory already exists. The project may have already been initialized.");
EnsureSuccess(!Directory.Exists(Path.Combine(repoRootDir, $"src", $"{projectName}.Shared")), $"'{projectName}.Shared' directory already exists. The project may have already been initialized.");
EnsureSuccess(!Directory.Exists(Path.Combine(repoRootDir, $"src", $"{projectName}.Unity")), $"'{projectName}.Unity' directory already exists. The project may have already been initialized.");

// Rename the project template directories
RenameSolution();
RenameProjectDirectory(".Server");
RenameProjectDirectory(".Shared");
RenameProjectDirectory(".Unity");

void EnsureSuccess(bool condition, string message)
{
    if (!condition)
    {
        Console.Error.WriteLine($"Error: {message}");
        Environment.Exit(1);
    }
}

void RenameSolution()
{
    var src = Path.Combine(repoRootDir, $"{ProjectTemplateBaseName}.sln");
    var dest = Path.Combine(repoRootDir, $"{projectName}.sln");
    
    Console.WriteLine($"Rename {src} -> {dest}");

    // __AppName__.sln -> MyApp.sln
    File.Move(src, dest);

    // Rewrite the project name in .sln file
    var content = File.ReadAllText(dest);
    if (content.Contains(ProjectTemplateBaseName))
    {
        Console.WriteLine($"Rewriting {dest}...");
        content = content.Replace(ProjectTemplateBaseName, projectName);
        File.WriteAllText(dest, content);
    }
}

void RenameProjectDirectory(string suffix)
{
    var srcName = $"{ProjectTemplateBaseName}{suffix}";
    var destName = $"{projectName}{suffix}";
    var srcDir = Path.Combine(repoRootDir, "src", srcName);
    var destDir = Path.Combine(repoRootDir, "src", destName);

    Console.WriteLine($"Rename {srcName} -> {destName}");

    // __AppName__.Suffix -> MyApp.Suffix
    Directory.Move(srcDir, destDir);

    // __AppName__.Suffix.csproj -> MyApp.Suffix.csproj
    foreach (var fileName in Directory.EnumerateFiles(destDir, "*.*").Where(x => x.Contains(ProjectTemplateBaseName)))
    {
        if (File.Exists(Path.Combine(destDir, fileName)))
        {
            var newFileName = fileName.Replace(ProjectTemplateBaseName, projectName);
            Console.WriteLine($"Rename {fileName} -> {newFileName}");
            File.Move(Path.Combine(destDir, fileName), Path.Combine(destDir, newFileName));
        }
    }

    // Rewrite the project name in .cs files
    foreach (var path in Directory.EnumerateFiles(destDir, "*.*", SearchOption.AllDirectories)
                 .Where(x => !x.Contains(".vs") && !x.Contains(".idea"))
                 .Where(x => x.EndsWith(".json") || x.EndsWith(".cs") || x.EndsWith(".csproj") || x.EndsWith(".asmdef") || x.EndsWith(".mergesettings")))
    {
        var content = File.ReadAllText(path);
        if (content.Contains(ProjectTemplateBaseName, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Rewriting {path}...");
            content = content.Replace(ProjectTemplateBaseName, projectName);
            content = content.Replace(ProjectTemplateBaseName.ToLower(), projectName.ToLower());
            File.WriteAllText(path, content);
        }
    }
}
