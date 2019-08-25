using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;

namespace ManjuuInfrastructure.Repository.Mapper.Auto
{
    public class EntityAutoMapper
    {
        public static EntityAutoMapper Instance{get; private set;} = new EntityAutoMapper();

        private IEnumerable<Type> AutoMapperableTypes = null;
        static EntityAutoMapper()
        {
            Instance.GetAutoMapperTypes();
        }
        private EntityAutoMapper() { }



        private void GetAutoMapperTypes()
        {
            //获取类型信息
            Type entityAutoMapperType = this.GetType();
            TypeInfo typeInfo = entityAutoMapperType.GetTypeInfo();

            //根据类型获取程序集信息
            Assembly packageAssembly = typeInfo.Assembly;

            //获取实现了IAutoMapperable的类型
            this.AutoMapperableTypes = packageAssembly.GetTypes()
           .Where(p => (!p.IsAbstract) && typeof(IAutoMapperable).IsAssignableFrom(p));


        }
        public MapperConfiguration AutoMapperConfig(string entityType)
        {

            if ((null == this.AutoMapperableTypes) || (!this.AutoMapperableTypes.Any()))
            {
                return null;
            }

            Type currentType = this.AutoMapperableTypes.Where(p => p.Name == $"{entityType}AutoMapper").FirstOrDefault();

            if (null == currentType)
            {
                return null;
            }

            ConstructorInfo currentConstructorInfo = currentType.GetConstructor(Array.Empty<Type>());
            if (null == currentConstructorInfo)
            {
                return null;
            }

            return (currentConstructorInfo.Invoke(Array.Empty<object>()) as IAutoMapperable).MapperInitialize();

            // ConstructorInfo currentConstructorInfo = null;
            // foreach (var item in this.AutoMapperableTypes)
            // {
            //     //创建实例
            //     currentConstructorInfo = item.GetConstructor(null);
            //     if (null == currentConstructorInfo)
            //     {
            //         continue;
            //     }

            //     //对应初始化映射
            //     (currentConstructorInfo.Invoke(null) as IAutoMapperable).MapperInitialize();
            // }

        }

        public IMapper GetMapper(MapperConfiguration cfg)
        {
                if(null == cfg)
                {
                    return null;
                }

                return cfg.CreateMapper();
        }

        public T GetMapperResult<T>(MapperConfiguration cfg, object source)
        where T:class
        {
            IMapper mapper = this.GetMapper(cfg);

            if(null == mapper)
            {
                return null;
            }

           return mapper.Map<T>(source);
        }
    }
}