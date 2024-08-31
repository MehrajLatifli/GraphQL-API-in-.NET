using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schemas;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Services.Instructors;
using HotChocolate.AspNetCore;
using HotChocolate.Types.Introspection;
using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SQLitePCL;

namespace GraphQLDemo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Configure GraphQL Server
            builder.Services.AddGraphQLServer()
                .AddQueryType<Query>().SetPagingOptions(new PagingOptions { MaxPageSize = int.MaxValue - 1, DefaultPageSize = int.MaxValue - 1, IncludeTotalCount = true })
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddInMemorySubscriptions()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .AddAuthorization()
                .AddType<CourseType>()
                .AddType<InstructorType>()
                .AddTypeExtension<ExtendingQuery>();



            // Configure DbContext
            var configuration = builder.Configuration;

            builder.Services.AddPooledDbContextFactory<SchoolDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("default")));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
        

            builder.Services.AddAuthorization(o=>o.AddPolicy("IsAdmin", p=>p.RequireClaim(FirebaseUserClaimType.EMAIL)));

            builder.Services.AddSingleton(FirebaseApp.Create());
            builder.Services.AddFirebaseAuthentication();


            builder.Services.AddScoped<CoursesRepository>();
            builder.Services.AddScoped<InstructorRepository>();
            builder.Services.AddScoped<InstructorDataLoader>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
            app.UseWebSockets();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL();
            });

            app.Run();
        }
    }
}
