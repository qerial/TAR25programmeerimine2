using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using OnlineShopTAR25.RealEstateTest.Macros;
using ShopTarpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Authentication.ExtendedProtection;
using System.Text;

namespace OnlineShopTAR25.RealEstateTest
{
    public abstract class TestBase
    {

        //
        protected IServiceProvider ServiceProvider { get; set; }

        protected TestBase()
        {
            //loob uue teenuse kogu vajalikuks teenuste testimiseks 
            var services = new ServiceCollection();
            //kutsub välja meetodi
            SetupServices(services);
            //selle kaudu saab teenuseid küsida
            ServiceProvider = services.BuildServiceProvider();
        }
        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IRealEstateServices, RealEstateServices>();

            //registreerib andmebaasi konteksti
            services.AddDbContext<ShopTARpe25Context>(x =>
            {
                //seadistab kasutama mälusisest andmebaasi nimega TEST, et
                //testimisel kasutada ilma päris andmebaasita
                x.UseInMemoryDatabase("TEST");

                //eirab hoiatust, et mälusisene DB ei toeta tehinguid samamoodi
                //nagu päris andmebaas
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            //kutsub välja macrode classi
            RegisterMacros(services);
        }

        //vabastab ressursid peale kasutamist
        public void Dispose()
        {

        }
        //abimeetod, millega klass ja selle alamklassid saavad teenuse tööbi
        //järgi küsida
        protected T Svc<T>()
        {
            //otsib teenuspakkuja tüüpi
            return ServiceProvider.GetService<T>();
        }

        //abimeetod makrode leidmiseks ja registreerimiseks
        private void RegisterMacros(IServiceCollection services)
        {
            //saaks reflektsiooniga otsingus kasutada
            var macroBaseType = typeof(IMacros);

            //loeb kokku kõik tüübid selles assemblyst
            var macros = macroBaseType.Assembly.GetTypes()
                //valib tüübid, mis rakendavad IMacros liidest, aga ei ole ise liidesed
                //ega abstraktsed klassid
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);

            //käib leitud makrode classid ükshaaval läbi
            foreach(var macro in macros)
            {
                //kasutab ühte makro eksemplaari
                services.AddSingleton(macro);
            }
        }
    }
}
