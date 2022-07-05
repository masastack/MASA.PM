﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.UI.Admin.Global;

public class NavHelper
{
    private List<NavModel> _navList;
    private NavigationManager _navigationManager;

    public List<NavModel> Navs { get; } = new();

    public List<NavModel> SameLevelNavs { get; } = new();

    public List<PageTabItem> PageTabItems { get; } = new();

    public NavHelper(List<NavModel> navList, NavigationManager navigationManager)
    {
        _navList = navList;
        _navigationManager = navigationManager;
        Initialization();
    }

    private void Initialization()
    {
        _navList.ForEach(nav =>
        {
            if (nav.Hide is false) Navs.Add(nav);

            if (nav.Children is not null)
            {
                nav.Children = nav.Children.Where(c => c.Hide is false).ToArray();

                nav.Children.ForEach(child =>
                {
                    child.ParentId = nav.Id;
                    child.FullTitle = $"{nav.Title} {child.Title}";
                    child.ParentIcon = nav.Icon;
                });
            }
        });

        Navs.ForEach(nav =>
        {
            SameLevelNavs.Add(nav);
            if (nav.Children is not null) SameLevelNavs.AddRange(nav.Children);
        });

        SameLevelNavs.Where(nav => nav.Href is not null).ForEach(nav =>
        {
            PageTabItems.Add(new PageTabItem(nav.Title, nav.Href, nav.ParentIcon, PageTabsMatch.Prefix, nav.Target == "Self" ? PageTabsTarget.Self : PageTabsTarget.Blank)); /*nav.Href != GlobalVariables.DefaultRoute*/
        });
    }

    public void NavigateTo(NavModel nav)
    {
        Active(nav);
        _navigationManager.NavigateTo(nav.Href ?? "");
    }

    public void NavigateTo(string href)
    {
        var nav = SameLevelNavs.FirstOrDefault(n => n.Href == href);
        if (nav is not null) Active(nav);
        _navigationManager.NavigateTo(href);
    }

    public void RefreshRender(NavModel nav)
    {
        Active(nav);
    }

    public void NavigateToByEvent(NavModel nav)
    {
        RefreshRender(nav);
        _navigationManager.NavigateTo(nav.Href ?? "");
    }

    public void Active(NavModel nav)
    {
        SameLevelNavs.ForEach(n => n.Active = false);
        nav.Active = true;
        if (nav.ParentId != 0) SameLevelNavs.First(n => n.Id == nav.ParentId).Active = true;
    }
}

