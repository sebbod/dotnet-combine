# dotnet-combine

# forked from eduherminio/dotnet-combine and downgrade .NET 6 to 5 SDK and one feature

`dotnet-combine` is .NET global tool that allows you to:

- Merge multiple C# source files (`.cs`) into a single one.
- Generate a `.zip` with the specified files within a directory.

## Installing `dotnet-combine`

This Fork downgrade .NET 6 SDK to 5 (for VS2019)

build it with visual studio

Once installed, `dotnet-combine --help` should show you the general options:

```bash
  single-file    Combines multiple source code files (.cs) into a single one.

  zip            Zips multiple files.

  help           Display more information on a specific command.

  version        Display version information.
```

## `dotnet-combine sample to integrate in Visual studio`
[Sorry image in french]
1 - you want to combine all files in Folder4CG
![alt text](https://github.com/sebbod/dotnet-combine/blob/20a041932c797f3331e7d56a2ccf4d6ac36f36a2/images/2023-06-29%2015_54_43-DotnetCombineSolution.png)
2 - Tools -> External tools
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2015_55_52-.png
3 - Add a new one like "DotnetCombine"
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2015_56_23-Outils%20externes.png

For this ^^ sample
DotnetCombine
D:\github\dotnet-combine\src\DotnetCombine\bin\Release\net5.0\dotnet-combine.exe
single-file $(ItemPath) --overwrite --P --output combined.cs
$(ItemDir)

+ check use output windows
+ IMPORTANT for a next step count the index of your external tool here for me it's 5

4 - Tools -> Customize
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2015_57_29-.png

5 - Command -> Contextual menu -> Project and solution | Folder
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2015_58_12-Personnaliser.png

6 - click add a command
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2015_58_22-Personnaliser.png

7 - select Tools in left list
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2015_58_37-Ajouter%20une%20commande.png

8 - select your number me it's 5 (see step 3 - external tool index)
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2015_58_58-Ajouter%20une%20commande.png

9 - yeah, you can now do that on every Folders in your solution
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2015_59_16-.png

10 - result file
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2016_00_09-DotnetCombineSolution.png

11 - result lines (yes 3 class1 it's not very usefull)
https://github.com/sebbod/dotnet-combine/blob/4eddb85ba166efc0443bcacd019c8e212bd6bcf2/images/2023-06-29%2016_00_19-DotnetCombineSolution.png

## `dotnet-combine single-file`

`dotnet-combine single-file` combines multiple C# source files (`.cs`) into a single, valid one.

This tool, originally inspired by [TomKirby/CGCompactor](https://github.com/TomKirby/CGCompactor), is handy if you need to submit a single source file somewhere; but you prefer to develop your solution locally using your preferred IDE and structuring it in multiple files.

Some examples are competitive programming contests such as [Google Code Jam](https://codingcompetitions.withgoogle.com/codejam), [CodeChef](https://www.codingame.com/), [CodingGame](https://www.codingame.com/), etc.

Help for `dotnet-combine single-file` can be accessed by running:

`dotnet-combine single-file --help`

```bash
Usage: dotnet-combine single-file <INPUT> [options]

  input (pos. 0)     Required.
                     Input path.

  -o, --output       Output path (file or dir).
                     If no dir path is provided (i.e. --output file.cs), the file will be created in the input dir.
                     If no filename is provided (i.e. --output dir/), a unique name will be used.

  -f, --overwrite    (Default: false)
                     Overwrites the output file (if it exists).

  --exclude          (Default: bin/ obj/)
                     Excluded files and directories, separated by semicolons (;).

  -p, --prefix       Prefix for the output file.

  -P, --prefixWithParentFolder       Prefix with the folder name for the output file.

  -s, --suffix       Suffix for the output file

  -v, --verbose      (Default: false)
                     Verbose output. Shows combined files, progress, etc.

  --format           (Default: false)
                     Formats output file. Enabling it slows down file generation process.

  --help             Display this help screen.

  --version          Display version information.
```

### `dotnet-combine single-file` usage examples

If you want to merge the source code of `MyAmazingProject.csproj`, located in `C:/MyAmazingProject`, you can simply do:

```bash
dotnet-combine single-file C:/MyAmazingProject
```

That will generated a file under `C:/MyAmazingProject/`. Since having that file there may conflict with your project compilation, you probably want it to be created somewhere else, let's say, under `C:/tmp/`. You can use `--output` for that:

```bash
dotnet-combine single-file C:/MyAmazingProject --output C:/tmp/
```

If you want that output file to have a specific name, you can also provide it:

```bash
dotnet-combine single-file C:/MyAmazingProject --output C:/tmp/amazing.cs
```

The option `--overwrite` can be used in subsequent runs to replace that file:

```bash
dotnet-combine single-file C:/MyAmazingProject --overwrite --output C:/tmp/amazing.cs
```

If you prefer to keep different versions of your output file, but would still like to identify them, you can use `--prefix` and/or `--suffix` together with your output dir in `--output`:

```bash
dotnet-combine single-file C:/MyAmazingProject --output C:/tmp/ --prefix amazing_
dotnet-combine single-file C:/MyAmazingProject --output C:/tmp/ --suffix _amazing
```

By default, `bin/` and `obj/` directories are excluded. That can be modified using `--exclude`:

```bash
dotnet-combine single-file C:/MyAmazingProject --output C:/tmp/ --prefix amazing_ --exclude "bin/;obj/;AutogeneratedDir/;AssemblyInfo.cs"
```

If you want your `MyAmazingProject.csproj` files to be combined every time you build the project in release mode, you can integrate `dotnet-combine single-file` as a MsBuild target in your .csproj file:

```xml
  <Target Name="dotnet-combine single-file" AfterTargets="Build" DependsOnTargets="Build" Condition="'$(Configuration)' == 'Release' ">
    <Message Importance="high" Text="Running dotnet-combine tool"/>
    <Message Importance="high" Text="dotnet-combine single-file $(MSBuildProjectDirectory) --overwrite --output $(MSBuildProjectDirectory)/bin/ --prefix amazing_ --exclude &quot;bin/;obj/;AutogeneratedDir/;AssemblyInfo.cs&quot;"/>
    <Exec Command="dotnet-combine single-file $(MSBuildProjectDirectory) --overwrite --output $(MSBuildProjectDirectory)/bin/ --prefix amazing_ --exclude &quot;bin/;obj/;AutogeneratedDir/;AssemblyInfo.cs&quot;"/>
  </Target>
```

## `dotnet-combine zip`

`dotnet-combine zip` generates a `.zip` with the specified files within a given directory.

Currently it supports filtering files by extension and excluding both files and directories.

This tool can be used for more general purposes than the previous one. Some of them can be:

- Extracting all `.pdf` files from a folder in your hard drive with complex hierarchies that has them mixed up with other files (`.docx`, `.pptx`, `.jpg`, etc.).

- Extracting all `.mp3` files from your mirrored cloud's folder.

- Extracting all `.css` files from a web development project.

- Extracting all the relevant source code files (`.sln`, `.csproj`, `.cs`, `.json`, `.razor`, `.xaml`, etc.) from your .NET project in a GitHub Action step, to be able to make them available as an artifact.

- ...

Help for `dotnet-combine zip` can be accessed by running:

`dotnet-combine zip --help`

```bash
Usage: dotnet-combine zip <INPUT> [options]

  input (pos. 0)     Required.
                     Input path (file or dir).

  -o, --output       Output path (file or dir).
                     If no dir path is provided (i.e. --output file.cs), the file will be created in the input
                     directory.
                     If no filename is provided (i.e. --output dir/), a unique name will be used.

  --extensions       (Default: .sln .csproj .cs .json)
                     File extensions to include, separated by semicolons (;).

  -f, --overwrite    (Default: false)
                     Overwrites the output file (if it exists).

  --exclude          (Default: bin/ obj/)
                     Excluded files and directories, separated by semicolons (;).

  -p, --prefix       Prefix for the output file.

  -P, --prefixWithParentFolder       Prefix with the folder name for the output file.

  -s, --suffix       Suffix for the output file.

  -v, --verbose      (Default: false)
                     Verbose output. Shows compressed files, progress, etc.

  --help             Display this help screen.

  --version          Display version information.
```

### `dotnet-combine zip` usage examples

If you want to create a `.zip` with all the C++ source code in your `MyGeekyProject` project, located in `/home/ed/GeekyProject/`, you can simply do:

```bash
dotnet-combine zip /home/ed/GeekyProject --extensions ".cpp;cxx:cc;.h;.hpp"
```

That will generated a file under `/home/ed/GeekyProject`. If you plan to create `.zip` files regularly, you may want to place them somewhere else, such as `/home/ed/GeekyProject/artifacts/`. You can use `--output` for that:

```bash
dotnet-combine zip /home/ed/GeekyProject --extensions ".cpp;cxx:cc;.h;.hpp;" --output /home/ed/GeekyProject/artifacts/
```

If you want that output file to have a specific name, you can also provide it using `--output`:

```bash
dotnet-combine zip /home/ed/GeekyProject --extensions ".cpp;cxx:cc;.h;.hpp" --output /home/ed/GeekyProject/artifacts/src_v1`
```

The tool will prevent you from generating another file with the same name unless you use `--overwrite` (also abbreviated as `-f`):

```bash
dotnet-combine zip /home/ed/GeekyProject -f --extensions ".cpp;cxx:cc;.h;.hpp" --output /home/ed/GeekyProject/artifacts/src_v1
```

However, you may prefer to keep different versions of your output file without having to worry about specifying different version numbers in the name. You can achieve that by only including a dir path in `--output`, and making use of `--prefix` and/or `--suffix`:

```bash
dotnet-combine zip /home/ed/GeekyProject --extensions ".cpp;cxx:cc;.h;.hpp" --prefix geeky- --output /home/ed/GeekyProject/artifacts/
```

By default, `bin/` and `obj/` directories are excluded. That can be modified by using `--exclude`:

```bash
dotnet-combine zip /home/ed/GeekyProject --extensions ".cpp;cxx:cc;.h;.hpp;.vcxproj;" --prefix geeky- --output /home/ed/GeekyProject/artifacts/ --exclude "build/;UnwantedFile.h"
```

If you want to pack your `MyGeekyProject` source files as part of your GitHub Actions CI pipeline, you can do it like this:

```yaml
    steps:
    - uses: actions/checkout@v2.3.3

    - name: Install dotnet-combine locally.
      run: |
        dotnet new tool-manifest
        dotnet tool install dotnet-combine

    - name: Create source code artifact using dotnet-combine.
      run: dotnet dotnet-combine zip . --extensions ".cpp;cxx:cc;.h;.hpp;.vcxproj;" --prefix geeky- --output ./artifacts/ --exclude "build/;UnwantedFile.h"

    - name: Upload source code artifact.
      uses: actions/upload-artifact@v2
      with:
        name: source-code-ci-${{ github.run_number }}
        path: artifacts/
        if-no-files-found: error
```

## Additional notes

- Although `dotnet-combine` does support `\` path separators in Windows, bear in mind that using them will prevent your commands/targets from being cross-platform.

- If you want use a suffix that starts with a dash (`-`), you can do that by using '[double-dash](https://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html)' (`--`) to indicate the end of options (i.e. `dotnet-combine single-file ./MyDir --suffix -- -my-suffix`).

## Contributing

If you experience difficulties using `dotnet-combine`, please [open an issue](https://github.com/eduherminio/dotnet-combine/issues/new/choose) detailing what you want to achieve and the command you've tried.

Feature requests are welcome.

PRs are also more than welcome, but feel free to open an issue before carrying out any major changes to avoid any misalignments.

[githubactionslogo]: https://github.com/eduherminio/dotnet-combine/actions/workflows/ci.yml/badge.svg
[githubactionslink]: https://github.com/eduherminio/dotnet-combine/actions/workflows/ci.yml
[nugetlogo]: https://img.shields.io/nuget/v/dotnet-combine.svg?style=flat-square&label=nuget
[nugetlink]: https://www.nuget.org/packages/dotnet-combine
[coveragelogo]: https://sonarcloud.io/api/project_badges/measure?project=eduherminio_dotnet-combine&metric=coverage
[coveragelink]: https://sonarcloud.io/dashboard?id=eduherminio_dotnet-combine
[sonarqubelink]: https://sonarcloud.io/dashboard?id=eduherminio_dotnet-combine
[sonarqualitylogo]: https://sonarcloud.io/api/project_badges/measure?project=eduherminio_dotnet-combine&metric=alert_status
[sonarvulnerabilitieslogo]: https://sonarcloud.io/api/project_badges/measure?project=eduherminio_dotnet-combine&metric=vulnerabilities
[sonarbugslogo]: https://sonarcloud.io/api/project_badges/measure?project=eduherminio_dotnet-combine&metric=bugs
[sonarcodesmellslogo]: https://sonarcloud.io/api/project_badges/measure?project=eduherminio_dotnet-combine&metric=code_smells
