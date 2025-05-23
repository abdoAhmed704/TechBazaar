﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Enums;
using TechBazaar.Core.Models;

namespace TechBazaar.Ef
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            try
            {
                var context = service.GetService<EContext>();

                var usermanage = service.GetService<UserManager<ApplicationUser>>();

                var roleManager = service.GetService<RoleManager<IdentityRole>>();


                var adminRoleExists = await roleManager.RoleExistsAsync(Role.Admin.ToString());

                if (!adminRoleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
                }

                // create user role if not exists
                var userRoleExists = await roleManager.RoleExistsAsync(Role.User.ToString());

                if (!userRoleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(Role.User.ToString()));
                }

                var Admin = new ApplicationUser
                {
                    UserName = "osama",
                    Email = "osama@gmail.com",
                    FirstName = "Osama",
                    LastName = "",
                    EmailConfirmed = true,
                    PhoneNumber = " ",
                    Street = "",
                    Floor = 0,
                    BuldingNo = 0,
                    AppartmentNo = 0,
                    City = "",
                    Country = "",
                    PostalCode = "",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,

                };

                if (!context.PaymentMethods.Any())
                {
                    var pymentMethods = new List<PaymentMethod>
                {
                    new PaymentMethod
                    {
                        Name = "Visa",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        Comission = "2.5%"
                    },
                    new PaymentMethod
                    {
                        Name = "MasterCard",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        Comission = "2.5%"
                        
                    },
                    new PaymentMethod
                    {
                        Name = "PayPal",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        Comission = "3.5%"
                    }
                };
                    context.PaymentMethods.AddRange(pymentMethods);
                    context.SaveChanges();
                }
                

                var userInDb = await usermanage.FindByEmailAsync(Admin.Email);

                if (userInDb is null)
                {
                    var result = await usermanage.CreateAsync(Admin, "Osama12@");
                    if (result.Succeeded)
                    {
                        await usermanage.AddToRoleAsync(Admin, "Admin");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


            }

        }
    }
}
