// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Components
{
    public class ColorChip : MChip
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (TextColor is null) return;

            var rgba = GenRgba(TextColor, 0.2f);
            Color = rgba;
        }

        private static string GenRgba(string hex, float opacity)
        {
            var color = System.Drawing.ColorTranslator.FromHtml(hex);
            var r = Convert.ToInt16(color.R);
            var g = Convert.ToInt16(color.G);
            var b = Convert.ToInt16(color.B);

            return $"rgba({r},{g},{b},{opacity})";
        }
    }
}
