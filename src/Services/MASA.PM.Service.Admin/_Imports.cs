﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Reflection;
global using Mapster;
global using Masa.BuildingBlocks.Authentication.Identity;
global using Masa.BuildingBlocks.Data.UoW;
global using Masa.BuildingBlocks.Ddd.Domain.Entities;
global using Masa.BuildingBlocks.Ddd.Domain.Entities.Full;
global using Masa.BuildingBlocks.Dispatcher.Events;
global using Masa.BuildingBlocks.Dispatcher.IntegrationEvents;
global using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Commands;
global using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Queries;
global using Masa.BuildingBlocks.StackSdks.Auth.Contracts;
global using Masa.BuildingBlocks.StackSdks.Auth.Contracts.Consts;
global using Masa.BuildingBlocks.StackSdks.Config;
global using Masa.BuildingBlocks.StackSdks.Dcc;
global using Masa.Contrib.Caching.Distributed.StackExchangeRedis;
global using Masa.Contrib.Dispatcher.Events;
global using Masa.Contrib.StackSdks.Config;
global using Masa.Contrib.StackSdks.Middleware;
global using Masa.Contrib.StackSdks.Tsc;
global using Masa.Utils.Configuration.Json;
global using MASA.PM.Contracts.Admin.Dto;
global using MASA.PM.Contracts.Admin.Enum;
global using MASA.PM.Service.Admin.Application.App.Commands;
global using MASA.PM.Service.Admin.Application.App.Queries;
global using MASA.PM.Service.Admin.Application.Environment.Commands;
global using MASA.PM.Service.Admin.Application.Environment.Queries;
global using MASA.PM.Service.Admin.Application.Project.Queries;
global using MASA.PM.Service.Admin.Infrastructure;
global using MASA.PM.Service.Admin.Infrastructure.Entities;
global using MASA.PM.Service.Admin.Infrastructure.IRepositories;
global using MASA.PM.Service.Admin.Migrations;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.OpenApi.Models;
global using AppModel = MASA.PM.Contracts.Admin.Model.AppModel;
global using ProjectModel = MASA.PM.Contracts.Admin.Model.ProjectModel;
global using FluentValidation.Resources;
global using FluentValidation;
global using Masa.BuildingBlocks.Globalization.I18n; 
