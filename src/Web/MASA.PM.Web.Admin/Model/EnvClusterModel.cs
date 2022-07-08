// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using FluentValidation;

namespace MASA.PM.UI.Admin.Model
{
    public class EnvModel
    {
        public List<EnvClusterModel> Environments { get; set; } = new();
    }

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

    class EnvModelValidator : AbstractValidator<EnvModel>
    {
        public EnvModelValidator()
        {
            RuleForEach(o => o.Environments).SetValidator(new EnvClusterModelValidator());
        }
    }

    class EnvClusterModelValidator : AbstractValidator<EnvClusterModel>
    {
        public EnvClusterModelValidator()
        {
            RuleFor(o => o.Name).NotEmpty().WithMessage("environment name is required");
            RuleFor(o => o.Name).Matches(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$").WithMessage("Please enter [Chinese, English、and - _ symbols]");
            RuleFor(o => o.Name).MinimumLength(2).MaximumLength(50).WithMessage("environment name length range is [2-50]");
        }
    }
}
