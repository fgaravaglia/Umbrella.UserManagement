# Repository Content
Library to implement lightweight User Management domain

[![Build Status](https://garaproject.visualstudio.com/UmbrellaFramework/_apis/build/status/fgaravaglia.Umbrella.UserManagement?branchName=main)](https://garaproject.visualstudio.com/UmbrellaFramework/_build/latest?definitionId=79&branchName=main)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Umbrella.UserManagement&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Umbrella.UserManagement)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Umbrella.UserManagement&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Umbrella.UserManagement)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Umbrella.UserManagement&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Umbrella.UserManagement)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Umbrella.UserManagement&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=Umbrella.UserManagement)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Umbrella.UserManagement&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Umbrella.UserManagement)

# Installation
To install it, use proper command:
```
dotnet add package Umbrella.UserManagement 
```

[![Nuget](https://img.shields.io/nuget/v/Umbrella.UserManagement.svg?style=plastic)](https://www.nuget.org/packages/Umbrella.UserManagement/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Umbrella.UserManagement.svg)](https://www.nuget.org/packages/Umbrella.UserManagement/)

For more details about download, see [NuGet Web Site](https://www.nuget.org/packages/Umbrella.UserManagement/)


# Usage
to setup the repository at application startup, use the following extension:

```c#

using Umbrella.UserManagement;
. . .
builder.Services.AddUserManagement(Environment.CurrentDirectory);

```

# Usage with Google Firestore

to setup the repository at application startup, use the following extension:

```c#

using Umbrella.UserManagement.Firestore;
. . .
builder.Services.AddFirestoreUserManagement(Environment.CurrentDirectory);

```
