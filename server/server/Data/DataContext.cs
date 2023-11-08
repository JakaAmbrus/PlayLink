﻿using WebAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Configurations;

namespace WebAPI.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
          IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
          IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //AppUser
            builder.ApplyConfiguration(new AppUserConfiguration());

            //AppUserRole
            builder.ApplyConfiguration(new AppRoleConfiguration());

            //Posts
            builder.ApplyConfiguration(new PostsConfiguration());

            //Comments
            builder.ApplyConfiguration(new CommentConfiguration());

            //Likes
            builder.ApplyConfiguration(new LikeConfiguration());
        
            //Notifications
            builder.ApplyConfiguration(new NotificationConfiguration());

            //FriendRequest
            builder.ApplyConfiguration(new FriendRequestConfiguration());
           
            //GroupChat
            builder.ApplyConfiguration(new GroupChatUserConfiguration());

            //GroupMessages
            builder.ApplyConfiguration(new GroupMessageConfiguration());

            //PrivatMessages
            builder.ApplyConfiguration(new PrivateMessageConfiguration());
        }
    }
}