// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Reflection;
global using Masa.BuildingBlocks.Data.Contracts.DataFiltering;
global using Masa.BuildingBlocks.Ddd.Domain.Entities;
global using Masa.BuildingBlocks.Dispatcher.Events;
global using Masa.Contrib.Data.UoW.EF;
global using Masa.Contrib.Dispatcher.Events;
global using Masa.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF;
global using Masa.Contrib.ReadWriteSpliting.Cqrs.Commands;
global using Masa.Contrib.ReadWriteSpliting.Cqrs.Queries;
global using Masa.Contrib.Service.MinimalAPIs;
global using MASA.PM.Contracts.Admin.Dto;
global using MASA.PM.Contracts.Admin.Model;
global using MASA.PM.Service.Admin.Application.App.Queries;
global using MASA.PM.Service.Admin.Application.Environment.Queries;
global using MASA.PM.Service.Admin.Application.Project.Queries;
global using MASA.PM.Service.Admin.Infrastructure;
global using MASA.PM.Service.Admin.Infrastructure.Entities;
global using MASA.PM.Service.Admin.Infrastructure.IRepositories;
global using MASA.PM.Service.Admin.Infrastructure.Middleware;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.OpenApi.Models;


