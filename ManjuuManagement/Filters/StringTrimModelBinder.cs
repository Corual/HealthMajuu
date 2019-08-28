using ManjuuCommon.Tools;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManjuuManagement.Filters
{
    /// <summary>
    /// 普通字符串参数去空格
    /// </summary>
    public class StringTrimModelBinder : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException($"{nameof(bindingContext)} is null");
            }

            //通过参数名获得参数
            var valueProvider = bindingContext.ValueProvider;
            var modelName = bindingContext.ModelName;
            var valueProviderResult = valueProvider.GetValue(modelName);

            //拿不到参数，直接返回
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }



            //然后将绑定的结果拿到
            var value = valueProviderResult.FirstValue;

            //去空格
            value = StringHelper.StringWithTrim(value);
            //设置参数值
            bindingContext.ModelState.SetModelValue(modelName, new ValueProviderResult(value));

            return Task.CompletedTask;

            ////替换原有ValueProvider
            //bindingContext.ValueProvider = new CompositeValueProvider() {
            //        new ElementalValueProvider(modelName, value, valueProviderResult.Culture),
            //        bindingContext.ValueProvider
            //    };

            //SimpleTypeModelBinder simpleTypeModelBinder = new SimpleTypeModelBinder(_type, (ILoggerFactory)bindingContext.HttpContext.RequestServices.GetService(typeof(ILoggerFactory)));
            //simpleTypeModelBinder.BindModelAsync(bindingContext);
        }
    }
}
