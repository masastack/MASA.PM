// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using System.Web;
using NGitLab;
using NGitLab.Models;

namespace MASA.PM.Web.Docs.Models;

public class DocFile
{
    public Sha1 Id { get; set; }

    public string Name { get; init; }

    public string Path { get; init; }

    public string NameWidthExtension { get; private set; }

    public string UrlEncodePathWithoutExtension { get; private set; }

    public string Direcotry { get; private set; }

    public DocFile(Sha1 id, string name, string path)
    {
        Id = id;
        Name = name;
        Path = path;

        ResolveFile();
    }

    public DocFile(Tree tree)
    {
        Id = tree.Id;
        Name = tree.Name;
        Path = tree.Path;

        ResolveFile();
    }

    private void ResolveFile()
    {
        var split = Path.Split("/");
        Direcotry = split[0];
        NameWidthExtension = Name.Replace(".md", "");
        UrlEncodePathWithoutExtension = HttpUtility.UrlEncode(Path.Replace(".md", ""));
    }
}
