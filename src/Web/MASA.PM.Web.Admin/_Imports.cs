// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

global using System.Reflection;
global using System.Security.Cryptography.X509Certificates;
global using System.Text.RegularExpressions;
global using BlazorComponent;
global using BlazorComponent.I18n;
global using Masa.Blazor;
global using Masa.BuildingBlocks.StackSdks.Auth;
global using Masa.BuildingBlocks.StackSdks.Auth.Contracts;
global using Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;
global using Masa.BuildingBlocks.StackSdks.Config;
global using Masa.Contrib.Service.Caller.Authentication.OpenIdConnect;
global using Masa.Contrib.StackSdks.Config;
global using Masa.Contrib.StackSdks.Tsc;
global using Masa.Stack.Components;
global using Masa.Stack.Components.Configs;
global using Masa.Stack.Components.Extensions.OpenIdConnect;
global using MASA.PM.Caller.Callers;
global using MASA.PM.Contracts.Admin.Dto;
global using MASA.PM.Contracts.Admin.Enum;
global using MASA.PM.UI.Admin.Data.Base;
global using MASA.PM.UI.Admin.Model;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Hosting.StaticWebAssets;
global using Microsoft.IdentityModel.Logging;
global using Force.DeepCloner;
