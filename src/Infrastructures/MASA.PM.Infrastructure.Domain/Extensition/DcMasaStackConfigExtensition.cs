// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Config;

public static class DcMasaStackConfigExtensition
{
    public static List<AddProjectAppDto> GetProjectApps(this IMasaStackConfig masaStackConfig)
    {
        var masaStack = masaStackConfig.GetMasaStack();
        List<AddProjectAppDto> projectApps = new List<AddProjectAppDto>();
        foreach (var project in masaStack)
        {
            if (project == null)
            {
                continue;
            }

            if (project["id"] == null)
            {
                continue;
            }

            var id = project["id"]!.ToString();
            MasaStack.MasaStackIdNamePairs.TryGetValue(id, out var name);
            AddProjectAppDto projectApp = new AddProjectAppDto
            {
                Name = name ?? "",
                Identity = id,
                LabelCode = GetLabel(id),
                TeamIds = new() { masaStackConfig.GetDefaultTeamId() },
                Description = ""
            };

            foreach (var app in project.AsObject())
            {
                if (app.Key == "id")
                {
                    continue;
                }
                projectApp.Apps.Add(GenAppDto(id, app));
            }

            projectApps.Add(projectApp);
        }

        return projectApps;
    }

    private static string GetLabel(string projectIdentity)
    {
        MasaStack.MasaStackIdLabelPairs.TryGetValue(projectIdentity, out string? label);
        return label ?? "Other";
    }

    private static AddAppDto GenAppDto(string projectId, KeyValuePair<string, System.Text.Json.Nodes.JsonNode?> keyValuePair)
    {
        var type = keyValuePair.Key.ToLower();
        AppTypes appType = AppTypes.UI;
        if (type == "web" || type == "sso")
        {
            appType = AppTypes.UI;
        }
        else if (type == "service")
        {
            appType = AppTypes.Service;

        }
        else if (type == "job" || type == "worker")
        {
            appType = AppTypes.Job;
        }
        var app = new AddAppDto
        {
            ServiceType = ServiceTypes.WebAPI,
            Type = appType,
            Identity = keyValuePair.Value?["id"]?.ToString() ?? "",
            Name = $"{ToTitle(projectId)} {ToTitle(type)}",//
            Description = ""
        };

        return app;

        static string ToTitle(string value)
        {
            if (value.Length > 3 || value.Equals(MasaStackApp.WEB.Name))
            {
                value = new CultureInfo("en-US", false).TextInfo.ToTitleCase(value);
            }
            else
            {
                value = value.ToUpper();
            }
            return value;
        }
    }
}
