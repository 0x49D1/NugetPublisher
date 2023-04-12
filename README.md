## What is this?
I know this one is quick and dirty solution, but it works for me...   
![image](https://user-images.githubusercontent.com/1378866/231454679-7cc44944-50f8-4c82-8186-414832e53603.png)   
This is just a simple project to try publish all local packages to local nuget(or not) at once. I was just tired to use console publish or "NuGet Package Explorer" for every project when we needed to. So created this small tool, put it into root directory of repositories with code and just doubleclick-to-run. By default it publishes executable for windows, but you're free to change that in project settings.   
Package files must be prepared for this tool to see them:   
```dotnet pack <PROJECT_NAME>.csproj -c Release```   
Or just use "Pack" functionality in Rider for example, it will create package in `bin` directory under the project.
## How to use
Put the published .exe to the folder you have you packages under. It will search all the subdirectories for *.nupkg based on "RegexPatternOfPackages" from configuration and NOT from "packages" subfolder. After that it will try to publish all found packages to your local nuget, based on configuration.
### Environment variables
By default project searches for `.env.nugetpublisher`, see `.env.example`:  
```
NugetHost=YOUR_NUGET_HOST   
NugetApiKey=YOUR_NUGET_API_KEY   
RegexPatternOfPackages=(\\bin\\)
```

