// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

global using System.Globalization;
global using Mapster;
global using Masa.BuildingBlocks.Authentication.Identity;
global using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Commands;
global using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Queries;
global using Masa.BuildingBlocks.StackSdks.Config;
global using Masa.BuildingBlocks.StackSdks.Dcc;
global using Masa.Contrib.Dispatcher.Events;
global using Masa.Contrib.StackSdks.Config;
global using MASA.PM.Contracts.Admin.Dto;
global using MASA.PM.Contracts.Admin.Enum;
global using MASA.PM.Contracts.Admin.Model;
global using MASA.PM.Infrastructure.Domain;
global using MASA.PM.Infrastructure.Domain.App.Commands;
global using MASA.PM.Infrastructure.Domain.App.Queries;
global using MASA.PM.Infrastructure.Domain.Cluster.Commands;
global using MASA.PM.Infrastructure.Domain.Cluster.Queries;
global using MASA.PM.Infrastructure.Domain.Environment.Commands;
global using MASA.PM.Infrastructure.Domain.Environment.Queries;
global using MASA.PM.Infrastructure.Domain.Project.Commands;
global using MASA.PM.Infrastructure.Domain.Project.Queries;
global using MASA.PM.Infrastructure.Domain.Shared.Entities;
global using MASA.PM.Infrastructure.Repository;
