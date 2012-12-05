# SVNChangeSetCreator

### Please Note: This project is under development. Not available for public use

## Introduction

This helps to create a changeset of a given archive to the specified destination. The data will be copied under old/new directories respectively.

## Colophon
 - Zip implemented using DotNetZip library - http://dotnetzip.codeplex.com/
 - Svn Implementation using SharpSVN 1.7 library - http://sharpsvn.open.collab.net/
 - Visual Studio 2010 (.NET Framework 3.5)

## How the changesets are made?
This application using SharpSVN 1.7 (x86) for populating the change list. The changesets are made using `SvnClient` class and check against `LocalContentStatus`. The implementation can be found at `SvnChangeSet.cs` in `LibSvnChangeSet` class
The zip file is made using DotNetZip library and use can choose to zip or store as raw changes in the folder specified by the user

## Background Worker
To get the changesets [BackgroundWorker](http://msdn.microsoft.com/en-us/library/system.componentmodel.backgroundworker.aspx) class is used. The user can give custom event handler to subscribe for notifications.

## MetroUI
The Application features different type of UI. The standalone Metro application uses [Mahapps.Metro](https://github.com/MahApps/MahApps.Metro) Framework.

## The woes of different .NET Frameworks (2.0 and 4.0)
This application is solely created using .NET Framework 4.0 but SharpSVN library is developed using .NET Framework 2.0. The applications neesd to change the `app.config` file as follows. (need to add `useLegacyV2RuntimeActivationPolicy` inside `startup` node.

```
<?xml version="1.0"?>
<configuration>
  <startup useLegacyV2RuntimeActivationPolicy="true">
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.0,Profile=Client" />
  </startup>
  
</configuration>
```