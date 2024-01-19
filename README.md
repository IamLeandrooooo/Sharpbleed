# Sharpbleed
A tool made in C# to exploit the Heartbleed vulnerability.


# **Disclaimer: Legal Use Only**

This script is provided solely for legal purposes. Any use of this script for illegal activities or in violation of applicable laws is strictly prohibited.

I take no responsibility if:

- You use this script for unlawful purposes.
- You encounter any legal consequences as a result of using this script inappropriately.

# What is Sharpbleed
Sharpbleed is a tool created using C# and .NET 8. It can be compiled for Windows, Linux and MacOS systems, since .NET 8 has the fantastic advantage of being cross-platform.

# Arquitecture
The solution has a very basic arquitecture consisting in a Console Application that has a Helper Class, a Constants Class, and the Main class where all the magic happens.

The representation of the solution can be seen as such:
```
SharpBleed
├─ Dependencies
├─ Constants
   ├─ Constants.cs
├─ Helper
   ├─ HeartBleedHelper.cs
├─ Program.cs
```

# How to run it

```
 .\SharpBleed.exe  <TARGET IP> <TARGET PORT> <PATH TO SAVE GENERATED DUMP>
```

# How to compile it
If you need to use SharpBleed you can compile it using the dotnet build command. In this case, let's say you wanted to use it on a Linux system, you can use the following:
```
dotnet build --configuration Release --runtime linux-x64
```
For Windows:
```
dotnet build --configuration Release --runtime win-x64
```
Or for MacOS:
```
dotnet build --configuration Release --runtime osx-x64
```

These are just examples for each Operative System, if you need something more specific, either to the OS architecture or the OS type, you can check all the RID's to use in the --runtime flag on the following link:

[Dotnet build command available RID's](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)









