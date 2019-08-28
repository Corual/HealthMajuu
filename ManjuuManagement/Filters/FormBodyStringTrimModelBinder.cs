using ManjuuCommon.Tools;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManjuuManagement.Filters
{
    /// <summary>
    /// 自定义字符串去空格模型绑定
    /// </summary>
    public class FormBodyStringTrimModelBinder : IModelBinder
    {
        //formbody绑定的参数
        private readonly BodyModelBinder bodyModelBinder;

        public FormBodyStringTrimModelBinder(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
        {
            bodyModelBinder = new BodyModelBinder(formatters, readerFactory);
        }


        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException($"{nameof(bindingContext)} is null");
            }

            //调用原始body绑定数据
            bodyModelBinder.BindModelAsync(bindingContext);
            //判断是否设置了值
            if (!bindingContext.Result.IsModelSet)
            {
                return Task.CompletedTask;
            }

            //获取绑定对象
            var model = bindingContext.Result.Model;

            var stringPropertyInfo = model.GetType().GetProperties().Where(c => c.PropertyType == typeof(string));
            foreach (var property in stringPropertyInfo)
            {
                string value = StringHelper.StringWithTrim(property.GetValue(model)?.ToString()); 
                property.SetValue(model, value);
            }

            return Task.CompletedTask;

        }
    }
}
