using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessLayer.Query.Mappers;
using BusinessLayer.Query.Queries;
using Client.Tushare;
using Dapper;
using Data.Contract;
using Data.Reader;
using Data.Writer;
using FundImporter.Bls;
using FundImporter.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace FundImporter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddTransient(typeof(ITypeReader<,>), typeof(TypeReader<,>))
                .AddTransient(typeof(IDateReader<,>), typeof(DateReader<,>))
                .AddSingleton<IFundMapper, FundMapper>()
                .AddSingleton<IFundCompanyMapper, FundCompanyMapper>()
                .AddSingleton<IFundManagerMapper, FundManagerMapper>()
                .AddSingleton<IIndexFundMapper, IndexFundMapper>()
                .AddSingleton<IFundDetailMapper, FundDetailMapper>()
                .AddSingleton<IMorningStarMapper, MorningStarMapper>()
                .AddSingleton<IShareholderMapper, ShareholderMapper>()
                .AddTransient<IFundQuery, FundQuery>()
                .AddTransient<IFundCompanyQuery, FundCompanyQuery>()
                .AddTransient<IFundManagerQuery, FundManagerQuery>()
                .AddTransient<IIndexFundQuery, IndexFundQuery>()
                .AddTransient<IFundDetailQuery, FundDetailQuery>()
                .AddTransient<IMorningStarQuery, MorningStarQuery>()
                .AddTransient<IShareholderQuery, ShareholderQuery>()
                .AddScoped<IFundBl, FundBl>()
                .AddScoped<IFundDetailBl, FundDetailBl>()
                .AddScoped<IFundManagerBl, FundManagerBl>()
                .AddScoped<IFundCompanyBl, FundCompanyBl>()
                .AddScoped<IMorningstarBl, MorningstarBl>()
                .AddScoped<ICsi300Bl, Csi300Bl>()
                .AddScoped<IShareholderBl, ShareholderBl>()
                .AddScoped<ICalculateBl, CalculateBl>()
                .AddScoped<ITushareClient, TushareClient>()
                .AddSingleton<IConvertService, ConvertService>()
                .AddScoped(typeof(ITypeWriter<,>), typeof(TypeWriter<,>))
                .AddScoped(typeof(IDateWriter<,>), typeof(DateWriter<,>))
                .AddSingleton<IDbContext>(x=> new DbContext("Host=localhost;Database=Fund;Username=postgres;Password=sa"))
                .AddTransient<IDbConnection>(x => new NpgsqlConnection("Host=localhost;Database=Fund;Username=postgres;Password=sa"))
                .BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var fundBl = scope.ServiceProvider.GetService<IFundBl>();
            var fundDetailBl = scope.ServiceProvider.GetService<IFundDetailBl>();
            var fundManagerBl = scope.ServiceProvider.GetService<IFundManagerBl>();
            var fundCompanyBl = scope.ServiceProvider.GetService<IFundCompanyBl>();
            var csi300Bl = scope.ServiceProvider.GetService<ICsi300Bl>();
            var calculateBl = scope.ServiceProvider.GetService<ICalculateBl>();
            var shareHolderBl = scope.ServiceProvider.GetService<IShareholderBl>();
            var morningstarBl = scope.ServiceProvider.GetService<IMorningstarBl>();
            //await shareHolderBl.RunAsync();
            //await fundBl.RunAsync();
            //await fundCompanyBl.RunAsync();
            //await fundManagerBl.RunAsync();
            //await csi300Bl.RunAsync();
            //await fundDetailBl.RunAsync();
            await calculateBl.RunAsync();
            //await morningstarBl.RunAsync();
        }
    }
}
