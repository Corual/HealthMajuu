using AutoMapper;
using ManjuuCommon.Tools;
using ManjuuDomain.Dto;
using ManjuuDomain.HealthCheck;
using ManjuuInfrastructure.Repository.Entity;
using System;
using System.Collections.Generic;

namespace ManjuuInfrastructure.Repository.Mapper.Auto
{
    public class JobConfigurationAutoMapper : IAutoMapperable
    {
        public MapperConfiguration MapperInitialize()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<JobConfiguration, CheckConfig>();
                cfg.CreateMap<DateTime?, string>().ConvertUsing<NullableDateTime2StringConverter>();
                cfg.CreateMap<JobConfiguration, ToolConfigDto>();//.ConvertUsing<JobConfiguration2ToolConfigDtoConverter>();
                cfg.CreateMap<string, DateTime?>().ConvertUsing<String2NullableDateTimeConverter>();
                cfg.CreateMap<ToolConfigDto, CheckConfig>().ConvertUsing<ToolConfigDto2CheckConfigConverter>();
                cfg.CreateMap<CheckConfig, JobConfiguration>();
            });

        }


        #region NullableDateTime2StringConverter
        private class NullableDateTime2StringConverter : ITypeConverter<DateTime?, string>
        {
            public string Convert(DateTime? source, string destination, ResolutionContext context)
            {
                return source.HasValue ? source.Value.ToString("HH:mm") : string.Empty;
            }
        }
        #endregion

        #region String2NullableDateTimeConverter
        private class String2NullableDateTimeConverter : ITypeConverter<string, DateTime?>
        {
            public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
            {
                if (string.IsNullOrWhiteSpace(source))
                {
                    return null;
                }

                DateTime dateTime;
                if (!DateTime.TryParse($"{TimeMgr.GetLoaclDateTime().ToString("yyyy-MM-dd")} {source}", out dateTime))
                {
                    return null;
                }

                return dateTime;
            }
        }
        #endregion

        #region ToolConfigDto2CheckConfigConverter
        private class ToolConfigDto2CheckConfigConverter : ITypeConverter<ToolConfigDto, CheckConfig>
        {
            public CheckConfig Convert(ToolConfigDto source, CheckConfig destination, ResolutionContext context)
            {
                if (null == source) { return null; }

                var converter = new String2NullableDateTimeConverter();

                return new CheckConfig(
                    source.Id,
                    converter.Convert(source.StartToWrokTime, DateTime.MinValue, null),
                    converter.Convert(source.StopToWorkTime, DateTime.MinValue, null),
                   source.WorkSpan,
                   source.PresetTimeout,
                   source.PingSendCount
                    );
            }
        }
        #endregion

        #region JobConfiguration2ToolConfigDtoConverter
        //private class JobConfiguration2ToolConfigDtoConverter : ITypeConverter<JobConfiguration, ToolConfigDto>
        //{
        //    public ToolConfigDto Convert(JobConfiguration source, ToolConfigDto destination, ResolutionContext context)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        #endregion
    }



}