﻿using Microsoft.Extensions.Http;
using System.Application.Services;
using System.Application.Services.Implementation;
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加适用于iOS的原生 <see cref="IHttpClientFactory"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNativeHttpClient(this IServiceCollection services)
        {
            // https://github.com/dotnet/runtime/blob/v5.0.4/src/libraries/Microsoft.Extensions.Http/src/DependencyInjection/HttpClientFactoryServiceCollectionExtensions.cs#L36
            services.AddTransient<HttpMessageHandlerBuilder, PlatformHttpMessageHandlerBuilder>();
            services.AddHttpClient();
            return services;
        }

        /// <summary>
        /// 添加适用于iOS的 <see cref="IHttpPlatformHelper"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPlatformHttpPlatformHelper(this IServiceCollection services)
        {
            services.AddSingleton<IHttpPlatformHelper, PlatformHttpPlatformHelper>();
            return services;
        }

        /// <summary>
        /// 尝试添加适用于iOS平台的Toast
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection TryAddToast(this IServiceCollection services)
            => PlatformToastImpl.TryAddToast(services);

        /// <summary>
        /// 添加电话服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTelephonyService(this IServiceCollection services)
        {
            services.AddSingleton<ITelephonyService, PlatformTelephonyServiceImpl>();
            return services;
        }

        /// <summary>
        /// 添加平台权限
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPlatformPermissions(this IServiceCollection services)
        {
            return services;
        }
    }
}