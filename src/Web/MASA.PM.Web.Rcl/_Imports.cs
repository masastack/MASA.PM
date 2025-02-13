// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

global using System.Reflection;
global using System.Security.Cryptography.X509Certificates;
global using System.Text.RegularExpressions;
global using FluentValidation;
global using FluentValidation.Resources;
global using DeepCloner.Core;
global using Masa.Blazor;
global using Masa.BuildingBlocks.StackSdks.Auth.Contracts;
global using Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;
global using Masa.Contrib.StackSdks.Config;
global using Masa.Stack.Components;
global using Masa.Stack.Components.Configs;
global using Masa.Stack.Components.Extensions;
global using Masa.Stack.Components.Extensions.OpenIdConnect;
global using MASA.PM.Caller.Callers;
global using MASA.PM.Contracts.Admin.Dto;
global using MASA.PM.Contracts.Admin.Enum;
global using MASA.PM.Web.Rcl.Data.Base;
global using MASA.PM.Web.Rcl.Model;
global using MASA.PM.Web.Docs;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Hosting.StaticWebAssets;
global using Microsoft.IdentityModel.Logging;
global using Masa.Stack.Components;
