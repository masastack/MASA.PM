// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.Entities;

public class BaseEntity<TKey, TUserKey> : ISoftDelete
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey Id { get; set; } = default!;

    public bool IsDeleted { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreationTime { get; set; } = DateTime.Now;

    [Required]
    public TUserKey Creator { get; set; } = default!;

    public DateTime ModificationTime { get; set; }

    [Required]
    public TUserKey Modifier { get; set; } = default!;
}
