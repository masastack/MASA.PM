// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.UI.Admin.Model
{
    public class EnvClusterModel
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public string Color { get; set; } = "";

        public int Index { get; set; }

        public EnvClusterModel(int index, string name, string description, string color)
        {
            Index = index;
            Name = name;
            Description = description;
            Color = color;
        }

        public EnvClusterModel(int index)
        {
            Index = index;
        }
    }
}
