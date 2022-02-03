﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace XtremeIdiots.Portal.DataLib
{
    public partial class GameServer
    {
        public GameServer()
        {
            ChatMessages = new HashSet<ChatMessage>();
            GameServerEvents = new HashSet<GameServerEvent>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(50)]
        public string GameType { get; set; }
        [Required]
        [StringLength(50)]
        public string IpAddress { get; set; }
        public int QueryPort { get; set; }
        public bool HasFtp { get; set; }

        [InverseProperty(nameof(ChatMessage.GameServer))]
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        [InverseProperty(nameof(GameServerEvent.GameServer))]
        public virtual ICollection<GameServerEvent> GameServerEvents { get; set; }
    }
}