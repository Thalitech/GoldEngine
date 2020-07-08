﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TheGodfather.Database;

namespace TheGodfather.Migrations
{
    [DbContext(typeof(TheGodfatherDbContext))]
    partial class TheGodfatherDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("gf")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TheGodfather.Database.Models.BlockedChannel", b =>
                {
                    b.Property<long>("ChannelIdDb")
                        .HasColumnName("cid")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("reason")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64)
                        .HasDefaultValue(null);

                    b.HasKey("ChannelIdDb");

                    b.ToTable("blocked_channels");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.BlockedUser", b =>
                {
                    b.Property<long>("UserIdDb")
                        .HasColumnName("uid")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("reason")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64)
                        .HasDefaultValue(null);

                    b.HasKey("UserIdDb");

                    b.ToTable("blocked_users");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.BotStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Activity")
                        .HasColumnName("activity_type")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("status")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("bot_statuses");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.EmojiReaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("GuildIdDb")
                        .HasColumnName("gid")
                        .HasColumnType("bigint");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnName("reaction")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("GuildIdDb");

                    b.ToTable("reactions_emoji");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.EmojiReactionTrigger", b =>
                {
                    b.Property<int>("ReactionId")
                        .HasColumnName("id")
                        .HasColumnType("integer");

                    b.Property<string>("Trigger")
                        .HasColumnName("trigger")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.HasKey("ReactionId", "Trigger");

                    b.ToTable("reactions_emoji_triggers");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.ExemptedAntispamEntity", b =>
                {
                    b.Property<long>("IdDb")
                        .HasColumnName("xid")
                        .HasColumnType("bigint");

                    b.Property<long>("GuildIdDb")
                        .HasColumnName("gid")
                        .HasColumnType("bigint");

                    b.Property<byte>("Type")
                        .HasColumnName("type")
                        .HasColumnType("smallint");

                    b.HasKey("IdDb", "GuildIdDb", "Type");

                    b.HasIndex("GuildIdDb");

                    b.ToTable("exempt_antispam");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.ExemptedLoggingEntity", b =>
                {
                    b.Property<long>("IdDb")
                        .HasColumnName("xid")
                        .HasColumnType("bigint");

                    b.Property<long>("GuildIdDb")
                        .HasColumnName("gid")
                        .HasColumnType("bigint");

                    b.Property<byte>("Type")
                        .HasColumnName("type")
                        .HasColumnType("smallint");

                    b.HasKey("IdDb", "GuildIdDb", "Type");

                    b.HasIndex("GuildIdDb");

                    b.ToTable("exempt_logging");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.ExemptedRatelimitEntity", b =>
                {
                    b.Property<long>("IdDb")
                        .HasColumnName("xid")
                        .HasColumnType("bigint");

                    b.Property<long>("GuildIdDb")
                        .HasColumnName("gid")
                        .HasColumnType("bigint");

                    b.Property<byte>("Type")
                        .HasColumnName("type")
                        .HasColumnType("smallint");

                    b.HasKey("IdDb", "GuildIdDb", "Type");

                    b.HasIndex("GuildIdDb");

                    b.ToTable("exempt_ratelimit");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.Filter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("GuildIdDb")
                        .HasColumnName("gid")
                        .HasColumnType("bigint");

                    b.Property<string>("TriggerString")
                        .IsRequired()
                        .HasColumnName("trigger")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("GuildIdDb");

                    b.ToTable("filters");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.GuildConfig", b =>
                {
                    b.Property<long>("GuildIdDb")
                        .HasColumnName("gid")
                        .HasColumnType("bigint");

                    b.Property<short>("AntiInstantLeaveCooldown")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("antiinstantleave_cooldown")
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)3);

                    b.Property<bool>("AntiInstantLeaveEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("antiinstantleave_enabled")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<byte>("AntifloodAction")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("antiflood_action")
                        .HasColumnType("smallint")
                        .HasDefaultValue((byte)4);

                    b.Property<short>("AntifloodCooldown")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("antiflood_cooldown")
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)10);

                    b.Property<bool>("AntifloodEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("antiflood_enabled")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<short>("AntifloodSensitivity")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("antiflood_sensitivity")
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)5);

                    b.Property<byte>("AntispamAction")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("antispam_action")
                        .HasColumnType("smallint")
                        .HasDefaultValue((byte)0);

                    b.Property<bool>("AntispamEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("antispam_enabled")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<short>("AntispamSensitivity")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("antispam_sensitivity")
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)5);

                    b.Property<string>("Currency")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("currency")
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32)
                        .HasDefaultValue(null);

                    b.Property<long?>("LeaveChannelIdDb")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("leave_cid")
                        .HasColumnType("bigint")
                        .HasDefaultValue(null);

                    b.Property<string>("LeaveMessage")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("leave_msg")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128)
                        .HasDefaultValue(null);

                    b.Property<bool>("LinkfilterBootersEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("linkfilter_booters")
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<bool>("LinkfilterDiscordInvitesEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("linkfilter_invites")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("LinkfilterDisturbingWebsitesEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("linkfilter_disturbing")
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<bool>("LinkfilterEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("linkfilter_enabled")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("LinkfilterIpLoggersEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("linkfilter_loggers")
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<bool>("LinkfilterUrlShortenersEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("linkfilter_shorteners")
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Locale")
                        .HasColumnName("locale")
                        .HasColumnType("character varying(8)")
                        .HasMaxLength(8);

                    b.Property<long?>("LogChannelIdDb")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("log_cid")
                        .HasColumnType("bigint")
                        .HasDefaultValue(null);

                    b.Property<long?>("MuteRoleIdDb")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("mute_rid")
                        .HasColumnType("bigint")
                        .HasDefaultValue(null);

                    b.Property<string>("Prefix")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("prefix")
                        .HasColumnType("character varying(8)")
                        .HasMaxLength(8)
                        .HasDefaultValue(null);

                    b.Property<byte>("RatelimitAction")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ratelimit_action")
                        .HasColumnType("smallint")
                        .HasDefaultValue((byte)1);

                    b.Property<bool>("RatelimitEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ratelimit_enabled")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<short>("RatelimitSensitivity")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ratelimit_sensitivity")
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)5);

                    b.Property<bool>("ReactionResponse")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("silent_response_enabled")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("SuggestionsEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("suggestions_enabled")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<long?>("WelcomeChannelIdDb")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("welcome_cid")
                        .HasColumnType("bigint")
                        .HasDefaultValue(null);

                    b.Property<string>("WelcomeMessage")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("welcome_msg")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128)
                        .HasDefaultValue(null);

                    b.HasKey("GuildIdDb");

                    b.ToTable("guild_cfg");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.TextReaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("GuildIdDb")
                        .HasColumnName("gid")
                        .HasColumnType("bigint");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnName("reaction")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("GuildIdDb");

                    b.ToTable("reactions_text");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.TextReactionTrigger", b =>
                {
                    b.Property<int>("ReactionId")
                        .HasColumnName("id")
                        .HasColumnType("integer");

                    b.Property<string>("Trigger")
                        .HasColumnName("trigger")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.HasKey("ReactionId", "Trigger");

                    b.ToTable("reactions_text_triggers");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.XpCount", b =>
                {
                    b.Property<long>("UserIdDb")
                        .HasColumnName("uid")
                        .HasColumnType("bigint");

                    b.Property<int>("XpDb")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("xp")
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.HasKey("UserIdDb");

                    b.ToTable("xp_count");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.XpRank", b =>
                {
                    b.Property<long>("GuildIdDb")
                        .HasColumnName("gid")
                        .HasColumnType("bigint");

                    b.Property<short>("Rank")
                        .HasColumnName("rank")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32);

                    b.HasKey("GuildIdDb", "Rank");

                    b.ToTable("guild_ranks");
                });

            modelBuilder.Entity("TheGodfather.Database.Models.EmojiReaction", b =>
                {
                    b.HasOne("TheGodfather.Database.Models.GuildConfig", "GuildConfig")
                        .WithMany("EmojiReactions")
                        .HasForeignKey("GuildIdDb")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheGodfather.Database.Models.EmojiReactionTrigger", b =>
                {
                    b.HasOne("TheGodfather.Database.Models.EmojiReaction", "Reaction")
                        .WithMany("DbTriggers")
                        .HasForeignKey("ReactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheGodfather.Database.Models.ExemptedAntispamEntity", b =>
                {
                    b.HasOne("TheGodfather.Database.Models.GuildConfig", "GuildConfig")
                        .WithMany()
                        .HasForeignKey("GuildIdDb")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheGodfather.Database.Models.ExemptedLoggingEntity", b =>
                {
                    b.HasOne("TheGodfather.Database.Models.GuildConfig", "GuildConfig")
                        .WithMany()
                        .HasForeignKey("GuildIdDb")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheGodfather.Database.Models.ExemptedRatelimitEntity", b =>
                {
                    b.HasOne("TheGodfather.Database.Models.GuildConfig", "GuildConfig")
                        .WithMany()
                        .HasForeignKey("GuildIdDb")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheGodfather.Database.Models.Filter", b =>
                {
                    b.HasOne("TheGodfather.Database.Models.GuildConfig", "GuildConfig")
                        .WithMany("Filters")
                        .HasForeignKey("GuildIdDb")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheGodfather.Database.Models.TextReaction", b =>
                {
                    b.HasOne("TheGodfather.Database.Models.GuildConfig", "GuildConfig")
                        .WithMany("TextReactions")
                        .HasForeignKey("GuildIdDb")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheGodfather.Database.Models.TextReactionTrigger", b =>
                {
                    b.HasOne("TheGodfather.Database.Models.TextReaction", "Reaction")
                        .WithMany("DbTriggers")
                        .HasForeignKey("ReactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheGodfather.Database.Models.XpRank", b =>
                {
                    b.HasOne("TheGodfather.Database.Models.GuildConfig", "GuildConfig")
                        .WithMany()
                        .HasForeignKey("GuildIdDb")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
