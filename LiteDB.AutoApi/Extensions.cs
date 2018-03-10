using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LiteDB.AutoApi
{
    public static class Extensions
    {
        public static IMvcBuilder AddAutoApi<T>(this IMvcBuilder mvcBuilder, string route = null) where T : LiteDbModel
        {
            AssemblyName assemblyName = typeof(BaseController<>).Assembly.GetName();

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            Type newControllerParentType = typeof(BaseController<>).MakeGenericType(typeof(T));

            string newControllerName = $"{typeof(T).Name}Controller"; 
            
            TypeBuilder typeBuilder = moduleBuilder.DefineType(newControllerName, TypeAttributes.Public, newControllerParentType);

            if (!string.IsNullOrEmpty(route))
            {
                ConstructorInfo constructorInfo = typeof(RouteAttribute).GetConstructor(new[] {typeof(string)});
                CustomAttributeBuilder routeAttributeBuilder = new CustomAttributeBuilder(constructorInfo, new object[]{route});
                typeBuilder.SetCustomAttribute(routeAttributeBuilder);
            }
            
            Type newType = typeBuilder.CreateType();
            
            mvcBuilder.AddApplicationPart(newType.Assembly).AddControllersAsServices();
            
            return mvcBuilder;
        }
    }
}