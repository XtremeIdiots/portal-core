﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace XtremeIdiots.Portal.DataLib
{
    public partial class GameServerEvent
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string GameServerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Timestamp { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string EventType { get; set; }
        [Unicode(false)]
        public string EventData { get; set; }

        [ForeignKey(nameof(GameServerId))]
        [InverseProperty("GameServerEvents")]
        public virtual GameServer GameServer { get; set; }
    }
}