using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DesafioPitang.Data;
using DesafioPitang.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace DesafioPitang
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
            //Adicionando o pacote newtonsoftjson para poder recuperar os telefones do usuário ao ser transformado em JSON
            services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Configurando o banco de dados na memória
            services.AddScoped<IUserRepository, SqlUserRepository>();
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("Desafio"));
            //Recuperando a chave secreta e adicionando autenticação
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //Adicionando autenticação e autorização no pipeline
            app.UseAuthentication();
            app.UseAuthorization();
            //chamando um método auxiliar para popular o banco de dados em memória
            var context = serviceProvider.GetService<ApiContext>();
            AddTestData(context);

        }

        //Método auxiliar para popular o banco de dados em memória
        private static void AddTestData(ApiContext context)
        {
            var testUser1 = new User
            {
                Id = 1,
                FirstName = "Jon",
                LastName = "Jones",
                Email = "jv@email.com",
                Password = "123",
                Phones = new List<Phone>(),
                CreatedAt = DateTime.Now
            };

            context.Users.Add(testUser1);

            var testPhone = new Phone
            {
                Id = 1,
                UserId = testUser1.Id,
                Number = 123,
                AreaCode = 81,
                CountryCode = "+55"
            };

            context.Phones.Add(testPhone);

            context.SaveChanges();
        }
    }
}
