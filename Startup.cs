﻿using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;
using ShoppingCartApi.Conventions;
using ShoppingCartApi.Models;
using ShoppingCartApi.Services;
using Swashbuckle.AspNetCore.Swagger;
namespace ShoppingCartApi

{

    public class Startup

    {

        public Startup(IConfiguration configuration)

        {

            Configuration = configuration;

        }



        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)

        {

            services.AddDbContext<ShoppingCartDbContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("ShoppingCartDbConnectionString"))
            );
            services.Configure<StkSetting>(options => Configuration.GetSection("StkSetting").Bind(options));
            services.AddTransient<IRepository<Order>, OrdersManager>();
            services.AddTransient<IRepository<ShipmentMethod>, ShipmentMethodManager>();
            services.AddTransient<IRepository<PaymentMethod>, PaymentMethodsManager>();
            services.AddTransient<IRepository<Product>, ProductsManager>();
            services.AddTransient<IRepository<Shopper>, ShoppersManager>();
            services.AddTransient<IRepository<Manufacturer>, ManufacturersManager>();
            services.AddTransient<IRepository<ProductCategory>, ProductCategoryManager>();
            services.Configure<ShoppingCartStkPushKey>(options => Configuration.GetSection("ShoppingCartStkPushKey").Bind(options));
            services.AddMvc(options =>
            {
                options.Conventions.Add(new ComplexTypeConvention());
            });

            services.AddSwaggerGen(c =>

            {

                c.SwaggerDoc("v1", new Info { Title = "Shopping Cart Api", Version = "v1" });

            });



        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)

        {

            if (env.IsDevelopment())

            {



                app.UseDeveloperExceptionPage();

            }

            app.UseCors(builder => {

                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build();

            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>

            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping Cart V1");

            });

            app.UseMvc();

        }

    }

}