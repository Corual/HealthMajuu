using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManjuuManagement.Filters
{
    public class StringTrimModelBinderProvider : IModelBinderProvider
    {

        private readonly IList<IInputFormatter> _formatters;

        public StringTrimModelBinderProvider(IList<IInputFormatter> formatters)
        {
            _formatters = formatters;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {

            if (context == null)
            {
                throw new ArgumentNullException($"{nameof(context)} is null");
            }

            if (!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(string))
            {
                //简单类型
                return new StringTrimModelBinder();
            }
            else if (context.BindingInfo.BindingSource != null && context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Body))
            {
                //通过[FromBody]绑定的
                return new FormBodyStringTrimModelBinder(_formatters, context.Services.GetRequiredService<IHttpRequestStreamReaderFactory>());
            }

            return null;
        }
    }
}
