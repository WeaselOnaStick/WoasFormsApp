﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WoasFormsApp.Data;

#nullable disable

namespace WoasFormsApp.Migrations
{
    [DbContext(typeof(WoasFormsDbContext))]
    [Migration("20250212201630_HiddenFields RegistrationDate")]
    partial class HiddenFieldsRegistrationDate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TemplateTemplateTag", b =>
                {
                    b.Property<int>("TagsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TemplateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TagsId", "TemplateId");

                    b.HasIndex("TemplateId");

                    b.ToTable("TemplateTemplateTag");
                });

            modelBuilder.Entity("TemplateWoasFormsAppUser", b =>
                {
                    b.Property<int>("TemplateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UsersWhoLikedId")
                        .HasColumnType("TEXT");

                    b.HasKey("TemplateId", "UsersWhoLikedId");

                    b.HasIndex("UsersWhoLikedId");

                    b.ToTable("TemplateWoasFormsAppUser");
                });

            modelBuilder.Entity("TemplateWoasFormsAppUser1", b =>
                {
                    b.Property<string>("AllowedUsersId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Template1Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("AllowedUsersId", "Template1Id");

                    b.HasIndex("Template1Id");

                    b.ToTable("TemplateWoasFormsAppUser1");
                });

            modelBuilder.Entity("WoasFormsApp.Data.Response", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TemplateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("WoasFormsApp.Data.ResponseAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.PrimitiveCollection<string>("AnswerCheckedBoxes")
                        .HasColumnType("TEXT");

                    b.Property<string>("AnswerMultiLine")
                        .HasColumnType("TEXT");

                    b.Property<int?>("AnswerPositiveInt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AnswerSingleLine")
                        .HasColumnType("TEXT");

                    b.Property<int>("FieldId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ResponseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("ResponseId");

                    b.ToTable("ResponseAnswer");
                });

            modelBuilder.Entity("WoasFormsApp.Data.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CoverImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Public")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TopicId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("TopicId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("WoasFormsApp.Data.TemplateComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PostedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TemplateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.HasIndex("UserId");

                    b.ToTable("TemplateComments");
                });

            modelBuilder.Entity("WoasFormsApp.Data.TemplateField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Hidden")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ShowInAnalytics")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TemplateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.HasIndex("TypeId");

                    b.ToTable("TemplateFields");
                });

            modelBuilder.Entity("WoasFormsApp.Data.TemplateFieldType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FieldTypes");
                });

            modelBuilder.Entity("WoasFormsApp.Data.TemplateTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TemplateTags");
                });

            modelBuilder.Entity("WoasFormsApp.Data.TemplateTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TemplateTopics");
                });

            modelBuilder.Entity("WoasFormsApp.Data.WoasFormsAppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("RegisteredAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WoasFormsApp.Data.WoasFormsAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WoasFormsApp.Data.WoasFormsAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WoasFormsApp.Data.WoasFormsAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WoasFormsApp.Data.WoasFormsAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TemplateTemplateTag", b =>
                {
                    b.HasOne("WoasFormsApp.Data.TemplateTag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WoasFormsApp.Data.Template", null)
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TemplateWoasFormsAppUser", b =>
                {
                    b.HasOne("WoasFormsApp.Data.Template", null)
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WoasFormsApp.Data.WoasFormsAppUser", null)
                        .WithMany()
                        .HasForeignKey("UsersWhoLikedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TemplateWoasFormsAppUser1", b =>
                {
                    b.HasOne("WoasFormsApp.Data.WoasFormsAppUser", null)
                        .WithMany()
                        .HasForeignKey("AllowedUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WoasFormsApp.Data.Template", null)
                        .WithMany()
                        .HasForeignKey("Template1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WoasFormsApp.Data.Response", b =>
                {
                    b.HasOne("WoasFormsApp.Data.Template", "Template")
                        .WithMany("Responses")
                        .HasForeignKey("TemplateId");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("WoasFormsApp.Data.ResponseAnswer", b =>
                {
                    b.HasOne("WoasFormsApp.Data.TemplateField", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WoasFormsApp.Data.Response", null)
                        .WithMany("Answers")
                        .HasForeignKey("ResponseId");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("WoasFormsApp.Data.Template", b =>
                {
                    b.HasOne("WoasFormsApp.Data.WoasFormsAppUser", "Owner")
                        .WithMany("OwnedTemplates")
                        .HasForeignKey("OwnerId");

                    b.HasOne("WoasFormsApp.Data.TemplateTopic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("WoasFormsApp.Data.TemplateComment", b =>
                {
                    b.HasOne("WoasFormsApp.Data.Template", "Template")
                        .WithMany("Comments")
                        .HasForeignKey("TemplateId");

                    b.HasOne("WoasFormsApp.Data.WoasFormsAppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Template");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WoasFormsApp.Data.TemplateField", b =>
                {
                    b.HasOne("WoasFormsApp.Data.Template", "Template")
                        .WithMany("Fields")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WoasFormsApp.Data.TemplateFieldType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("WoasFormsApp.Data.Response", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("WoasFormsApp.Data.Template", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Fields");

                    b.Navigation("Responses");
                });

            modelBuilder.Entity("WoasFormsApp.Data.WoasFormsAppUser", b =>
                {
                    b.Navigation("OwnedTemplates");
                });
#pragma warning restore 612, 618
        }
    }
}
