using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using Application.Common.ErrorMessages;
using Application.Common.Exceptions;

using FluentValidation.Results;
using System.Collections.Generic;

namespace Application.Common.ModelBinderProviders
{
    public class NullModelBinderProvider : IModelBinderProvider
    {
        private readonly Type[] _allowedTypes = new Type[] { typeof(string),
                                                             typeof(double), typeof(double?),
                                                             typeof(decimal), typeof(decimal?),
                                                             typeof(float), typeof(float?),
                                                             typeof(bool), typeof(bool?),
                                                             typeof(int), typeof(int?),
                                                             typeof(DateTime), typeof(DateTime?)
                                                           };


        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (_allowedTypes.Any(allowedType => allowedType == context.Metadata.ModelType))
            {
                return new NullOrInvalidModelBinder();
            }

            return null;
        }
    }

    public class NullOrInvalidModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;

            if (value == "null" && bindingContext.ModelType == typeof(string))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            else if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) && value != "null" && bindingContext.ModelType != typeof(string))
            {
                ValidValue(bindingContext, bindingContext.ModelName, value);
            }
            else if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) && bindingContext.ModelType == typeof(string))
            {
                bindingContext.Result = ModelBindingResult.Success(value.Trim());
            }

            return Task.CompletedTask;
        }

        private static void ValidValue(ModelBindingContext bindingContext, string modelName, string value)
        {
            bool success = false;

            var parseMethods = new Dictionary<Type, Func<string, object>>
            {
                { typeof(int), x => int.TryParse(x, out var result) ? result : null },
                { typeof(int?), x => int.TryParse(x, out var result) ? result : null },
                { typeof(double), x => double.TryParse(x, out var result) ? result : null },
                { typeof(double?), x => double.TryParse(x, out var result) ? result : null },
                { typeof(decimal), x => decimal.TryParse(x, out var result) ? result : null },
                { typeof(decimal?), x => decimal.TryParse(x, out var result) ? result : null },
                { typeof(float), x => float.TryParse(x, out var result) ? result : null },
                { typeof(float?), x => float.TryParse(x, out var result) ? result : null },
                { typeof(bool), x => bool.TryParse(x, out var result) ? result : null },
                { typeof(bool?), x => bool.TryParse(x, out var result) ? result : null },
                { typeof(DateTime), x => DateTime.TryParse(x, out var result) ? result : null },
                { typeof(DateTime?), x => DateTime.TryParse(x, out var result) ? result : null }
            };

            if (parseMethods.TryGetValue(bindingContext.ModelType, out var parseMethod))
            {
                var result = parseMethod(value);
                if (result != null)
                {
                    success = true;
                    bindingContext.Result = ModelBindingResult.Success(result);
                }
            }

            if (!success)
            {
                throw new ValidationException(new ValidationFailure(modelName, PropertyErrorMessages.InvalidValue(modelName, value)));
            }
        }
    }
}