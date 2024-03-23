# Corn Jam 1

Oh we jamming

## Environment setup

### Install Git LFS

[Install Git LFS](https://docs.github.com/en/repositories/working-with-files/managing-large-files/installing-git-large-file-storage)

Verify Git LFS is working with `git lfs install` before cloning this repo

### Install Dotnet

Note: Not necessary if you are using Visual Studio, or VS Code

[Download Dotnet](https://dotnet.microsoft.com/en-us/download/dotnet)

I had to install the 6.0, 7.0, AND 8.0 runtimes + SDKs for all of the
tooling to work correctly.

Note: You do not need the ASP.NET, nor .NET Desktop runtimes. Only the SDKs +
the standard .NET runtimes.

### Editor Setup

If you are using VS Code or Visual Studio, please setup the Unity
extension/workload respectively:

- [VS Code](https://marketplace.visualstudio.com/items?itemName=visualstudiotoolsforunity.vstuc)
- [Visual Studio](https://learn.microsoft.com/en-us/visualstudio/gamedev/unity/get-started/getting-started-with-visual-studio-tools-for-unity?pivots=windows#install-visual-studio-and-unity)

### Install CSharpier

- [VS Code](https://marketplace.visualstudio.com/items?itemName=csharpier.csharpier-vscode)
- [Visual Studio](https://marketplace.visualstudio.com/items?itemName=csharpier.CSharpier)
- [Other editors](https://csharpier.com/docs/Editors)

### Install Roslyn LSP Server

Note: Only necessary if using VS Code/Neovim

Please use the official Roslyn-based LSP server, and not omnisharp/csharp-ls for
consistency:

- [VS Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit&ssr=false#overview)
  - Note: This installs `IntelliCode for C# Dev Kit` which has AI completions.
    Uninstall this if you do not want it.
- [Neovim](https://github.com/jmederosalvarado/roslyn.nvim)

### Download Unity

[Download Unity 2022.3.22](https://unity.com/releases/editor/whats-new/2022.3.22)

**Please** assure you are using 2022.3.22

### Configure Unity

Clone this repo, add the cloned directory as a project in Unity, and then open
the project.

NOTE: You may get this error: `The project file 'Assembly-CSharp.csproj' is in
unsupported format`. If you do, update the "Visual Studio Editor" version to
2.0.20 from the Unity Package Manager.

Lastly, in the `Edit` > `Preferences` settings menu within the Unity editor,
navigate to `External Tools`. Assure you have your primary editor selected for
the `External Script Editor` option.

## Contribution notes

- Do not use your file explorer, or IDE to move ANY asset in the project.
  This includes C# files. Unity tracks these files via unique IDs. In order to
  keep these IDs up to date and accurate, you MUST move the file within the
  Unity file explorer.
- We are using Git LFS, and I believe we only get 1gb of storage for free.
  Please message Jason if you are pushing large files! :)
