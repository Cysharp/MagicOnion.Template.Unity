# MagicOnion.Template.Unity

This repository provides a template for creating a project that has completed the [Quick Start with Unity and .NET Server](https://cysharp.github.io/MagicOnion/quickstart-unity).

This template based on the "Universal 3D" template using Unity 6000.0.36f1.

### How to set up

You can download the archive file from GitHub and extract it, or create a repository from the GitHub template feature. The following is an example command to extract the template to the `MyApp` directory.

```bat
REM Windows (cmd.exe or PowerShell)
mkdir MyApp
cd MyApp
curl.exe -L -o - https://github.com/Cysharp/MagicOnion.Template.Unity/archive/refs/heads/main.tar.gz | tar xz -C . --strip-component 1
```

```bash
# Bash, zsh
mkdir MyApp
cd MyApp
curl -L -o - https://github.com/Cysharp/MagicOnion.Template.Unity/archive/refs/heads/main.tar.gz | tar xz -C . --strip-component 1
```

After extracting the source code, run `init.cmd` or `init.sh` with an arbitrary project name (e.g., `MyApp`). This script performs preparation such as renaming projects and files in the repository root.

```bash
init.cmd MyApp
```

```bash
bash init.sh MyApp
```

After running the script, you can delete `init.sh` and `init.cmd` and `tools/RepoInitializer` that actually perform the rewriting process.

### License
The repository is provided under the [CC0 - Public Domain](https://creativecommons.org/publicdomain/zero/1.0/) license.
