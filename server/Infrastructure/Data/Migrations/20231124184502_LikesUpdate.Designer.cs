﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231124184502_LikesUpdate")]
    partial class LikesUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("ProfilePicturePublicId")
                        .HasColumnType("text");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.AppUserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CommentId"));

                    b.Property<int>("AppUserId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<bool>("IsLikedByCurrentUser")
                        .HasColumnType("boolean");

                    b.Property<int>("PostId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeCommented")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("CommentId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Domain.Entities.FriendRequest", b =>
                {
                    b.Property<int>("FriendRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FriendRequestId"));

                    b.Property<int>("ReceiverId")
                        .HasColumnType("integer");

                    b.Property<int>("SenderId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeSent")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("FriendRequestId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("FriendRequest");
                });

            modelBuilder.Entity("Domain.Entities.Friendship", b =>
                {
                    b.Property<int>("FriendshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FriendshipId"));

                    b.Property<DateTime>("DateEstablished")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("User1Id")
                        .HasColumnType("integer");

                    b.Property<int>("User2Id")
                        .HasColumnType("integer");

                    b.HasKey("FriendshipId");

                    b.HasIndex("User2Id");

                    b.HasIndex("User1Id", "User2Id")
                        .IsUnique();

                    b.ToTable("Friendship");
                });

            modelBuilder.Entity("Domain.Entities.GroupChat", b =>
                {
                    b.Property<int>("GroupChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GroupChatId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("GroupChatId");

                    b.ToTable("GroupChat");
                });

            modelBuilder.Entity("Domain.Entities.GroupChatUser", b =>
                {
                    b.Property<int>("AppUserId")
                        .HasColumnType("integer");

                    b.Property<int>("GroupChatId")
                        .HasColumnType("integer");

                    b.HasKey("AppUserId", "GroupChatId");

                    b.HasIndex("GroupChatId");

                    b.ToTable("GroupChatUser");
                });

            modelBuilder.Entity("Domain.Entities.GroupMessage", b =>
                {
                    b.Property<int>("GroupMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GroupMessageId"));

                    b.Property<int>("AppUserId")
                        .HasColumnType("integer");

                    b.Property<int>("GroupChatId")
                        .HasColumnType("integer");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<DateTime>("MessageTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("GroupMessageId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("GroupChatId");

                    b.ToTable("GroupMessage");
                });

            modelBuilder.Entity("Domain.Entities.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LikeId"));

                    b.Property<int>("AppUserId")
                        .HasColumnType("integer");

                    b.Property<int?>("CommentId")
                        .HasColumnType("integer");

                    b.Property<int?>("PostId")
                        .HasColumnType("integer");

                    b.HasKey("LikeId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("Domain.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AppUserId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("NotificationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PostId")
                        .HasColumnType("integer");

                    b.Property<int>("SenderId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("PostId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Domain.Entities.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PostId"));

                    b.Property<int>("AppUserId")
                        .HasColumnType("integer");

                    b.Property<int>("CommentsCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsLikedByCurrentUser")
                        .HasColumnType("boolean");

                    b.Property<string>("PhotoPublicId")
                        .HasColumnType("text");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.HasKey("PostId");

                    b.HasIndex("AppUserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Domain.Entities.PrivateMessage", b =>
                {
                    b.Property<int>("PrivateMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PrivateMessageId"));

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DateRead")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("PrivateMessageSent")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("RecipientDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("RecipientId")
                        .HasColumnType("integer");

                    b.Property<string>("RecipientUsername")
                        .HasColumnType("text");

                    b.Property<bool>("SenderDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("SenderId")
                        .HasColumnType("integer");

                    b.Property<string>("SenderUsername")
                        .HasColumnType("text");

                    b.HasKey("PrivateMessageId");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("PrivateMessage");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.AppUserRole", b =>
                {
                    b.HasOne("Domain.Entities.AppRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AppUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "AppUser")
                        .WithMany("Comments")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Domain.Entities.FriendRequest", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "Receiver")
                        .WithMany("ReceivedFriendRequests")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AppUser", "Sender")
                        .WithMany("SentFriendRequests")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.Entities.Friendship", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "User1")
                        .WithMany("FriendsAsUser1")
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AppUser", "User2")
                        .WithMany("FriendsAsUser2")
                        .HasForeignKey("User2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User1");

                    b.Navigation("User2");
                });

            modelBuilder.Entity("Domain.Entities.GroupChatUser", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "AppUser")
                        .WithMany("GroupChatUsers")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.GroupChat", "GroupChat")
                        .WithMany("GroupChatUsers")
                        .HasForeignKey("GroupChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("GroupChat");
                });

            modelBuilder.Entity("Domain.Entities.GroupMessage", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "AppUser")
                        .WithMany("GroupMessages")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.GroupChat", "GroupChat")
                        .WithMany("GroupMessages")
                        .HasForeignKey("GroupChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("GroupChat");
                });

            modelBuilder.Entity("Domain.Entities.Like", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "AppUser")
                        .WithMany("Likes")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Comment", "Comment")
                        .WithMany("Likes")
                        .HasForeignKey("CommentId");

                    b.HasOne("Domain.Entities.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId");

                    b.Navigation("AppUser");

                    b.Navigation("Comment");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Domain.Entities.Notification", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "AppUser")
                        .WithMany("Notifications")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Post", "Post")
                        .WithMany("Notifications")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Domain.Entities.Post", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "AppUser")
                        .WithMany("Posts")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("Domain.Entities.PrivateMessage", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "Recipient")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AppUser", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Domain.Entities.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.AppRole", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Domain.Entities.AppUser", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("FriendsAsUser1");

                    b.Navigation("FriendsAsUser2");

                    b.Navigation("GroupChatUsers");

                    b.Navigation("GroupMessages");

                    b.Navigation("Likes");

                    b.Navigation("MessagesReceived");

                    b.Navigation("MessagesSent");

                    b.Navigation("Notifications");

                    b.Navigation("Posts");

                    b.Navigation("ReceivedFriendRequests");

                    b.Navigation("SentFriendRequests");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Navigation("Likes");
                });

            modelBuilder.Entity("Domain.Entities.GroupChat", b =>
                {
                    b.Navigation("GroupChatUsers");

                    b.Navigation("GroupMessages");
                });

            modelBuilder.Entity("Domain.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
