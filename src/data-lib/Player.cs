﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace XtremeIdiots.Portal.DataLib
{
    public partial class Player
    {
        public Player()
        {
            ChatMessages = new HashSet<ChatMessage>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string GameType { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        public string Guid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FirstSeen { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastSeen { get; set; }
        [StringLength(50)]
        public string IpAddress { get; set; }

        [InverseProperty(nameof(ChatMessage.Player))]
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}