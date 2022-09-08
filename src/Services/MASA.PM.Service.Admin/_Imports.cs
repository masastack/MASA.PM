// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Reflection;
global using Mapster;
global using Masa.BuildingBlocks.Configuration;
global using Masa.BuildingBlocks.Ddd.Domain.Entities;
global using Masa.BuildingBlocks.Ddd.Domain.Entities.Full;
global using Masa.BuildingBlocks.Dispatcher.Events;
global using Masa.BuildingBlocks.ReadWriteSpliting.Cqrs.Commands;
global using Masa.BuildingBlocks.ReadWriteSpliting.Cqrs.Queries;
global using Masa.BuildingBlocks.StackSdks.Dcc;
global using Masa.Contrib.Configuration.ConfigurationApi.Dcc;
global using Masa.Contrib.Data.Contracts.EFCore;
global using Masa.Contrib.Data.UoW.EFCore;
global using Masa.Contrib.Dispatcher.Events;
global using Masa.Contrib.Dispatcher.IntegrationEvents.Dapr;
global using Masa.Contrib.Dispatcher.IntegrationEvents.EventLogs.EFCore;
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
global using MASA.PM.Service.Admin.Infrastructure.Middleware;
global using MASA.PM.Service.Admin.Migrations;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.OpenApi.Models;
global using AppModel = MASA.PM.Contracts.Admin.Model.AppModel;
global using ProjectModel = MASA.PM.Contracts.Admin.Model.ProjectModel;
