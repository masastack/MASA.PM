﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

global using System.Reflection;
global using System.Text.Json;
global using FluentValidation;
global using FluentValidation.Resources;
global using Mapster;
global using Masa.BuildingBlocks.Data.UoW;
global using Masa.BuildingBlocks.Ddd.Domain.Repositories;
global using Masa.BuildingBlocks.Dispatcher.Events;
global using Masa.BuildingBlocks.Dispatcher.IntegrationEvents;
global using Masa.BuildingBlocks.StackSdks.Auth.Contracts;
global using Masa.BuildingBlocks.StackSdks.Config;
global using Masa.BuildingBlocks.StackSdks.Config.Consts;
global using Masa.BuildingBlocks.StackSdks.Config.Models;
global using Masa.Contrib.Caching.Distributed.StackExchangeRedis;
global using Masa.Contrib.StackSdks.Config;
global using Masa.Contrib.StackSdks.Middleware;
global using Masa.Contrib.StackSdks.Tsc;
global using MASA.PM.Contracts.Admin.Dto;
global using MASA.PM.Contracts.Admin.Enum;
global using MASA.PM.Infrastructure.Domain.App.Commands;
global using MASA.PM.Infrastructure.Domain.App.Queries;
global using MASA.PM.Infrastructure.Domain.Cluster.Commands;
global using MASA.PM.Infrastructure.Domain.Cluster.Queries;
global using MASA.PM.Infrastructure.Domain.Environment.Commands;
global using MASA.PM.Infrastructure.Domain.Environment.Queries;
global using MASA.PM.Infrastructure.Domain.Project.Commands;
global using MASA.PM.Infrastructure.Domain.Project.Queries;
global using MASA.PM.Infrastructure.Domain.Shared.Entities;
global using MASA.PM.Infrastructure.EFCore;
global using MASA.PM.Infrastructure.Repository;
global using MASA.PM.Service.Admin;
global using MASA.PM.Service.Admin.Services;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.OpenApi.Models;
global using ProjectModel = MASA.PM.Contracts.Admin.Model.ProjectModel;
